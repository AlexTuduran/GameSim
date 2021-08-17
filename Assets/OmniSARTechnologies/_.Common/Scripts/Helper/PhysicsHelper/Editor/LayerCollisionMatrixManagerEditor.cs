//
// Layer Collision Matrix Manager Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [CustomEditor(typeof(LayerCollisionMatrixManager))]
    public class LayerCollisionMatrixManagerEditor : Editor {
        private LayerCollisionMatrixManager _component {
            get {
                return (LayerCollisionMatrixManager)target;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if (GUILayout.Button("Reset Project Layer Collision Matrix (No Undo!)")) {
                LayerCollisionMatrixManager.ResetProjectPhysicsLayerCollisionMatrix();
            }

            if (!_component.workingLayerCollisionMatrix) {
                GUILayout.Label(
                    string.Format(
                        "Please select a {0} in the '{1}' field or create a new one.",
                        nameof(LayerCollisionMatrix),
                        nameof(_component.workingLayerCollisionMatrix)
                    ),
                    EditorStyles.helpBox
                );

                if (GUILayout.Button("Create And Assign New Layer Collision Matrix Asset")) {
                    _component.workingLayerCollisionMatrix = LayerCollisionMatrix.CreateNewAsset(false);
                }

                return;
            }

            if (GUILayout.Button(string.Format("Save Project Layer Collision Matrix To '{0}'", _component.workingLayerCollisionMatrix.name))) {
                _component.SavePhysicsLayerCollisionMatrix();
            }

            if (GUILayout.Button(string.Format("Check Layer Collision Matrix in '{0}'", _component.workingLayerCollisionMatrix.name))) {
                _component.LoadPhysicsLayerCollisionMatrix(true, false);
            }

            if (GUILayout.Button(string.Format("Load Project Layer Collision Matrix From '{0}' (No Undo!) [Replace]", _component.workingLayerCollisionMatrix.name))) {
                _component.LoadPhysicsLayerCollisionMatrix(false, false);
            }

            if (GUILayout.Button(string.Format("Load Project Layer Collision Matrix From '{0}' (No Undo!) [Additive]", _component.workingLayerCollisionMatrix.name))) {
                _component.LoadPhysicsLayerCollisionMatrix(false, true);
            }
        }
    }
}