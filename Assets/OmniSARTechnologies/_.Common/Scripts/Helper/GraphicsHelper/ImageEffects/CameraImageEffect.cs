//
// Camera Image Effect
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Rendering;
#endif

namespace OmniSARTechnologies.Helper {
    public class CameraImageEffect : ImageEffect {
        private Camera _currentCamera;

        public Camera currentCamera {
            get {
                Camera camera = GetComponent<Camera>();

                if (_currentCamera != camera) {
                    _currentCamera = camera;
                }

                return _currentCamera;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool UpdateCamera() {
            Camera camera = currentCamera;

            if (!camera) {
                return false;
            }

            if ((currentCamera.depthTextureMode & DepthTextureMode.Depth) == DepthTextureMode.Depth) {
                return true;
            }

            // camera needs to generate depth texture
            currentCamera.depthTextureMode |= DepthTextureMode.Depth;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool UpdateInternals() {
            if (!base.UpdateInternals()) {
                return false;
            }

            if (!UpdateCamera()) {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDeferredShadingCamera() {
            if (!currentCamera) {
                return false;
            }

            if (currentCamera.renderingPath != RenderingPath.DeferredShading) {
                return false;
            }

#if UNITY_EDITOR
            TierSettings currentTierSettings = EditorGraphicsSettings.GetTierSettings(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                Graphics.activeTier
            );

            if (
                currentCamera.renderingPath == RenderingPath.UsePlayerSettings &&
                currentTierSettings.renderingPath != RenderingPath.DeferredShading
            ) {
                return false;
            }
#endif

            return true;
        }
    }
}
