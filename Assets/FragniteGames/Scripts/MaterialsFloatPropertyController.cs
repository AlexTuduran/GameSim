using System.Runtime.CompilerServices;
using UnityEngine;

namespace FragniteGames {
    [ExecuteInEditMode]
    public class MaterialsFloatPropertyController : MonoBehaviour {
        public string materialsPropertyName = "_Intensity";
        public float materialPropertyValue = 0.0f;
        public Material[] targetMaterials = null;

        private int m_PropertyID = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            m_PropertyID = Shader.PropertyToID(materialsPropertyName);

            UpdateMaterials();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValidate() {
            UpdateMaterials();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateMaterials() {
            if (targetMaterials == null) {
                return;
            }

            int length = targetMaterials.Length;
            if (length < 1) {
                return;
            }

            for (int i = 0; i < length; i++) {
                var mat = targetMaterials[i];

                if (!mat) {
                    continue;
                }

                if (!mat.HasProperty(m_PropertyID)) {
                    continue;
                }

                mat.SetFloat(m_PropertyID, materialPropertyValue);
            }
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            UpdateMaterials();
        }
    }
}