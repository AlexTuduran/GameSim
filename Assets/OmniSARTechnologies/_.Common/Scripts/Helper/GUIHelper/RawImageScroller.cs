//
// Raw Image Scroller
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OmniSARTechnologies.Helper {
    [RequireComponent(typeof(RawImage))]
    [ExecuteInEditMode]
    public class RawImageScroller : MonoBehaviour {
        public Vector2 position = Vector2.one;

        [Range(0.0f, 0.999f)]
        public float positionDampening = 0.95f;

        public Vector2 zoom = Vector2.one;
        public Vector2 zoomSpeed = Vector2.one * 0.1f;

        [Range(0.0f, 0.999f)]
        public float zoomDampening = 0.95f;

        private RawImage m_RawImage;
        private Vector2 m_RawImageInvertedRectSize = Vector2.one;
        private Rect m_UVRect = Rect.zero;
        private Vector2 m_LastPosition = Vector2.one;
        private Vector2 m_DampenedPosition = Vector2.one;
        private Vector2 m_LastZoom = Vector2.one;
        private Vector2 m_DampenedZoom = Vector2.one;

        public bool AcquireRawImage() {
            if (m_RawImage) {
                return true;
            }

            m_RawImage = GetComponent<RawImage>();

            if (!m_RawImage) {
                return false;
            }

            m_RawImageInvertedRectSize = new Vector2(
                1.0f / m_RawImage.rectTransform.rect.width,
                1.0f / m_RawImage.rectTransform.rect.height
            );

            return true;
        }

        public void OnEnable() {
            AcquireRawImage();
        }

        public void OnValidate() {
            UpdateAll();
        }

        public void ValidateProperties() {
            zoom.x = Mathf.Max(1.0f, zoom.x);
            zoom.y = Mathf.Max(1.0f, zoom.y);

            zoomSpeed.x = Mathf.Max(0.0f, zoomSpeed.x);
            zoomSpeed.y = Mathf.Max(0.0f, zoomSpeed.y);

            position.x = Mathf.Clamp01(position.x);
            position.y = Mathf.Clamp01(position.y);
        }

        public void UpdateUVRect() {
            m_DampenedZoom = Vector2.Lerp(zoom, m_DampenedZoom, zoomDampening);

            m_UVRect.size = new Vector2(
                1.0f / m_DampenedZoom.x,
                1.0f / m_DampenedZoom.y
            );

            m_DampenedPosition = Vector2.Lerp(position, m_DampenedPosition, positionDampening);

            m_UVRect.position = new Vector2(
                (1.0f - m_DampenedPosition.x) * (1.0f - m_UVRect.size.x),
                (1.0f - m_DampenedPosition.y) * (1.0f - m_UVRect.size.y)
            );

            if (AcquireRawImage()) {
                m_RawImage.uvRect = m_UVRect;
            }
        }

        public void UpdateAll() {
            ValidateProperties();
            UpdateUVRect();
        }

        public void OnAreaMouseHoverEvent(AreaInfo areaInfo) {
            
        }

        public void OnAreaMouseDownEvent(AreaInfo areaInfo, MouseButton mouseButton) {
            if (AreaHandler.kInvalidAreaId == areaInfo.areaId) {
                return;
            }

            if (MouseButton.Left == mouseButton) {
                m_LastPosition = position;
            }

            if (MouseButton.Right == mouseButton) {
                m_LastZoom = zoom;
            }

            if (MouseButton.Middle == mouseButton) {
                position = Vector2.one;
                zoom = Vector2.one;

                m_LastPosition = position;
                m_LastZoom = zoom;

                UpdateAll();
            }
        }

        public void OnAreaMouseUpEvent(AreaInfo areaInfo, MouseButton mouseButton, AreaInfo mouseDownAreaInfo) {
            
        }

        public void OnAreaMouseDraggingEvent(AreaInfo areaInfo, MouseButton mouseButton, AreaInfo mouseDownAreaInfo) {
            if (AreaHandler.kInvalidAreaId == mouseDownAreaInfo.areaId) {
                return;
            }

            float screenSpaceDeltaX = areaInfo.mousePositionScreenSpace.x - mouseDownAreaInfo.mousePositionScreenSpace.x;
            float screenSpaceDeltaY = areaInfo.mousePositionScreenSpace.y - mouseDownAreaInfo.mousePositionScreenSpace.y;

            bool update = false;

            if (MouseButton.Left == mouseButton) {
                float speedX = zoom.x > 1.0f ? 1.0f / (zoom.x - 1.0f) : 0.0f;
                float speedY = zoom.y > 1.0f ? 1.0f / (zoom.y - 1.0f) : 0.0f;
                position.x = m_LastPosition.x + screenSpaceDeltaX / mouseDownAreaInfo.screenSpaceBounds.size.x * speedX;
                position.y = m_LastPosition.y + screenSpaceDeltaY / mouseDownAreaInfo.screenSpaceBounds.size.y * speedY;

                update = true;
            }

            if (MouseButton.Right == mouseButton) {
                zoom.x = m_LastZoom.x + screenSpaceDeltaX * zoomSpeed.x;
                zoom.y = m_LastZoom.y + screenSpaceDeltaY * zoomSpeed.y;

                update = true;
            }

            if (update) {
                UpdateAll();
            }
        }

        public void OnAreaMouseScrollEvent(AreaInfo areaInfo, float mouseScrollDelta) {
            if (AreaHandler.kInvalidAreaId == areaInfo.areaId) {
                return;
            }

            Vector2 isometricZoomBase = new Vector2(
                m_RawImage.mainTexture.width * m_RawImageInvertedRectSize.x,
                m_RawImage.mainTexture.height * m_RawImageInvertedRectSize.y
            );

            Vector2 isometricZoomToZoomRatio = new Vector2(
                zoom.x / isometricZoomBase.x,
                zoom.y / isometricZoomBase.y
            );

            float isometricZoomToZoomRatioAverage = (isometricZoomToZoomRatio.x + isometricZoomToZoomRatio.y) * 0.5f;
            int isometriZoomMultiplier = Mathf.RoundToInt(isometricZoomToZoomRatioAverage + mouseScrollDelta);
            isometriZoomMultiplier = Mathf.Max(1, isometriZoomMultiplier);

            zoom = isometricZoomBase * isometriZoomMultiplier;

            UpdateAll();
        }

        public void Update() {
            UpdateUVRect();
        }
    }
}
