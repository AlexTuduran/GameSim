//
// Game Object Info Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEditor;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [CustomEditor(typeof(GameObjectInfo))]
    public class GameObjectInfoEditor : Editor {
        private GameObjectInfo _component {
            get {
                return (GameObjectInfo)target;
            }
        }

        private string _infoStr = "";

        private void UpdateInfo() {
            _infoStr = _component.GetInfo();
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("Update Info")) {
                    UpdateInfo();
                }
            } GUILayout.EndHorizontal();

            if (string.IsNullOrEmpty(_infoStr)) {
                UpdateInfo();
            }

            EditorGUILayout.HelpBox(_infoStr, MessageType.None);

            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("Select Object")) {
                    _component.gameObject.Select();
                }
            } GUILayout.EndHorizontal();
        } 
    }
}