using System.Runtime.CompilerServices;
using UnityEngine;

namespace FragniteGames {
    [ExecuteInEditMode]
    public class ScreenStretchController : MonoBehaviour {
        public Camera targetCamera = null;

        private int m_LastPixelWidth = -1;
        private int m_LastPixelHeight = -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            StretchFullScreen();
        }

#if UNITY_EDITOR
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValidate() {
            StretchFullScreen();
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CameraPixelSizeChanged() {
            return
                (targetCamera.pixelWidth != m_LastPixelWidth) ||
                (targetCamera.pixelHeight != m_LastPixelHeight);
        }

        [ContextMenu("Stretch Full-Screen Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StretchFullScreen() {
            targetCamera.depthTextureMode |= DepthTextureMode.Depth;
            Vector3 cornerTR = targetCamera.ViewportToWorldPoint(Vector3.one);

#if !true
            Debug.LogFormat("{0:F3}", cornerTR);
#endif

            var trf = transform;
            trf.localScale = new Vector3(
                cornerTR.x * 2.0f,
                cornerTR.y * 2.0f,
                trf.localScale.z
            );

#if !true
            Debug.LogFormat("Screen stretched to full-screen.");
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateScreenSize() {
            if (!CameraPixelSizeChanged()) {
                return;
            }

            m_LastPixelWidth = targetCamera.pixelWidth;
            m_LastPixelHeight = targetCamera.pixelHeight;

            StretchFullScreen();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            UpdateScreenSize();
        }
    }
}