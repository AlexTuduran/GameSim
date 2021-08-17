//
// Layer Collision Matrix Manager
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class LayerCollisionMatrixManager : MonoBehaviour {
        public LayerCollisionMatrix workingLayerCollisionMatrix;

        private const int kInvalidLayer = -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SavePhysicsLayerCollisionMatrix() {
            if (!workingLayerCollisionMatrix) {
                Debug.LogWarningFormat("'{0}' must be assigned to a valid {1}.", nameof(workingLayerCollisionMatrix), nameof(LayerCollisionMatrix));

                return;
            }

            int numLayers = LayerCollisionMatrix.kNumLayers;
            workingLayerCollisionMatrix.layerNames = new string[numLayers];

            Debug.Log("");
            Debug.LogFormat("Project Layers:");
            for (int i = 0; i < numLayers; i++) {
                workingLayerCollisionMatrix.layerNames[i] = LayerMask.LayerToName(i);
                if (workingLayerCollisionMatrix.layerNames[i].Length > 0) {
                    Debug.LogFormat("Layer {0} = '{1}'", i, workingLayerCollisionMatrix.layerNames[i]);
                }
            }

            workingLayerCollisionMatrix.ignoreLayerCollision = new List<LayerCollisionIgnore>();
            Debug.Log("");
            Debug.LogFormat("Project Ignore Layer Collisions:");
            for (int i = 0; i < numLayers; i++) {
                var layerAName = workingLayerCollisionMatrix.layerNames[i];

                if (layerAName.Length < 1) {
                    continue;
                }

                for (int j = 0; j < numLayers; j++) {
                    var layerBName = workingLayerCollisionMatrix.layerNames[j];

                    if (layerBName.Length < 1) {
                        continue;
                    }

                    var ignoreLayerCollision = Physics.GetIgnoreLayerCollision(i, j);

                    if (!ignoreLayerCollision) {
                        continue;
                    }

                    workingLayerCollisionMatrix.ignoreLayerCollision.Add(new LayerCollisionIgnore(layerAName, layerBName));
                    Debug.LogFormat("Ignore layer collision between '{0}' and '{1}'", layerAName, layerBName);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadPhysicsLayerCollisionMatrix(bool checkOnly, bool additiveOperation) {
            if (!workingLayerCollisionMatrix) {
                Debug.LogWarningFormat("'{0}' must be assigned to a valid {1}", nameof(workingLayerCollisionMatrix), nameof(LayerCollisionMatrix));

                return;
            }

            if (null == workingLayerCollisionMatrix.layerNames) {
                Debug.LogWarningFormat("{0} '{1}' has invalid (NULL) layer names data.", nameof(LayerCollisionMatrix), nameof(workingLayerCollisionMatrix));

                return;
            }

            if (workingLayerCollisionMatrix.layerNames.Length < 1) {
                Debug.LogWarningFormat("{0} '{1}' has invalid (empty) layer names data.", nameof(LayerCollisionMatrix), nameof(workingLayerCollisionMatrix));

                return;
            }

            if (null == workingLayerCollisionMatrix.ignoreLayerCollision) {
                Debug.LogWarningFormat("{0} '{1}' has invalid (NULL) ignore layer collision data.", nameof(LayerCollisionMatrix), nameof(workingLayerCollisionMatrix));

                return;
            }

            if (workingLayerCollisionMatrix.ignoreLayerCollision.Count < 1) {
                Debug.LogWarningFormat("{0} '{1}' has invalid (empty) ignore layer collision data.", nameof(LayerCollisionMatrix), nameof(workingLayerCollisionMatrix));

                return;
            }

            int numLayers = LayerCollisionMatrix.kNumLayers;

            Debug.Log("");
            Debug.LogFormat("Asset Layers:");
            for (int i = 0; i < numLayers; i++) {
                if (workingLayerCollisionMatrix.layerNames[i].Length < 1) {
                    continue;
                }

                Debug.LogFormat("Asset Layer {0} = '{1}'", i, workingLayerCollisionMatrix.layerNames[i]);
            }

            Debug.Log("");
            Debug.LogFormat("Project Layers:");
            string[] projectLayerNames = new string[numLayers];
            for (int i = 0; i < numLayers; i++) {
                projectLayerNames[i] = LayerMask.LayerToName(i);

                if (projectLayerNames[i].Length < 1) {
                    continue;
                }

                Debug.LogFormat("Project Layer {0} = '{1}'", i, projectLayerNames[i]);
            }

            Debug.Log("");
            Debug.Log("Asset Ignore Layer Collisions:");
            int numIgnoreLayerCollisions = workingLayerCollisionMatrix.ignoreLayerCollision.Count;
            for (int i = 0; i < numIgnoreLayerCollisions; i++) {
                var ignoreLayerCollision = workingLayerCollisionMatrix.ignoreLayerCollision[i];

                Debug.LogFormat("Ignore layer collision between '{0}' and '{1}'", ignoreLayerCollision.layerA, ignoreLayerCollision.layerB);
            }

            if (checkOnly) {
                Debug.Log("");
                Debug.LogWarning("Ignore layer collision will not be set, but only checked.");

                return;
            }

            Debug.Log("");
            Debug.Log("Setting ignore layer collisions...");

            if (!additiveOperation) {
                // if not additive operation, it's replace operation
                // that means all the layer collisions are reset first
                for (int i = 0; i < numLayers; i++) {
                    for (int j = 0; j < numLayers; j++) {
                        Physics.IgnoreLayerCollision(i, j, false);
                    }
                }
            }

            for (int i = 0; i < numIgnoreLayerCollisions; i++) {
                var ignoreLayerCollision = workingLayerCollisionMatrix.ignoreLayerCollision[i];

                int layerA = LayerMask.NameToLayer(ignoreLayerCollision.layerA);

                if (kInvalidLayer == layerA) {
                    continue;
                }

                int layerB = LayerMask.NameToLayer(ignoreLayerCollision.layerB);

                if (kInvalidLayer == layerB) {
                    continue;
                }

                Physics.IgnoreLayerCollision(layerA, layerB);
            }
            Debug.Log("Ignore layer collisions set.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResetProjectPhysicsLayerCollisionMatrix() {
            int numLayers = LayerCollisionMatrix.kNumLayers;
            for (int i = 0; i < numLayers; i++) {
                for (int j = 0; j < numLayers; j++) {
                    Physics.IgnoreLayerCollision(i, j, false);
                }
            }
        }
    }
}