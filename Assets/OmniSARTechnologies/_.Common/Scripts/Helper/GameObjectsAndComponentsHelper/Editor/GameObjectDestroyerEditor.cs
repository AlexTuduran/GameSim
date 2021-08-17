//
// Game Object Destroyer Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GameObjectDestroyer))]
    public class GameObjectDestroyerEditor : Editor {
        private GameObjectDestroyer _component {
            get {
                return (GameObjectDestroyer)target;
            }
        }

        private SerializedProperty _targetGameObject;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            _targetGameObject = serializedObject.FindProperty(nameof(_component.targetGameObject));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GUILayout.Space(2);
            if (_component.destroyInBuildOnly) {
                EditorGUILayout.HelpBox("This Game Object will NOT be destroyed in editor, but ONLY in builds.", MessageType.Warning);
            }

            GUILayout.Space(2);
            if (GUILayout.Button("Set Self As Target Game Object")) {
                _targetGameObject.serializedObject.Update();
                _targetGameObject.objectReferenceValue = _component.gameObject;
                _targetGameObject.serializedObject.ApplyModifiedProperties();
                _targetGameObject.serializedObject.Update();
            }

            GUILayout.Space(2);
            if (GUILayout.Button("Destroy Target Game Object Now")) {
                _component.DestroyTargetGameObject();
            }
            
            EditorGUILayout.HelpBox("Press the above button ONLY if you know what you're doing.", MessageType.Info);
        }
    }
}