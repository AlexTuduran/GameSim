//
// Area Mouse Handler
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OmniSARTechnologies.Helper {
	public enum MouseButton {
		Left = 0,
		Right,
		Middle,

        Count
	}

	public enum MousePositionSpace {
		ScreenSpace,
		WorldSpace,
		AreaSpace,
		AreaNormalizedSpace
	}

	[System.Serializable]
	public class AreaMouseHoverEvent : UnityEvent<AreaInfo> { } // areaInfo

	[System.Serializable]
	public class AreaMouseDownEvent : UnityEvent<AreaInfo, MouseButton> { } // areaInfo, mouseButton

	[System.Serializable]
	public class AreaMouseUpEvent : UnityEvent<AreaInfo, MouseButton, AreaInfo> { } // areaInfo, mouseButton, mouseDownAreaInfo

	[System.Serializable]
	public class AreaMouseDraggingEvent : UnityEvent<AreaInfo, MouseButton, AreaInfo> { } // areaInfo, mouseButton, mouseDownAreaInfo

    [System.Serializable]
    public class AreaMouseScrollEvent : UnityEvent<AreaInfo, float> { } // areaInfo, mouseScrollDelta

    [ExecuteInEditMode]
	public class AreaMouseHandler : MonoBehaviour {
        [Header("Events")]
        public AreaMouseHoverEvent onAreaMouseHover = null;
        public AreaMouseDownEvent onAreaMouseDown = null;
        public AreaMouseUpEvent onAreaMouseUp = null;
        public AreaMouseDraggingEvent onAreaMouseDragging = null;
        public AreaMouseScrollEvent onAreaMouseScroll = null;

        [Header("Area Handlers Refs.")]
        public AreaHandler[] areaHandlers = null;

        [Header("Debug")]
        public bool logMouseEvents = true;
        public bool ignoreInvalidArea = true;
        public bool logMouseHoverEvents = false;
        public bool logMouseDownEvents = false;
        public bool logMouseUpEvents = false;
        public bool logMouseDraggingEvents = false;
        public bool logMouseScrollEvents = false;

        private Vector2 m_LastMousePosition = Vector2.one * -1.0f;
		private int m_MouseButtonCount = System.Enum.GetValues(typeof(MouseButton)).Length;
		private AreaInfo[] m_MouseDownAreaInfo = null;

		private const int InvalidIndex = -1;

		private void Awake() {
			m_MouseDownAreaInfo = new AreaInfo[m_MouseButtonCount];
			for (int i = 0; i < m_MouseDownAreaInfo.Length; i++) {
                m_MouseDownAreaInfo[i].Reset();
			}
		}

		public int GetAreaIndexByAreaId(int areaId) {
			if (AreaHandler.kInvalidAreaId == areaId) {
				return InvalidIndex;
			}

			if (null == areaHandlers) {
				return InvalidIndex;
			}

			if (areaHandlers.Length < 1) {
				return InvalidIndex;
			}

			for (int i = 0; i < areaHandlers.Length; i++) {
				if (areaHandlers[i].areaId == areaId) {
					return i;
				}
			}

			return InvalidIndex;
		}

		public int ScreenSpaceToAreaId(Vector2 screenSpace) {
			if (null == areaHandlers) {
				return AreaHandler.kInvalidAreaId;
			}

			if (areaHandlers.Length < 1) {
				return AreaHandler.kInvalidAreaId;
			}

			for (int i = 0; i < areaHandlers.Length; i++) {
				if (areaHandlers[i].IsScreenSpaceInArea(screenSpace)) {
					return areaHandlers[i].areaId;
				}
			}

			return AreaHandler.kInvalidAreaId;
		}

		public void HandleMouse() {
			Vector2 mousePosition = Input.mousePosition;
            float mouseScrollDelta = Input.mouseScrollDelta.y;

			int areaId = ScreenSpaceToAreaId(mousePosition);
            int areaIndex = GetAreaIndexByAreaId(areaId);
            bool notInvalidArea = !(ignoreInvalidArea && (AreaHandler.kInvalidAreaId == areaId));
            bool mouseMoved = !Vector2.Equals(m_LastMousePosition, mousePosition);

			AreaInfo nowAreaInfo;
			nowAreaInfo.areaId                              = areaId;
            nowAreaInfo.mousePositionScreenSpace            = mousePosition;
            nowAreaInfo.mousePositionWorldSpace             = AreaHandler.ScreenSpaceToWorldSpace(nowAreaInfo.mousePositionScreenSpace);
            nowAreaInfo.mousePositionAreaSpace              = (InvalidIndex == areaIndex) ? Vector2.zero : areaHandlers[areaIndex].WorldSpaceToAreaSpace(nowAreaInfo.mousePositionWorldSpace);
            nowAreaInfo.mousePositionAreaNormalizedSpace    = (InvalidIndex == areaIndex) ? Vector2.zero : areaHandlers[areaIndex].AreaSpaceToAreaNormalizedSpace(nowAreaInfo.mousePositionAreaSpace);
            nowAreaInfo.screenSpaceBounds                   = (InvalidIndex == areaIndex) ? Rect.zero : areaHandlers[areaIndex].screenSpaceBounds;
            nowAreaInfo.worldSpaceBounds                    = (InvalidIndex == areaIndex) ? Rect.zero : areaHandlers[areaIndex].worldSpaceBounds;

            if (mouseMoved) {
                onAreaMouseHover.Invoke(nowAreaInfo);

                if (logMouseEvents && logMouseHoverEvents && notInvalidArea) {
                    Debug.LogFormat("onAreaMouseHover({0})", nowAreaInfo);
                }
            }

            if (Mathf.Abs(mouseScrollDelta) > Mathf.Epsilon) {
                onAreaMouseScroll.Invoke(nowAreaInfo, mouseScrollDelta);

                if (logMouseEvents && logMouseScrollEvents && notInvalidArea) {
                    Debug.LogFormat("onAreaMouseScroll({0}, {1})", nowAreaInfo, mouseScrollDelta);
                }
            }

			for (int mouseButton = 0; mouseButton < m_MouseButtonCount; mouseButton++) {
				if (Input.GetMouseButtonDown(mouseButton)) {
					onAreaMouseDown.Invoke(
						nowAreaInfo,
						(MouseButton)mouseButton
					);

                    if (logMouseEvents && logMouseDownEvents && notInvalidArea) {
                        Debug.LogFormat("onAreaMouseDown({0}, {1})", nowAreaInfo, (MouseButton)mouseButton);
                    }

					m_MouseDownAreaInfo[mouseButton] = nowAreaInfo;
				}

				if (Input.GetMouseButtonUp(mouseButton)) {
					onAreaMouseUp.Invoke(
						nowAreaInfo,
						(MouseButton)mouseButton,
						m_MouseDownAreaInfo[mouseButton]
					);

                    if (logMouseEvents && logMouseUpEvents && notInvalidArea) {
                        Debug.LogFormat("onAreaMouseUp({0}, {1}, {2})", nowAreaInfo, (MouseButton)mouseButton, m_MouseDownAreaInfo[mouseButton]);
                    }

					m_MouseDownAreaInfo[mouseButton].Reset();
				}

                if (Input.GetMouseButton(mouseButton) && mouseMoved) {
					onAreaMouseDragging.Invoke(
						nowAreaInfo,
						(MouseButton)mouseButton,
						m_MouseDownAreaInfo[mouseButton]
					);

                    if (logMouseEvents && logMouseDraggingEvents && notInvalidArea) {
                        Debug.LogFormat("onAreaMouseDragging({0}, {1}, {2})", nowAreaInfo, (MouseButton)mouseButton, m_MouseDownAreaInfo[mouseButton]);
                    }
				}
			}

            m_LastMousePosition = mousePosition;
		}

		private void Update() {
			HandleMouse();
		}
	}
}
