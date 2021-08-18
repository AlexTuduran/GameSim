using System.Runtime.CompilerServices;
using UnityEngine;
using OmniSARTechnologies.Helper;

namespace FragniteGames {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    public class DepthVisualizer : ImageEffect {
        public bool readFromColorAlpha = false;
        public bool linear = false;
        public float minDistance = 1.0f;
        public float maxDistance = 10.0f;
        public float gamma = 1.0f;
        public float gain = 1.0f;

        private static readonly string kShaderName = "Hidden/Fragnite Games/Depth Visualizer";

        private static class ShaderPropertyToID {
            public static readonly int kColorTexture = Shader.PropertyToID("_ColorTexture");
            public static readonly int kReadFromColorAlpha = Shader.PropertyToID("_ReadFromColorAlpha");
            public static readonly int kLinear = Shader.PropertyToID("_Linear");
            public static readonly int kMinDistance = Shader.PropertyToID("_MinDistance");
            public static readonly int kMaxDistance = Shader.PropertyToID("_MaxDistance");
            public static readonly int kGamma = Shader.PropertyToID("_Gamma");
            public static readonly int kGain = Shader.PropertyToID("_Gain");
        }

        private static class ShaderPass {
            public static readonly int kRenderDepth = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetShaderName() {
            return kShaderName;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnValidate() {
            base.OnValidate();

            gain = Mathf.Max(0.0f, gain);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (!material) {
                Graphics.Blit(src, dst);
                return;
            }

            material.SetTexture(ShaderPropertyToID.kColorTexture, src);
            material.SetInt(ShaderPropertyToID.kReadFromColorAlpha, readFromColorAlpha ? 1 : 0);
            material.SetInt(ShaderPropertyToID.kLinear, linear ? 1 : 0);
            material.SetFloat(ShaderPropertyToID.kMinDistance, minDistance);
            material.SetFloat(ShaderPropertyToID.kMaxDistance, maxDistance);
            material.SetFloat(ShaderPropertyToID.kGamma, gamma);
            material.SetFloat(ShaderPropertyToID.kGain, gain);

            Graphics.Blit(
                null,
                dst,
                material,
                ShaderPass.kRenderDepth
            );
        }
    }
}