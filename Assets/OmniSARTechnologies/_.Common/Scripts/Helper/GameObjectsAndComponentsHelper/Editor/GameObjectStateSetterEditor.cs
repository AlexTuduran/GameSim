//
// Game Object State Setter Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEditor;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [CustomEditor(typeof(GameObjectStateSetter))]
    public class GameObjectStateSetterEditor : Editor {
        private GameObjectStateSetter _component {
            get {
                return (GameObjectStateSetter)target;
            }
        }

        private SerializedProperty _setStateOnAwakeSerializedProperty;

        public void OnEnable() {
            _setStateOnAwakeSerializedProperty = serializedObject.FindProperty(nameof(_component.setStateOnAwake));
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GUILayout.Space(2);
            if (_component.setStateOnAwake) {
                EditorGUILayout.HelpBox(
                    string.Format(
                        "'{0}' is ON." +
                        "\r\n" +
                        "\r\n" +
                        "This is a very expensive operation to be done in Awake. " +
                        "It's intended to be used on small scenes, but if your scene is large, rather consider disabling it and setting the states using the below button.",
                        _setStateOnAwakeSerializedProperty.displayName
                    ),
                    MessageType.Warning
                );
            }

            GUILayout.Space(2);
            if (GUILayout.Button("Auto Set State\n\rOn All Game Objects\n\rIn Scene", GUILayout.MinHeight(64))) {
                _component.AutoSetStateOnAllGameObjectsInScene();
            }
            
            EditorGUILayout.HelpBox("Press the above button ONLY if you know what you're doing.", MessageType.Info);
        }
    }
}