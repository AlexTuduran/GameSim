//
// Raw Image Painter
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace OmniSARTechnologies.Helper {
    public enum PaintOperation {
        Replace,
        Add,
        Subtract
    }

    [System.Serializable]
    public class ImageChangedEvent : UnityEvent<RawImagePainter> { }

    [RequireComponent(typeof(RawImage))]
    [ExecuteInEditMode]
    public class RawImagePainter : MonoBehaviour {
        public int areaId = -1;
        public float brushSize = 25f;
        public float brushIntensity = 0.1f;
        public bool smoothBrush = true;
        public bool energyConservativeBrush = true;

        [Range(0.01f, 16.0f)]
        public float brushCoarse = 1.0f;

        [Range(0, 1)]
        public float brushNoise = 0.0f;

        public ImageChangedEvent onImageChanged = null;
        public RawImage brushOverlayIndicator = null;

        private RawImage m_RawImage = null;
        private Vector2 m_LastDraggingPosition = Vector2.zero;
        private RectTransform m_RectTransform = null;

        private float m_TransformRectW = 0;
        private float m_TransformRectH = 0;
        private float m_BrushScaleX = 1;
        private float m_BrushScaleY = 1;

        private const float MaxBrushSize = 200.0f;

        public bool AcquireRawImage() {
            if (m_RawImage) {
                return true;
            }

            m_RawImage = GetComponent<RawImage>();

            if (!m_RawImage) {
                return false;
            }

            m_RectTransform = GetComponent<RectTransform>();
            m_TransformRectW = m_RectTransform.rect.width;
            m_TransformRectH = m_RectTransform.rect.height;

            return true;
        }

        public void OnEnable() {
            AcquireRawImage();
        }

        public void OnValidate() {
            UpdateAll();
        }

        public void ValidateProperties() {
            brushSize = Mathf.Clamp(brushSize, 1.0f, MaxBrushSize);
            brushIntensity = Mathf.Max(0, brushIntensity);

            updateBrushOverlayIndicatorSize();
        }

        public void updateBrushOverlayIndicatorSize() {
            if (!brushOverlayIndicator) {
                return;
            }

            int bs = Mathf.RoundToInt(brushSize);
            brushOverlayIndicator.rectTransform.sizeDelta = new Vector2(bs, bs);
        }

        public void UpdateAll() {
            ValidateProperties();
        }

        public void ResetImage() {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            int numPixels = texture.width * texture.height;
            Color32[] pixels = new Color32[numPixels];

            texture.SetPixels32(pixels);
            texture.Apply();

            if (null != onImageChanged) {
                onImageChanged.Invoke(this);
            }
        }

        private void DrawPixel(Texture2D texture, int x, int y, Color color, PaintOperation paintOperation) {
            switch (paintOperation) {
                case PaintOperation.Replace: {
                    texture.SetPixel(x, y, color);
                    return;
                }

                case PaintOperation.Subtract: {
                    texture.SetPixel(x, y, texture.GetPixel(x, y) - color);
                    return;
                }

                case PaintOperation.Add: {
                    texture.SetPixel(x, y, texture.GetPixel(x, y) + color);
                    return;
                }
            }
        }

        private void DrawBrush(Texture2D texture, int x, int y, Color color, PaintOperation paintOperation) {
            float halfBrushSize = brushSize * 0.5f;
            int xBrushSize = Mathf.RoundToInt(brushSize * m_BrushScaleX);
            int yBrushSize = Mathf.RoundToInt(brushSize * m_BrushScaleY);
            int halfXBrushSize = xBrushSize >> 1;
            int halfYBrushSize = yBrushSize >> 1;
            int xCenterOfs = x - halfXBrushSize;
            int yCenterOfs = y - halfYBrushSize;
            int xOfs;
            int yOfs;
            float xDiff;
            float yDiff;
            float dist;
            float atten = 1.0f;

            if (energyConservativeBrush) {
                color *= MaxBrushSize;
                color /= brushSize;
            }

            for (int j = 0; j < yBrushSize; j++) {
                yOfs = yCenterOfs + j;
                yDiff = (float)(yOfs - y) / m_BrushScaleY;
                yDiff *= yDiff;

                for (int i = 0; i < xBrushSize; i++) {
                    if (Random.value < brushNoise * brushNoise) {
                        continue;
                    }

                    xOfs = xCenterOfs + i;
                    xDiff = (float)(xOfs - x) / m_BrushScaleX;
                    xDiff *= xDiff;

                    dist = Mathf.Sqrt(xDiff + yDiff);
                    if (dist > halfBrushSize) {
                        continue;
                    }

                    if (smoothBrush) {
                        atten = 1.0f - Mathf.Clamp01(dist / halfBrushSize);
                        atten = Mathf.Pow(atten, brushCoarse);
                    }

                    DrawPixel(texture, xOfs, yOfs, color * atten, paintOperation);
                }
            }
        }

        public void DrawLine(int xStart, int yStart, int xEnd, int yEnd, Color color, PaintOperation paintOperation) {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            UpdateBrushScale();

            xStart = Mathf.Clamp(xStart, 0, texture.width - 1);
            yStart = Mathf.Clamp(yStart, 0, texture.height - 1);
            xEnd = Mathf.Clamp(xEnd, 0, texture.width - 1);
            yEnd = Mathf.Clamp(yEnd, 0, texture.height - 1);

            int xDiff = (int)(xEnd - xStart);
            int yDiff = (int)(yEnd - yStart);
            int xStep;
            int yStep;

            if (yDiff < 0) {
                yDiff = -yDiff;
                yStep = -1;
            }
            else {
                yStep = 1;
            }

            if (xDiff < 0) {
                xDiff = -xDiff;
                xStep = -1;
            }
            else {
                xStep = 1;
            }

            yDiff <<= 1;
            xDiff <<= 1;

            float f = 0.0f;

            DrawBrush(texture, xStart, yStart, color, paintOperation);

            if (xDiff > yDiff) {
                f = yDiff - (xDiff >> 1);
                while (Mathf.Abs(xStart - xEnd) > 1) {
                    if (f >= 0) {
                        yStart += yStep;
                        f -= xDiff;
                    }
                    xStart += xStep;
                    f += yDiff;
                    DrawBrush(texture, xStart, yStart, color, paintOperation);
                }
            } else {
                f = xDiff - (yDiff >> 1);
                while (Mathf.Abs(yStart - yEnd) > 1) {
                    if (f >= 0) {
                        xStart += xStep;
                        f -= yDiff;
                    }
                    yStart += yStep;
                    f += xDiff;
                    DrawBrush(texture, xStart, yStart, color, paintOperation);
                }
            }

            texture.Apply();
        }

        public void DrawLine(float uStart, float vStart, float uEnd, float vEnd, Color color, PaintOperation paintOperation) {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            uStart = Mathf.Clamp01(uStart);
            vStart = Mathf.Clamp01(vStart);
            uEnd = Mathf.Clamp01(uEnd);
            vEnd = Mathf.Clamp01(vEnd);

            int xStart = Mathf.RoundToInt(uStart * texture.width);
            int yStart = Mathf.RoundToInt(vStart * texture.height);
            int xEnd = Mathf.RoundToInt(uEnd * texture.width);
            int yEnd = Mathf.RoundToInt(vEnd * texture.height);

            DrawLine(xStart, yStart, xEnd, yEnd, color, paintOperation);
        }

        public bool DrawBrushLine(float uStart, float vStart, float uEnd, float vEnd, MouseButton mouseButton) {
            bool updateAll = false;
            bool lCtrlDown = Input.GetKey(KeyCode.LeftControl);

            if (MouseButton.Left == mouseButton) {
                if (lCtrlDown) {
                    DrawLine(
                        uStart,
                        vStart,
                        uEnd,
                        vEnd,
                        Color.white,
                        PaintOperation.Replace
                    );
                } else {
                    DrawLine(
                        uStart,
                        vStart,
                        uEnd,
                        vEnd,
                        Color.white * brushIntensity,
                        PaintOperation.Add
                    );
                }

                updateAll = true;
            }

            if (MouseButton.Right == mouseButton) {
                if (lCtrlDown) {
                    DrawLine(
                        uStart,
                        vStart,
                        uEnd,
                        vEnd,
                        Color.black,
                        PaintOperation.Replace
                    );
                } else {
                    DrawLine(
                        uStart,
                        vStart,
                        uEnd,
                        vEnd,
                        Color.white * brushIntensity,
                        PaintOperation.Subtract
                    );
                }

                updateAll = true;
            }

            if (updateAll) {
                UpdateAll();
            }

            return updateAll;
        }
            
        private void UpdateBrushScale() {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            m_BrushScaleX = texture.width / m_TransformRectW;
            m_BrushScaleY = texture.height / m_TransformRectH;
        }



        public void OnAreaMouseHoverEvent(AreaInfo areaInfo) {
            if (!brushOverlayIndicator) {
                return;
            }

            if (AreaHandler.kInvalidAreaId == areaInfo.areaId) {
                brushOverlayIndicator.enabled = false;
                return;
            }

            brushOverlayIndicator.enabled = true;
            brushOverlayIndicator.rectTransform.position = new Vector3(areaInfo.mousePositionWorldSpace.x, areaInfo.mousePositionWorldSpace.y, 0);
        }

        public void OnAreaMouseDownEvent(AreaInfo areaInfo, MouseButton mouseButton) {
            if (areaId != areaInfo.areaId) {
                return;
            }

            if (DrawBrushLine(
                areaInfo.mousePositionAreaNormalizedSpace.x,
                areaInfo.mousePositionAreaNormalizedSpace.y,
                areaInfo.mousePositionAreaNormalizedSpace.x,
                areaInfo.mousePositionAreaNormalizedSpace.y,
                mouseButton
            )) {
                m_LastDraggingPosition = areaInfo.mousePositionAreaNormalizedSpace;
            }

            if (MouseButton.Middle == mouseButton) {
                ResetImage();
                UpdateAll();
            }
        }

        public void OnAreaMouseUpEvent(AreaInfo areaInfo, MouseButton mouseButton, AreaInfo mouseDownAreaInfo) {
            
        }

        public void OnAreaMouseDraggingEvent(AreaInfo areaInfo, MouseButton mouseButton, AreaInfo mouseDownAreaInfo) {
            if (areaId != areaInfo.areaId) {
                return;
            }

            if (areaId != mouseDownAreaInfo.areaId) {
                return;
            }

            if (DrawBrushLine(
                m_LastDraggingPosition.x,
                m_LastDraggingPosition.y,
                areaInfo.mousePositionAreaNormalizedSpace.x,
                areaInfo.mousePositionAreaNormalizedSpace.y,
                mouseButton
            )) {
                m_LastDraggingPosition = areaInfo.mousePositionAreaNormalizedSpace;
            }
        }

        public void OnAreaMouseScrollEvent(AreaInfo areaInfo, float mouseScrollDelta) {
            brushSize += mouseScrollDelta * 0.1f * brushSize;
            brushSize = Mathf.Clamp(brushSize, 1.0f, MaxBrushSize);
            updateBrushOverlayIndicatorSize();
        }
    }
}
