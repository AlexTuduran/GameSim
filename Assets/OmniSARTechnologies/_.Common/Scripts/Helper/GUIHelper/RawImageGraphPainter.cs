//
// Raw Image Graph Painter
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
    [System.Serializable]
    public class GraphChangedEvent : UnityEvent<RawImageGraphPainter> { }

    [RequireComponent(typeof(RawImage))]
    [ExecuteInEditMode]
    public class RawImageGraphPainter : MonoBehaviour {
        public GraphChangedEvent onGraphChanged = null;

        [HideInInspector]
        public int[] graph = null;

        private RawImage m_RawImage;
        private Vector2 m_LastDraggingPosition;

        public bool AcquireRawImage() {
            if (m_RawImage) {
                return true;
            }

            m_RawImage = GetComponent<RawImage>();

            if (!m_RawImage) {
                return false;
            }

            return true;
        }

        public bool CreateGraph() {
            if (!m_RawImage) {
                return false;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return false;
            }

            graph = new int[texture.width];

            return true;
        }

        public void OnEnable() {
            AcquireRawImage();
        }

        public void OnValidate() {
            UpdateAll();
        }

        public void ValidateProperties() {
            
        }

        public void UpdateAll() {
            ValidateProperties();
        }

        public void ResetGraph() {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            if (!CreateGraph()) {
                return;
            }

            int numPixels = texture.width * texture.height;
            Color32[] pixels = new Color32[numPixels];

            texture.SetPixels32(pixels);

            for (int j = 0; j < texture.height; j++) {
                for (int i = 0; i < texture.width; i++) {
                    if (j == i) {
                        texture.SetPixel(i, j, Color.white);
                        graph[i] = j;
                    }
                }
            }

#if !true
            for (int j = 0; j < texture.height; j++) {
                for (int i = 0; i < texture.width; i++) {
                    texture.SetPixel(i, j, Color.white);
                }
            }
#endif
            texture.Apply();

            if (null != onGraphChanged) {
                onGraphChanged.Invoke(this);
            }
        }

        public void DrawGraphValue(int x, int y, Color color) {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            if (!CreateGraph()) {
                return;
            }

            x = Mathf.Clamp(x, 0, texture.width - 1);
            y = Mathf.Clamp(y, 0, texture.height - 1);

            graph[x] = y;
            for (int j = 0; j < texture.height; j++) {
                texture.SetPixel(x, j, j == y ? color : Color.black);
            }

            texture.Apply();

            if (null != onGraphChanged) {
                onGraphChanged.Invoke(this);
            }
        }

        public void DrawGraphValue(float u, float v, Color color) {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            u = Mathf.Clamp01(u);
            v = Mathf.Clamp01(v);

            int x = Mathf.RoundToInt(u * texture.width);
            int y = Mathf.RoundToInt(v * texture.height);

            DrawGraphValue(x, y, color);
        }

        public void DrawGraphLine(int xStart, int yStart, int xEnd, int yEnd, Color color) {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

            if (xStart > xEnd) {
                int aux = xStart;
                xStart = xEnd;
                xEnd = aux;

                aux = yStart;
                yStart = yEnd;
                yEnd = aux;
            }

            xStart = Mathf.Clamp(xStart, 0, texture.width - 1);
            yStart = Mathf.Clamp(yStart, 0, texture.height - 1);
            xEnd = Mathf.Clamp(xEnd, 0, texture.width - 1);
            yEnd = Mathf.Clamp(yEnd, 0, texture.height - 1);

            int y = 0;
            for (int i = xStart; i <= xEnd; i++) {
                float t = Mathf.InverseLerp(xStart, xEnd, i);
                y = Mathf.RoundToInt(Mathf.Lerp(yStart, yEnd, t));
                y = Mathf.Clamp(y, 0, texture.height - 1);

                graph[i] = y;
                for (int j = 0; j <= texture.height; j++) {
                    texture.SetPixel(i, j, j == y ? color : Color.black);
                }
            }

            texture.Apply();

            if (null != onGraphChanged) {
                onGraphChanged.Invoke(this);
            }
        }

        public void DrawGraphLine(float uStart, float vStart, float uEnd, float vEnd, Color color) {
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

            DrawGraphLine(xStart, yStart, xEnd, yEnd, color);
        }

        public void DrawLine(int xStart, int yStart, int xEnd, int yEnd, Color color) {
            if (!AcquireRawImage()) {
                return;
            }

            Texture2D texture = (Texture2D)m_RawImage.mainTexture;
            if (!texture) {
                return;
            }

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

            texture.SetPixel(xStart, yStart, color);

            if (xDiff > yDiff) {
                f = yDiff - (xDiff >> 1);
                while (Mathf.Abs(xStart - xEnd) > 1) {
                    if (f >= 0) {
                        yStart += yStep;
                        f -= xDiff;
                    }
                    xStart += xStep;
                    f += yDiff;
                    texture.SetPixel(xStart, yStart, color);
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
                    texture.SetPixel(xStart, yStart, color);
                }
            }

            texture.Apply();
        }

        public void DrawLine(float uStart, float vStart, float uEnd, float vEnd, Color color) {
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

            DrawLine(xStart, yStart, xEnd, yEnd, color);
        }

        public void OnAreaMouseHoverEvent(AreaInfo areaInfo) {
            
        }

        public void OnAreaMouseDownEvent(AreaInfo areaInfo, MouseButton mouseButton) {
            if (AreaHandler.kInvalidAreaId == areaInfo.areaId) {
                return;
            }

            if (MouseButton.Left == mouseButton) {
                DrawGraphValue(
                    areaInfo.mousePositionAreaNormalizedSpace.x,
                    areaInfo.mousePositionAreaNormalizedSpace.y,
                    Color.white
                );

                m_LastDraggingPosition = areaInfo.mousePositionAreaNormalizedSpace;
                UpdateAll();
            }

            if (MouseButton.Right == mouseButton) {
                
            }

            if (MouseButton.Middle == mouseButton) {
                ResetGraph();
                UpdateAll();
            }
        }

        public void OnAreaMouseUpEvent(AreaInfo areaInfo, MouseButton mouseButton, AreaInfo mouseDownAreaInfo) {
            
        }

        public void OnAreaMouseDraggingEvent(AreaInfo areaInfo, MouseButton mouseButton, AreaInfo mouseDownAreaInfo) {
            if (AreaHandler.kInvalidAreaId == areaInfo.areaId) {
                return;
            }

            if (MouseButton.Left == mouseButton) {
                DrawGraphLine(
                    m_LastDraggingPosition.x,
                    m_LastDraggingPosition.y,
                    areaInfo.mousePositionAreaNormalizedSpace.x,
                    areaInfo.mousePositionAreaNormalizedSpace.y,
                    Color.white
                );

                m_LastDraggingPosition = areaInfo.mousePositionAreaNormalizedSpace;
                UpdateAll();
            }
        }

        public void OnAreaMouseScrollEvent(AreaInfo areaInfo, float mouseScrollDelta) {
            
        }
    }
}
