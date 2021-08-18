using System.Runtime.CompilerServices;
using UnityEngine;

namespace FragniteGames {
    [ExecuteInEditMode]
    public class CameraAnimator : MonoBehaviour {
        public Camera cameraTarget;
        public bool animate = false;
        public float frequency = 0.1f;
        public float magnitude = 0.1f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleAnimation() {
            animate = !animate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAnimation() {
            var trf = cameraTarget.transform;

            if (!animate) {
                trf.localPosition = Vector3.zero;
                return;
            }

            float z = Mathf.Sin(Time.realtimeSinceStartup * Mathf.PI * 2.0f * frequency);
            z *= 0.5f;
            z += 0.5f;
            z *= magnitude;

            trf.localPosition = new Vector3(
                trf.localPosition.x,
                trf.localPosition.y,
                z
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FixedUpdate() {
            UpdateAnimation();
        }
    }
}
