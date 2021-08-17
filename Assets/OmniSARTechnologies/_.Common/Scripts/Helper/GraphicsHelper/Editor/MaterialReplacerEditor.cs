//
// Material Replacer Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [CustomEditor(typeof(MaterialReplacer))]
    public class MaterialReplacerEditor : Editor {
        private MaterialReplacer _component {
            get {
                return (MaterialReplacer)target;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GUI.enabled = _component.replacerMaterial;

            if (GUILayout.Button("Replace All Materials In Children")) {
                _component.ReplaceMaterialInChildren();
            }

            EditorGUILayout.HelpBox(
                "Use the above button only if you know what you're doing!" +
                Environment.NewLine +
                //Environment.NewLine +
                "If pressed, it will replace every material in its children with the replacer material.",
                MessageType.None
            );

            GUI.enabled = true;
        }
    }
}