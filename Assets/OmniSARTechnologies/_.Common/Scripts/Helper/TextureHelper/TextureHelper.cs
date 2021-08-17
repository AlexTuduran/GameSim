//
// Texture Helper
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public static class TextureHelper {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Texture2D CreateTexture(
            int textureWidth,
            int textureHeigth,
            string textureName = "tex",
            TextureFormat textureFormat = TextureFormat.RGBAFloat,
            bool linear = true
        ) {
            Texture2D tex = new Texture2D(textureWidth, textureHeigth, textureFormat, false, linear);

            tex.anisoLevel = 0;
            tex.filterMode = linear ? FilterMode.Bilinear : FilterMode.Point;
            tex.wrapMode = TextureWrapMode.Clamp;
#if !true
            tex.hideFlags = HideFlags.HideAndDontSave;
#else
            tex.hideFlags = HideFlags.None;
#endif
            tex.name = string.Format("{0}-{1}x{2}", textureName, textureWidth, textureHeigth);

            int sizeBytes = tex.GetRawTextureData().Length;
            Debug.LogFormat(
                "Created texture \"{0}\" ({1}x{2}, {3}, {4}, {5}, {6}MB ({7}GB))",
                tex.name,
                tex.width,
                tex.height,
                tex.format,
                tex.filterMode,
                tex.hideFlags,
                (sizeBytes / 1048576.0f).ToString("F2"),
                (sizeBytes / 1073741824.0f).ToString("F3")
            );

            return tex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearTexture(
            out Texture2D texture,
            OnUpdatedTextureDelegate onUpdatedTexture,
            int textureWidth,
            int textureHeight,
            string textureName = "tex",
            TextureFormat textureFormat = TextureFormat.RGBAFloat,
            bool linear = true
        ) {
            Debug.LogFormat("Starting to clear texture {0} ({1}x{2})...", textureName, textureWidth, textureHeight);

            texture = CreateTexture(textureWidth, textureHeight, textureName, textureFormat, linear);

            if (
                textureWidth != texture.width ||
                textureHeight != texture.height
            ) {
                Debug.LogErrorFormat(
                    "The dimensions of texture {0} could not be set to {1}x{2}: {0}={3}x{4}: Operation aborted",
                    textureName,
                    textureWidth,
                    textureHeight,
                    texture.width,
                    texture.height
                );

                return false;
            }

            int numPixels = texture.width * texture.height;
            Color32[] pixels = new Color32[numPixels];

            texture.SetPixels32(pixels);
            texture.Apply();

            if (null != onUpdatedTexture) {
                onUpdatedTexture(texture);
            }

            Debug.LogFormat("Cleared texture {0} data ({1}x{2})", textureName, textureWidth, textureHeight);

            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BlitRenderTextureOnTexture2D(RenderTexture src, Texture2D dst) {
            if (!src) {
                Debug.LogErrorFormat("NULL {0} {1}: Operation aborted", src.GetType(), nameof(src));
                return false;
            }
            
            if (src.width * src.height < 1) {
                Debug.LogErrorFormat(
                    "Invalid {0} {1} dimensions: {2}x{3}: Operation aborted",
                    src.GetType(),
                    nameof(src),
                    src.width,
                    src.height
                );

                return false;
            }

            if (!dst) {
                Debug.LogErrorFormat("NULL {0} {1}: Operation aborted", dst.GetType(), nameof(dst));
                return false;
            }

            if (dst.width * dst.height < 1) {
                Debug.LogErrorFormat(
                    "Invalid {0} {1} dimensions: {2}x{3}: Operation aborted",
                    dst.GetType(),
                    nameof(dst),
                    dst.width,
                    dst.height
                );

                return false;
            }

            Graphics.SetRenderTarget(src);
            dst.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
            dst.Apply();

            return true;
        }
    }
}
