//
// Simple Game Object Switcher Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;
using UnityEditor;

namespace OmniSARTechnologies.Helper {
    [CustomEditor(typeof(SimpleGameObjectSwitcher))]
    public class SimpleGameObjectSwitcherEditor : Editor {
        private SimpleGameObjectSwitcher m_Component {
            get {
                return (SimpleGameObjectSwitcher)target;
            }
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("<")) {
                    m_Component.ChangeActiveGameObject(-1);
                }

                if (GUILayout.Button(">")) {
                    m_Component.ChangeActiveGameObject(+1);
                }
            } GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("Update Game Objects List")) {
                    int goIndex = m_Component.gameObjectIndex;
                    m_Component.AcquireGameObjects();
                    m_Component.gameObjectIndex = goIndex;
                }
            } GUILayout.EndHorizontal();

            EditorGUILayout.HelpBox(
                string.Format(
                    "Current Game Object: {0} ({1} / {2})",
                    m_Component.GetCurrentGameObjectName(),
                    m_Component.gameObjectIndex + 1,
                    m_Component.gameObjectMaxIndex + 1
                ),
                MessageType.None
            );
        }
    }
}