//
// Render With Replacement Shader
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [ExecuteInEditMode]
    public class RenderWithReplacementShader : MonoBehaviour {
        public Camera targetCamera;
        public Shader replacementShader;
        public string replacementTag = "RenderType";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            if (null == replacementShader) {
                return;
            }

            if (null == targetCamera) {
                return;
            }

            targetCamera.SetReplacementShader(replacementShader, replacementTag);
            Debug.LogFormat("Set replacement shader '{0}' to camera '{1}'.", replacementShader.name, targetCamera.name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDisable() {
            if (null == targetCamera) {
                return;
            }

            targetCamera.ResetReplacementShader();
            Debug.LogFormat("Reset replacement shader on camera '{0}'.", targetCamera.name);
        }
    }
}