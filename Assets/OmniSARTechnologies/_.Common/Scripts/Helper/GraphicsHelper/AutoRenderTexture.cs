//
// Auto Render Texture
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class AutoRenderTexture {
        private RenderTexture _renderTexture = null;
        public RenderTexture renderTexture {
            get {
                return _renderTexture;
            }
        }

        private bool _rebuildScheduled = true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReleaseAutoRenderTexture(ref AutoRenderTexture autoRenderTexture) {
            if (null == autoRenderTexture) {
                return false;
            }

            autoRenderTexture.ReleaseAndClear();
            autoRenderTexture = null;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CreateAutoRenderTexture(ref AutoRenderTexture autoRenderTexture) {
            if (null != autoRenderTexture) {
                return true;
            }

            autoRenderTexture = new AutoRenderTexture();

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NeedsRebuilding(int width, int height) {
            return
                _rebuildScheduled ||
                _renderTexture == null ||
                _renderTexture.width != width ||
                _renderTexture.height != height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NeedsRebuilding(RenderTexture reference) {
            if (!reference) {
                return false;
            }

            return NeedsRebuilding(
                reference.width,
                reference.height
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Build(int width, int height, RenderTextureFormat format) {
            _renderTexture = RenderTexture.GetTemporary(width, height, 0, format);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Build(RenderTexture reference, bool copyReference = false, RenderTextureFormat renderTextureFormat = RenderTextureFormat.DefaultHDR) {
            if (!reference) {
                return;
            }

            Build(reference.width, reference.height, reference.format);

            if (copyReference) {
                Graphics.Blit(reference, _renderTexture);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rebuild(int width, int height, RenderTextureFormat renderTextureFormat = RenderTextureFormat.DefaultHDR) {
            Release();
            Build(width, height, renderTextureFormat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rebuild(RenderTexture reference, bool copyReference = false, RenderTextureFormat renderTextureFormat = RenderTextureFormat.DefaultHDR) {
            if (!reference) {
                return;
            }

            Release();
            Build(reference, copyReference, renderTextureFormat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool RebuildIfNeeded(int width, int height, RenderTextureFormat renderTextureFormat = RenderTextureFormat.DefaultHDR) {
            if (!NeedsRebuilding(width, height)) {
                return false;
            }

            Rebuild(width, height, renderTextureFormat);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool RebuildIfNeeded(RenderTexture reference, bool copyReference = false, RenderTextureFormat renderTextureFormat = RenderTextureFormat.DefaultHDR) {
            if (!NeedsRebuilding(reference)) {
                return false;
            }

            Rebuild(reference, copyReference, renderTextureFormat);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release() {
            if (_renderTexture) {
                RenderTexture.ReleaseTemporary(_renderTexture);
                _renderTexture = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseAndClear() {
            Release();
            ScheduleRebuild();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScheduleRebuild() {
            _rebuildScheduled = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelRebuild() {
            _rebuildScheduled = false;
        }
    }
}
