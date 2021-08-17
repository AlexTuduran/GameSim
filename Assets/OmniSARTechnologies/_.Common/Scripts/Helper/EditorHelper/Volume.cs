//
// Volume
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class Volume : MonoBehaviour {
        [Header("Boundaries")]
        public Vector3 minBoundaries = Vector3.one * -50.0f;
        public Vector3 maxBoundaries = Vector3.one * +50.0f;

        [Header("Gizmos Settings")]
        public bool ignoreTransform = false;
        public bool showGizmoName = true;
        public bool showGizmoVolume = true;
        public Color gizmoVolumeColor = Color.HSVToRGB(180.0f / 360.0f, 1, 1);
        public bool showHandles = true;
        public bool showCornerHandles = true;
        public bool cornerHandlesInvertColor = false;
        public bool showSideHandles = true;
        public bool sideHandlesInvertColor = true;
        [Range(0.1f, 10.0f)] public float handlesCapSize = 1.0f;
        [Range(0.125f, 2.0f)] public float sideHandlesCapSizeRatio = 0.5f;
        public float handlesSnapping = 0.0f;

        public Vector3 VolumeSize {
            get {
                return new Vector3(
                    Mathf.Abs(maxBoundaries.x - minBoundaries.x),
                    Mathf.Abs(maxBoundaries.y - minBoundaries.y),
                    Mathf.Abs(maxBoundaries.z - minBoundaries.z)
                );
            }
        }

        public Vector3 VolumeCenter {
            get {
                return (maxBoundaries + minBoundaries) * 0.5f;
            }
        }

        public float VolumeXYArea {
            get {
                var volumeSize = VolumeSize;
                return volumeSize.x * volumeSize.y;
            }
        }

        public float VolumeYZArea {
            get {
                var volumeSize = VolumeSize;
                return volumeSize.y * volumeSize.z;
            }
        }

        public float VolumeZXArea {
            get {
                var volumeSize = VolumeSize;
                return volumeSize.z * volumeSize.x;
            }
        }

        public float VolumeXYDiagonal {
            get {
                var volumeSize = VolumeSize;
                return Mathf.Sqrt(volumeSize.x * volumeSize.x + volumeSize.y * volumeSize.y);
            }
        }

        public float VolumeYZDiagonal {
            get {
                var volumeSize = VolumeSize;
                return Mathf.Sqrt(volumeSize.y * volumeSize.y + volumeSize.z * volumeSize.z);
            }
        }

        public float VolumeZXDiagonal {
            get {
                var volumeSize = VolumeSize;
                return Mathf.Sqrt(volumeSize.z * volumeSize.z + volumeSize.x * volumeSize.x);
            }
        }

        public float VolumeVolume {
            get {
                var volumeSize = VolumeSize;
                return volumeSize.x * volumeSize.y * volumeSize.z;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValidate() {
            handlesSnapping = Mathf.Max(0, 0f, handlesSnapping);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDrawGizmos() {
            DrawGizmos(false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDrawGizmosSelected() {
            DrawGizmos(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawGizmos(bool selected) {
            var col = gizmoVolumeColor;

            Vector3 gizmoVolumeCenter = VolumeCenter + (ignoreTransform ? Vector3.zero : transform.position);
            Vector3 gizmoVolumeSize = VolumeSize;

            col.a = showGizmoVolume ? selected ? 0.1f : 0.05f : 0.0f;
            Gizmos.color = col;
            Gizmos.DrawCube(gizmoVolumeCenter, gizmoVolumeSize);

            col.a = showGizmoVolume ? selected ? 0.5f : 0.2f : 0.0f;
            Gizmos.color = col;
            Gizmos.DrawWireCube(gizmoVolumeCenter, gizmoVolumeSize);
        }
    }
}