//
// Texture Blurrer
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class TextureBlurrer: Worker {
        public IEnumerator HBlurTexture(
            Texture2D texture,
            float amount,
            OnResetAllProgressDelegate onResetAllProgress,
            OnSetProgressDelegate onSetProgress
        ) {
            if (m_IsBusy) {
                yield break;
            }

            m_IsBusy = true;
            if (null != onResetAllProgress) {
                onResetAllProgress();
            }
            yield return null;

            if (!texture) {
                Debug.LogErrorFormat("Texture is NULL: Operation aborted");
                m_IsBusy = false;
                yield break;
            }

            if (texture.width < 1) {
                Debug.LogErrorFormat("Texture width must be > 1: Operation aborted");
                m_IsBusy = false;
                yield break;
            }

            if (texture.height < 1) {
                Debug.LogErrorFormat("Texture height must be > 1: Operation aborted");
                m_IsBusy = false;
                yield break;
            }

            int asyncReleasePeriod = 32 << 2;
            float dampedColor;
            Color color = Color.black;
            for (int j = 0; j < texture.height; j++) {
                dampedColor = 0;
                for (int i = 0; i < texture.width; i++) {
                    dampedColor = Mathf.Lerp(texture.GetPixel(i, j).r, dampedColor, amount);
                    color.r = dampedColor;
                    color.g = dampedColor;
                    color.b = dampedColor;
                    texture.SetPixel(i, j, color);
                }

                dampedColor = 0;
                for (int i = texture.width - 1; i > -1; i--) {
                    dampedColor = Mathf.Lerp(texture.GetPixel(i, j).r, dampedColor, amount);
                    color.r = dampedColor;
                    color.g = dampedColor;
                    color.b = dampedColor;
                    texture.SetPixel(i, j, color);
                }

                // breathe once in a while
                if (j % asyncReleasePeriod == 0) {
                    texture.Apply();
                    yield return null;

                    // update progress
                    if (null != onSetProgress) {
                        float progress = (float)j / (float)(texture.height - 1);
                        onSetProgress(progress);
                        yield return null;
                    }
                }
            }
            yield return null;

            texture.Apply();
            yield return null;

            m_IsBusy = false;
            if (null != onResetAllProgress) {
                onResetAllProgress();
            }
            yield return null;
        }

        public IEnumerator VBlurTexture(
            Texture2D texture,
            float amount,
            OnResetAllProgressDelegate onResetAllProgress,
            OnSetProgressDelegate onSetProgress
        ) {
            if (m_IsBusy) {
                yield break;
            }

            m_IsBusy = true;
            if (null != onResetAllProgress) {
                onResetAllProgress();
            }
            yield return null;

            if (!texture) {
                Debug.LogErrorFormat("Texture is NULL: Operation aborted");
                m_IsBusy = false;
                yield break;
            }

            if (texture.width < 1) {
                Debug.LogErrorFormat("Texture width must be > 1: Operation aborted");
                m_IsBusy = false;
                yield break;
            }

            if (texture.height < 1) {
                Debug.LogErrorFormat("Texture height must be > 1: Operation aborted");
                m_IsBusy = false;
                yield break;
            }

            int asyncReleasePeriod = 32 << 2;
            float dampedColor;
            Color color = Color.black;
            for (int j = 0; j < texture.width; j++) {
                dampedColor = 0;
                for (int i = 0; i < texture.height; i++) {
                    dampedColor = Mathf.Lerp(texture.GetPixel(j, i).r, dampedColor, amount);
                    color.r = dampedColor;
                    color.g = dampedColor;
                    color.b = dampedColor;
                    texture.SetPixel(j, i, color);
                }

                dampedColor = 0;
                for (int i = texture.height - 1; i > -1; i--) {
                    dampedColor = Mathf.Lerp(texture.GetPixel(j, i).r, dampedColor, amount);
                    color.r = dampedColor;
                    color.g = dampedColor;
                    color.b = dampedColor;
                    texture.SetPixel(j, i, color);
                }

                // breathe once in a while
                if (j % asyncReleasePeriod == 0) {
                    texture.Apply();
                    yield return null;

                    // update progress
                    if (null != onSetProgress) {
                        float progress = (float)j / (float)(texture.width - 1);
                        onSetProgress(progress);
                        yield return null;
                    }
                }
            }
            yield return null;

            texture.Apply();
            yield return null;

            m_IsBusy = false;
            if (null != onResetAllProgress) {
                onResetAllProgress();
            }
            yield return null;
        }
    }
}
