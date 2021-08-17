//
// Free Camera Enhanced
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OmniSARTechnologies.Helper {
    public class FreeCameraEnh : MonoBehaviour {
        public bool captureInput;

        public KeyCode goUp = KeyCode.E;
        public KeyCode goDown = KeyCode.Q;

        public KeyCode quitAppKey = KeyCode.Escape;
        public KeyCode toggleCaptureInputKey = KeyCode.F1;

        public float lookSpeed = 5f;
        public float moveSpeed = 5f;
        public float sprintSpeed = 50f;

        private bool m_IsActive = false;
        private float m_CameraYaw;
        private float m_CameraPitch;

        public void QuitApp() {
            if (!m_IsActive) {
                return;
            }

            if (Application.isEditor) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            } else {
                Application.Quit();
            }
        }

        public static void ShowCursor(bool show) {
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = show;
        }

        public void Start() {
            m_IsActive = true;

            m_CameraYaw = transform.rotation.eulerAngles.y;
            m_CameraPitch = transform.rotation.eulerAngles.x;

            if (captureInput) {
                ShowCursor(false);
            }
        }

        public void OnApplicationFocus(bool focus) {
            if (!focus) {
                ShowCursor(true);
            } else {
                if (captureInput) {
                    ShowCursor(false);
                }
            }
        }

        private void UpdateCamera() {
            if (!m_IsActive) {
                return;
            }

            if (!captureInput) {
                return;
            }

            var rotX = Input.GetAxis("Mouse X");
            var rotY = Input.GetAxis("Mouse Y");

            m_CameraYaw = m_CameraYaw + lookSpeed * rotX;
            m_CameraPitch = m_CameraPitch - lookSpeed * rotY;

            transform.rotation =
                Quaternion.AngleAxis(m_CameraYaw % 360f, Vector3.up) *
                Quaternion.AngleAxis(m_CameraPitch % 360f, Vector3.right);

            var speed = Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed);
            var forward = speed * Input.GetAxis("Vertical");
            var right = speed * Input.GetAxis("Horizontal");
            var up = speed * ((Input.GetKey(goUp) ? 1f : 0f) - (Input.GetKey(goDown) ? 1f : 0f));
            transform.position += transform.forward * forward + transform.right * right + Vector3.up * up;
        }

        private void Update() {
            if (!m_IsActive) {
                return;
            }

            if (Input.GetKeyDown(quitAppKey)) {
                QuitApp();
            }

            if (captureInput) {
                ShowCursor(false);
            }

            if (Input.GetKeyDown(toggleCaptureInputKey)) {
                captureInput = !captureInput;
                ShowCursor(!captureInput);
            }

            UpdateCamera();
        }
    }
}