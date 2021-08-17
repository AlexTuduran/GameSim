//
// Line Helper
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class LineHelper : MonoBehaviour {
        public static LineRenderer CreateLine(
            float xFrom,
            float yFrom,
            float xTo,
            float yTo,
            LineRenderer linePrefab = null,
            Color color = default(Color),
            float thickness = default(float),
            float depth = default(float)
        ) {
            LineRenderer lineRenderer = null != linePrefab ? Instantiate(linePrefab) : new LineRenderer();
            lineRenderer.name = "SimpleLine";

            if (default(float) != thickness) {
                lineRenderer.widthMultiplier = thickness;
            }

            if (default(Color) != color) {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, new Vector3(xFrom, yFrom, depth));
            lineRenderer.SetPosition(1, new Vector3(xTo, yTo, depth));

            return lineRenderer;
        }

        public static LineRenderer CreateMultiLine(
            Vector3[] positions,
            string name = "MultiLine",
            LineRenderer linePrefab = null,
            Color color = default(Color),
            float thickness = default(float)
        ) {
            if (null == positions) {
                return null;
            }

            LineRenderer lineRenderer = null != linePrefab ? Instantiate(linePrefab) : new LineRenderer();
            lineRenderer.name = string.Format("{0}[{1}]", name, positions.Length);

            if (default(float) != thickness) {
                lineRenderer.widthMultiplier = thickness;
            }

            if (default(Color) != color) {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

            return lineRenderer;
        }

        public static LineRenderer CreateRect(
            Rect bounds,
            LineRenderer linePrefab = null,
            Color color = default(Color), 
            float thickness = default(float),
            float depth = default(float)
        ) {
            Vector3[] positions = new Vector3[5];
            positions[0] = new Vector3(bounds.xMin, bounds.yMin, depth);
            positions[1] = new Vector3(bounds.xMax, bounds.yMin, depth);
            positions[2] = new Vector3(bounds.xMax, bounds.yMax, depth);
            positions[3] = new Vector3(bounds.xMin, bounds.yMax, depth);
            positions[4] = positions[0];

            return CreateMultiLine(
                positions,
                "Rectangle",
                linePrefab,
                color,
                thickness
            );
        }
    }
}