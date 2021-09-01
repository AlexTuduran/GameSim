//
// Frame Capturer Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;
using UnityEditor;

namespace OmniSARTechnologies.Utils {
    [CustomEditor(typeof(FrameCapturer))]
    public class FrameCapturerEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            FrameCapturer m_Component = target as FrameCapturer;
            if (!m_Component) {
                return;
            }

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("Capture Frame Now")) {
                    m_Component.CaptureFrame();
                }
            } GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("Capture HDR RT Now")) {
                    m_Component.CaptureHDRRenderTargetFrame();
                }
            } GUILayout.EndHorizontal();
        }
    }
}