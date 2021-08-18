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
    public class FrameRenderer : ImageEffect {
        [Header("Frames")]
        public List<Texture2D> colorTextures = null;
        public List<Texture2D> depthTextures = null;

        [Header("Depth Settings")]
        public bool jitterDepth = true;
        [Range(0, 1)] public float depthJitterAmount = 1.0f;

        [Header("Animation Settings")]
        public bool animate = false;
        public float frequency = 0.1f;
        public float magnitude = 0.1f;

        [Header("Frame Transform Settings")]
        public int zoomLevel = kDefaultZoomLevel;

        private int m_CurrentFrameIndex = 0;
        private Vector4 m_DepthJitter = Vector4.zero;
        private float m_AnimatedZoom = 1.0f;

        private const int kDefaultZoomLevel = 10;
        private const float kZoomStep = 0.1f;
        private static readonly string kShaderName = "Hidden/Fragnite Games/Frame Renderer";

        private static class ShaderPropertyToID {
            public static readonly int kColorTexture = Shader.PropertyToID("_ColorTexture");
            public static readonly int kDepthTexture = Shader.PropertyToID("_DepthTexture");
            public static readonly int kDepthJitter = Shader.PropertyToID("_DepthJitter");
            public static readonly int kZoom = Shader.PropertyToID("_Zoom");
        }

        private static class ShaderPass {
            public static readonly int kRenderImage = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetShaderName() {
            return kShaderName;
        }

        [ContextMenu("Go To Previous Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToPreviousFrame() {
            m_CurrentFrameIndex = ((m_CurrentFrameIndex - 1) + colorTextures.Count + colorTextures.Count) % colorTextures.Count;
        }

        [ContextMenu("Go To Next Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToNextFrame() {
            m_CurrentFrameIndex = ((m_CurrentFrameIndex + 1) + colorTextures.Count + colorTextures.Count) % colorTextures.Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleAnimation() {
            animate = !animate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleDepthJitter() {
            jitterDepth = !jitterDepth;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ZoomOut() {
            zoomLevel--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ZoomIn() {
            zoomLevel++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetZoom() {
            zoomLevel = kDefaultZoomLevel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateDepthJitter() {
            if (jitterDepth) {
                m_DepthJitter.x = (Random.value - 0.5f) * depthJitterAmount;
                m_DepthJitter.y = (Random.value - 0.5f) * depthJitterAmount;
            } else {
                m_DepthJitter = Vector4.zero;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAnimation() {
            if (animate) {
                m_AnimatedZoom = Mathf.Sin(Time.realtimeSinceStartup * Mathf.PI * 2.0f * frequency);
                m_AnimatedZoom *= 0.5f;
                m_AnimatedZoom += 0.5f;
                m_AnimatedZoom *= magnitude;
                m_AnimatedZoom = 1.0f - m_AnimatedZoom;
            } else {
                m_AnimatedZoom = 1.0f;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (!material) {
                Graphics.Blit(src, dst);
                return;
            }

            UpdateDepthJitter();
            UpdateAnimation();

            zoomLevel = Mathf.Max(1, zoomLevel);

            material.SetTexture(ShaderPropertyToID.kColorTexture, colorTextures[m_CurrentFrameIndex]);
            material.SetTexture(ShaderPropertyToID.kDepthTexture, depthTextures[m_CurrentFrameIndex]);
            material.SetVector(ShaderPropertyToID.kDepthJitter, m_DepthJitter);
            material.SetFloat(ShaderPropertyToID.kZoom, m_AnimatedZoom / (zoomLevel * kZoomStep));

            Graphics.Blit(
                null,
                dst,
                material,
                ShaderPass.kRenderImage
            );
        }
    }
}