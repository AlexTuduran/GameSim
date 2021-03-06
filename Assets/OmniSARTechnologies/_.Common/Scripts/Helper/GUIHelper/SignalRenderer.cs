//
// Signal Renderer
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class SignalRenderer {
        public Rect bounds {
            get {
                return m_Bounds;
            }
        }
        private Rect m_Bounds;

        public LineRenderer signal {
            get {
                return m_Signal;
            }
        }
        private LineRenderer m_Signal = null;

        public Color signalColor {
            get {
                if (null == m_Signal) {
                    return default(Color);
                }

                return m_Signal.startColor;
            }

            set {
                if (null == m_Signal) {
                    return;
                }

                m_Signal.startColor = value;
                m_Signal.endColor = value;
            }
        }

        public float signalThickness {
            get {
                if (null == m_Signal) {
                    return 1.0f;
                }

                return m_Signal.widthMultiplier;
            }

            set {
                if (null == m_Signal) {
                    return;
                }

                m_Signal.widthMultiplier = value;
            }
        }

        public float GetSample(int index, bool isPositiveOnly) {
            if (null == m_Signal) {
                return 0.0f;
            }

            if (index < 0) {
                return 0.0f;
            }

            if (index >= m_Signal.positionCount) {
                return 0.0f;
            }

            return WorldSpaceToSampleSpace(m_Signal.GetPosition(index).y, isPositiveOnly);
        }

        public void SetSample(int index, float sample, bool isPositiveOnly) {
            if (null == m_Signal) {
                return;
            }

            if (index < 0) {
                return;
            }

            if (index >= m_Signal.positionCount) {
                return;
            }

            Vector3 position = m_Signal.GetPosition(index);
            position.y = SampleSpaceToWorldSpace(sample, isPositiveOnly);
            m_Signal.SetPosition(index, position);
        }

        public void SetSamples(float[] samples, bool isPositiveOnly) {
            if (null == m_Signal) {
                return;
            }

            if (null == samples) {
                return;
            }

            if (m_Signal.positionCount != samples.Length) {
                return;
            }

            Vector3[] positions = new Vector3[samples.Length];
            m_Signal.GetPositions(positions);

            for (int i = 0; i < samples.Length; i++) {
                positions[i].y = SampleSpaceToWorldSpace(samples[i], isPositiveOnly);
            }

            m_Signal.SetPositions(positions);
        }

       public LineRenderer border {
            get {
                return m_Border;
            }
        }
        private LineRenderer m_Border = null;

        public Color borderColor {
            get {
                if (null == m_Border) {
                    return default(Color);
                }

                return m_Border.startColor;
            }

            set {
                if (null == m_Border) {
                    return;
                }

                m_Border.startColor = value;
                m_Border.endColor = value;
            }
        }

        public float borderThickness {
            get {
                if (null == m_Signal) {
                    return 1.0f;
                }

                return m_Border.widthMultiplier;
            }

            set {
                if (null == m_Border) {
                    return;
                }

                m_Border.widthMultiplier = value;
            }
        }

        public float IndexSpaceToWorldSpace(int index, int maxIndex) {
            return m_Bounds.xMin + m_Bounds.width  * Mathf.InverseLerp(0, maxIndex - 1, index);
        }

        public int WorldSpaceToIndexSpace(float x, int maxIndex) {
            return Mathf.RoundToInt(((x - m_Bounds.xMin) / m_Bounds.width) * (maxIndex - 1));
        }

        public float SampleSpaceToWorldSpace(float sample, bool isPositiveOnly) {
            if (isPositiveOnly) {
                return m_Bounds.yMin + m_Bounds.height * Mathf.Clamp01(sample);
            } else {
                return m_Bounds.yMin + m_Bounds.height * (Mathf.Clamp(sample, -1.0f, +1.0f) * 0.5f + 0.5f);
            }
        }

        public float WorldSpaceToSampleSpace(float y, bool isPositiveOnly) {
            if (isPositiveOnly) {
                return (y - m_Bounds.yMin) / m_Bounds.height;
            } else {
                return (((y - m_Bounds.yMin) / m_Bounds.height) - 0.5f) * 2.0f;
            }
        }

        public void InitializeGraphics(
            int numSamples,
            bool isPositiveOnly,
            Rect bounds,
            LineRenderer linePrefab = null,
            Color signalColor = default(Color),
            float signalThickness = default(float),
            bool createBorder = false,
            float borderThickness = default(float),
            Color borderColor = default(Color),
            float borderDepth = default(float)
        ) {
            m_Bounds = bounds;

            if (null != m_Signal) {
                GameObject.Destroy(m_Signal);
                m_Signal = null;
            }

            m_Signal = CreateSignal(
                numSamples,
                isPositiveOnly,
                m_Bounds,
                linePrefab,
                signalColor,
                signalThickness
            );

            if (null != m_Border) {
                GameObject.Destroy(m_Border);
                m_Border = null;
            }

            m_Border = createBorder ? LineHelper.CreateRect(
                m_Bounds,
                linePrefab,
                borderColor,
                borderThickness,
                borderDepth
            ) : null;
        }

        public LineRenderer CreateSignal(
            int numSamples,
            bool isPositiveOnly,
            Rect bounds,
            LineRenderer linePrefab = null,
            Color color = default(Color),
            float thickness = default(float),
            float depth = default(float)
        ) {
            if (numSamples < 2) {
                return null;
            }

            Vector3[] positions = new Vector3[numSamples];

            for (int i = 0; i < numSamples; i++) {
                positions[i] = new Vector3(
                    IndexSpaceToWorldSpace(i, numSamples - 1),
                    SampleSpaceToWorldSpace(0.0f, isPositiveOnly),
                    depth
                );
            }

            return LineHelper.CreateMultiLine(
                positions,
                "SignalLine",
                linePrefab,
                color,
                thickness
            );
        }
    }
}