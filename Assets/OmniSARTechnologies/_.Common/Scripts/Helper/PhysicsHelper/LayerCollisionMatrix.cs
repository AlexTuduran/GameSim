//
// Layer Collision Matrix
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [System.Serializable]
    public struct LayerCollisionIgnore {
        public string layerA;
        public string layerB;

        public LayerCollisionIgnore(string layerA, string layerB) {
            this.layerA = layerA;
            this.layerB = layerB;
        }
    }

    [CreateAssetMenu(menuName = "OmniSAR Technologies/Layer Collision Matrix", order = 10, fileName = "New LayerCollisionMatrix")]
    public class LayerCollisionMatrix : ScriptableObject {
        public const int kNumLayers = 32;

        public string[] layerNames;
        public List<LayerCollisionIgnore> ignoreLayerCollision;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerCollisionMatrix CreateNewAsset(bool select) {
#if UNITY_EDITOR
            LayerCollisionMatrix asset = CreateInstance<LayerCollisionMatrix>();
            AssetDatabase.CreateAsset(asset, "Assets/New LayerCollisionMatrix.asset");
            AssetDatabase.SaveAssets();

            if (select) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }

            return asset;
#else
            return null;
#endif
        }
    }
}