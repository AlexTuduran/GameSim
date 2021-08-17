//
// Material Replacer
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com

using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class MaterialReplacer : MonoBehaviour {
        public Material replacerMaterial;

        [ContextMenu("Replace All Materials In Children Now")]
        public void ReplaceMaterialInChildren() {
            var meshRenderers = transform.GetComponentsInChildren<MeshRenderer>(true);

            Debug.LogFormat(
                "Replacing materials with {0} in {1} {2}{3}...",
                replacerMaterial.name,
                meshRenderers.Length,
                nameof(MeshRenderer),
                meshRenderers.Length == 1 ? "" : "s"
            );

            int numReplacedMaterials = 0;
            for (int i = 0; i < meshRenderers.Length; i++) {
                var mr = meshRenderers[i];
                var sm = mr.sharedMaterials;
                for (int j = 0; j < sm.Length; j++) {
                    sm[j] = replacerMaterial;
                }
                mr.sharedMaterials = sm;
                numReplacedMaterials += sm.Length;
            }
            
            Debug.LogFormat(
                "Replaced {0} materials with {1} in {2} {3}{4}",
                numReplacedMaterials,
                replacerMaterial.name,
                meshRenderers.Length,
                nameof(MeshRenderer),
                meshRenderers.Length == 1 ? "" : "s"
            );
        }
    }
}