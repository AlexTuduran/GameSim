//
// Toggle Button Handler
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace OmniSARTechnologies.Helper {
    [System.Serializable]
    public class ButtonStateChangeEvent : UnityEvent<bool> { } // isOn

    [ExecuteInEditMode]
    public class ToggleButtonHandler : MonoBehaviour {
        [Header("References")]
        public Text buttonTextRef;

        [Header("State")]
        public bool isOn = true;
        public bool autoToggle = true;
        public bool autoToggleInEditorMode = false;
        public AnimationCurve autoToggleGraph = AnimationCurve.Linear(0, 1, 1, 1);
        public float autoToggleTimeMultiplier = 1.0f;

        [Header("Settings")]
        public string buttonOnText = "FEATURE ON";
        public string buttonOffText = "FEATURE OFF";
        public Color buttonOnColor = Color.HSVToRGB(80.0f / 360.0f, 1, 1);
        public Color buttonOffColor = Color.HSVToRGB(0, 0, 0.75f);

        [Header("Events")]
        public ButtonStateChangeEvent OnButtonStateChange;

        private bool m_LastIsOn = true;

        public void OnValidate() {
            if (m_LastIsOn != isOn) {
                m_LastIsOn = isOn;
                SetButton(isOn);
            }
        }

        public void OnButtonClick() {
            if (!buttonTextRef) {
                Debug.LogError("NULL button text reference");
                return;
            }

            Toggle();
        }

        public void Toggle() {
            isOn = !isOn;
            SetButton(isOn);
        }

        public void SetButton(bool on) {
            isOn = on;

            if (!buttonTextRef) {
                Debug.LogError("NULL button text reference");
                return;
            }

            if (null != OnButtonStateChange) {
                OnButtonStateChange.Invoke(on);
            }

            if (on) {
                buttonTextRef.text = buttonOnText;
                buttonTextRef.color = buttonOnColor;
            } else {
                buttonTextRef.text = buttonOffText;
                buttonTextRef.color = buttonOffColor;
            }
        }

        public void SetButtonOn() {
            SetButton(true);
        }

        public void SetButtonOff() {
            SetButton(false);
        }

        public void AutoToggle() {
            if (!autoToggle) {
                return;
            }

#if UNITY_EDITOR
            if (!autoToggleInEditorMode) {
                return;
            }
#endif // UNITY_EDITOR
            
            bool isToggleGraphOn = autoToggleGraph.Evaluate(Time.realtimeSinceStartup * autoToggleTimeMultiplier) > 0.5f;
            if (isOn != isToggleGraphOn) {
                SetButton(isToggleGraphOn);
            }
        }

        public void Update() {
            AutoToggle();
        }
    }
}
