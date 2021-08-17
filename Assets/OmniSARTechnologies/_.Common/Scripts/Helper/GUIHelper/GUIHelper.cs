//
// GUI Helper
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public static class GuiHelper {
        private static Texture2D _coloredLineTexture;
        private static Color _coloredLineColor;

        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color) {
            DrawLine(lineStart, lineEnd, color, 1);
        }

        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color, int thickness) {
            if (_coloredLineTexture == null || _coloredLineColor != color) {
                _coloredLineColor = color;
                _coloredLineTexture = new Texture2D(1, 1);
                _coloredLineTexture.SetPixel(0, 0, _coloredLineColor);
                _coloredLineTexture.wrapMode = TextureWrapMode.Repeat;
                _coloredLineTexture.Apply();
            }

            DrawLineStretched(lineStart, lineEnd, _coloredLineTexture, thickness);
        }

        public static void DrawLineStretched(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness) {
            Vector2 lineVector = lineEnd - lineStart;
            float angle = Mathf.Rad2Deg * Mathf.Atan(lineVector.y / lineVector.x);

            if (lineVector.x < 0) {
                angle += 180;
            }

            if (thickness < 1) {
                thickness = 1;
            }

            // The center of the line will always be at the center
            // regardless of the thickness.
            int thicknessOffset = (int)Mathf.Ceil(thickness / 2);

            GUIUtility.RotateAroundPivot(
                angle,
                lineStart
            );

            GUI.DrawTexture(
                new Rect(
                    lineStart.x,
                    lineStart.y - thicknessOffset,
                    lineVector.magnitude,
                    thickness
                ),
                texture
            );

            GUIUtility.RotateAroundPivot(-angle, lineStart);
        }

        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture) {
            DrawLine(lineStart, lineEnd, texture, 1);
        }

        public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, float thickness) {
            Vector2 lineVector = lineEnd - lineStart;
            float angle = Mathf.Rad2Deg * Mathf.Atan(lineVector.y / lineVector.x);

            if (lineVector.x < 0) {
                angle += 180;
            }

            if (thickness < 0.1f) {
                thickness = 0.1f;
            }

            // The center of the line will always be at the center
            // regardless of the thickness.
            //int thicknessOffset = (int)Mathf.Ceil(thickness / 2);
            float thicknessOffset = thickness / 2.0f;

            Rect drawingRect = new Rect(
                lineStart.x,
                lineStart.y - thicknessOffset,
                Vector2.Distance(lineStart, lineEnd),
                (float)thickness
            );

            GUIUtility.RotateAroundPivot(
                angle,
                lineStart
            );

            GUI.BeginGroup(drawingRect);
            {
                int drawingRectWidth = Mathf.RoundToInt(drawingRect.width);
                int drawingRectHeight = Mathf.RoundToInt(drawingRect.height);

                for (int y = 0; y < drawingRectHeight; y += texture.height) {
                    for (int x = 0; x < drawingRectWidth; x += texture.width) {
                        GUI.DrawTexture(
                            new Rect(
                                x,
                                y,
                                texture.width,
                                texture.height
                            ),
                            texture
                        );
                    }
                }
            }

            GUI.EndGroup();
            GUIUtility.RotateAroundPivot(-angle, lineStart);
        }
    }
}