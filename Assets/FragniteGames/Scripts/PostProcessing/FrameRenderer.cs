using System.Runtime.CompilerServices;
using UnityEngine;
using OmniSARTechnologies.Helper;
using Random = UnityEngine.Random;

namespace FragniteGames {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    public class FrameRenderer : CameraImageEffect {
        [Header("Frame Provider")]
        public FrameProvider frameProvider = null;

        [Header("Jitter Settings")]
        public bool jitterColor = false;
        public bool jitterDepth = true;
        [Range(0, 1)] public float jitterAmount = 1.0f;

        [Header("Animation Settings")]
        public bool animate = false;

        public float frequency = 0.1f;
        public float magnitude = 0.1f;

        [Header("Frame Transform Settings")]
        public int zoomLevel = kDefaultZoomLevel;

        [Header("Other Settings")]
        public bool enabledInSceneView = false;

        private Vector4 m_Jitter = Vector4.zero;
        private float m_AnimatedZoom = 1.0f;

        private const int kDefaultZoomLevel = 10;
        private const float kZoomStep = 0.1f;
        private static readonly string kShaderName = "Hidden/Fragnite Games/Frame Renderer";

        private static class ShaderPropertyToID {
            public static readonly int kColorTexture = Shader.PropertyToID("_ColorTexture");
            public static readonly int kDepthTexture = Shader.PropertyToID("_DepthTexture");
            public static readonly int kColorJitter = Shader.PropertyToID("_ColorJitter");
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToFrame(int frameIndex) {
            frameProvider.GoToFrame(frameIndex);
        }

        [ContextMenu("Go To First Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToFirstFrame() {
            frameProvider.GoToFirstFrame();
        }

        [ContextMenu("Go To Last Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToLastFrame() {
            frameProvider.GoToLastFrame();
        }

        [ContextMenu("Go To Previous Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToPreviousFrame() {
            frameProvider.GoToPreviousFrame();
        }

        [ContextMenu("Go To Next Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToNextFrame() {
            frameProvider.GoToNextFrame();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleAnimation() {
            animate = !animate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleColorJitter() {
            jitterColor = !jitterColor;
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
        private void UpdateJitter() {
            m_Jitter.x = (Random.value - 0.5f) * jitterAmount;
            m_Jitter.y = (Random.value - 0.5f) * jitterAmount;
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

            if (!enabledInSceneView && (currentCamera.cameraType == CameraType.SceneView)) {
                Graphics.Blit(src, dst);

                return;
            }

            UpdateJitter();
            UpdateAnimation();

            zoomLevel = Mathf.Max(1, zoomLevel);

            material.SetTexture(ShaderPropertyToID.kColorTexture, frameProvider.CurrentColorTexture);
            material.SetTexture(ShaderPropertyToID.kDepthTexture, frameProvider.CurrentDepthTexture);
            material.SetVector(ShaderPropertyToID.kColorJitter, jitterColor ? m_Jitter : Vector4.zero);
            material.SetVector(ShaderPropertyToID.kDepthJitter, jitterDepth ? m_Jitter : Vector4.zero);
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