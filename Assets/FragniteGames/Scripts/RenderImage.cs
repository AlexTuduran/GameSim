using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using OmniSARTechnologies.Helper;

namespace FragniteGames {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    public class RenderImage : ImageEffect {
        public List<Texture2D> textures = null;
        public bool animate = false;
        public KeyCode animationToggleKeyCode = KeyCode.A;
        public float zoom = 1.0f;
        public float frequency = 0.1f;
        public float magnitude = 0.1f;
        
        private int m_CurrentTextureIndex = 0;

        private static readonly string kShaderName = "Hidden/Fragnite Games/Render Image";

        private static class ShaderPropertyToID {
            public static readonly int kInputTexture = Shader.PropertyToID("_InputTexture");
            public static readonly int kZoom = Shader.PropertyToID("_Zoom");
        }

        private static class ShaderPass {
            public static readonly int kRenderImage = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetShaderName() {
            return kShaderName;
        }

        [ContextMenu("Set Previous Texture Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPreviousTexture() {
            m_CurrentTextureIndex = ((m_CurrentTextureIndex - 1) + textures.Count + textures.Count) % textures.Count;
        }

        [ContextMenu("Set Next Texture Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNextTexture() {
            m_CurrentTextureIndex = ((m_CurrentTextureIndex + 1) + textures.Count + textures.Count) % textures.Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAnimation() {
            if (animate) {
                zoom = Mathf.Sin(Time.realtimeSinceStartup * Mathf.PI * 2.0f * frequency);
                zoom *= 0.5f;
                zoom += 0.5f;
                zoom *= magnitude;
                zoom = 1.0f - zoom;
            } else {
                zoom = 1.0f;
            }
        }

        public void Update() {
            if (Input.GetKeyDown(animationToggleKeyCode)) {
                animate = !animate;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (!material) {
                Graphics.Blit(src, dst);
                return;
            }
            
            UpdateAnimation();

            material.SetTexture(ShaderPropertyToID.kInputTexture, textures[m_CurrentTextureIndex]);
            material.SetFloat(ShaderPropertyToID.kZoom, zoom);
            
            Graphics.Blit(
                null,
                dst,
                material,
                ShaderPass.kRenderImage
            );
        }
    }
}