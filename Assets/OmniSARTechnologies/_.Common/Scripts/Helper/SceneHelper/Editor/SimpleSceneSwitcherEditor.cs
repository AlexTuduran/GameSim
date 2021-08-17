//
// Simple Scene Switcher Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;
using UnityEditor;

namespace OmniSARTechnologies.Helper {
    [CustomEditor(typeof(SimpleSceneSwitcher))]
    public class SimpleSceneSwitcherEditor : Editor {
        private SimpleSceneSwitcher m_Component {
            get {
                return (SimpleSceneSwitcher)target;
            }
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("<")) {
                    m_Component.ChangeActiveScene(-1);
                }

                if (GUILayout.Button(">")) {
                    m_Component.ChangeActiveScene(+1);
                }
            } GUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Current Scene: " + m_Component.GetCurrentSceneName(), MessageType.None);
        }
    }
}