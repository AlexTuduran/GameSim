//
// Lite FPS Counter
//
// Version    : 1.0.0
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
//

// sync with LiteFPSCounterEditor.cs
#define __APPLY_SETTINGS_ON_VALIDATE__

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using OmniSARTechnologies.Helper;

namespace OmniSARTechnologies.LiteFPSCounter {
    public enum TextPosition {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        Center,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public enum TextSize {
        Tiny = 12,
        Small = 16,
        Regular = 22,
        Big = 28,
        Large = 36,
        Huge = 52,
        Gigantic = 72,
        Ginormous = 100
    }

    public enum TextEnhancement {
        None,
        Shadow,
        Outline,
        AllCombined
    }

    [SelectionBase]
    [DisallowMultipleComponent]
    public class LiteFPSCounter : MonoBehaviour {
        /// <summary>
        /// Reference to a Text component where the dynamic info will be displayed.
        /// <para></para>
        /// <para></para>
        /// Make sure the referenced UI Text component is not expensive to draw and also not 
        /// expensive to update (keep it as simple and efficient as possible).
        /// </summary>
        [Header("GUI")]
        [Tooltip(
            "Reference to a Text component where the dynamic info will be displayed.\n\r\n\r" +
            "Make sure the referenced UI Text component is not expensive to draw and also not " +
            "expensive to update (keep it as simple and efficient as possible)."
        )]
        public Text dynamicInfoText;

        /// <summary>
        /// Visibility of the dynamic info text.
        /// </summary>
        [Tooltip("Visibility of the dynamic info text.")]
        [DisplayNameAttribute("Visible")]
        public bool dynamicInfoTextVisible = true;

        /// <summary>
        /// Color for the framerate field(s).
        /// </summary>
        [Tooltip("Color for the framerate field(s).")]
        [DisplayNameAttribute("FPS Color")]
        public Color fpsFieldsColor = ColorHelper.HexStrToColor("#80FF00FF");

        /// <summary>
        /// Color for the min framerate field(s).
        /// </summary>
        [Tooltip("Color for the min framerate field(s).")]
        [DisplayNameAttribute("Min FPS Color")]
        public Color fpsMinFieldsColor = ColorHelper.HexStrToColor("#FF8400FF");

        /// <summary>
        /// Color for the max framerate field(s).
        /// </summary>
        [Tooltip("Color for the max framerate field(s).")]
        [DisplayNameAttribute("Max FPS Color")]
        public Color fpsMaxFieldsColor = ColorHelper.HexStrToColor("#00A0FFFF");

        /// <summary>
        /// Color for the framerate fluctuation field(s).
        /// </summary>
        [Tooltip("Color for the framerate fluctuation field(s).")]
        [DisplayNameAttribute("FPS Range Color")]
        public Color fpsFluctuationFieldsColor = ColorHelper.HexStrToColor("#DCEC00FF");

        /// <summary>
        /// Reference to a Text component where the static info will be displayed.
        /// <para></para>
        /// <para></para>
        /// Although this field will rarely be updated, still make sure the referenced UI Text 
        /// component is at least not expensive to draw.
        /// </summary>
        [Tooltip(
            "Reference to a Text component where the static info will be displayed.\n\r\n\r" +
            "Although this field will rarely be updated, still make sure the referenced UI Text " +
            "component is at least not expensive to draw."
        )]
        public Text staticInfoText;

        /// <summary>
        /// Visibility of the static text info.
        /// </summary>
        [Tooltip("Visibility of the static info text.")]
        [DisplayNameAttribute("Visible")]
        public bool staticInfoTextVisible = true;

        /// <summary>
        /// Color for the GPU field(s).
        /// </summary>
        [Tooltip("Color for the GPU field(s).")]
        [DisplayNameAttribute("GPU Info Color")]
        public Color gpuFieldsColor = ColorHelper.HexStrToColor("#FF5020FF");

        /// <summary>
        /// Color for the GPU detailed field(s).
        /// </summary>
        [Tooltip("Color for the GPU detailed field(s).")]
        [DisplayNameAttribute("GPU Misc. Color")]
        public Color gpuDetailFieldsColor = ColorHelper.HexStrToColor("#FF3379FF");

        /// <summary>
        /// Color for the CPU field(s).
        /// </summary>
        [Tooltip("Color for the CPU field(s).")]
        [DisplayNameAttribute("GPU Info Color")]
        public Color cpuFieldsColor = ColorHelper.HexStrToColor("#0090CBFF");

        /// <summary>
        /// Color for the system field(s).
        /// </summary>
        [Tooltip("Color for the system field(s).")]
        [DisplayNameAttribute("SYS Info Color")]
        public Color sysFieldsColor = ColorHelper.HexStrToColor("#C9D700FF");

        /// <summary>
        /// Screen-space position of the referenced Text components.
        /// </summary>
        [Tooltip("Screen-space position of the referenced Text components.")]
        [DisplayNameAttribute("Text Position")]
        public TextPosition textPosition = TextPosition.TopRight;

        /// <summary>
        /// Size of the text.
        /// </summary>
        [Tooltip("Size of the text.")]
        [DisplayNameAttribute("Text Size")]
        public TextSize textSize = TextSize.Regular;

        /// <summary>
        /// Various enhancements can be applied to the referenced Text
        /// components to increase readability.
        /// </summary>
        [Tooltip(
            "Various enhancements can be applied to the referenced Text " +
            "components to increase readability.\n\r\n\r" +
            "Choose:\n\r\n\r" +
            "● None for the maximum performance, but sacrificing readability. With this" +
            "option, the displayed info might be hard to read in certain conditions.\n\r\n\r" +
            "● Shadow as a fairly good compromise between visuals and performance. Although not " +
            "as good as Outline, the Shadow option still provides decent readability.\n\r\n\r" +
            "● Outline for the best visual enhancement, sacrificing a bit of performance (few " +
            "ms ONCE every second). This is the recommended option.\n\r\n\r" +
            "● All Combined for extreme readability.\n\r\n\r" +
            "The recommended option is Outline, since this option yields the best visual " +
            "enhancement and readability while performance is still really high.\n\r\n\r"
        )]
        [DisplayNameAttribute("Text Enhancement")]
        public TextEnhancement textEnhancement = TextEnhancement.Outline;

        /// <summary>
        /// This setting controlls how fast the components updates both in terms of visuals and probing. 
        /// <para></para>
        /// <para></para>
        /// Although the framerate probing mechanism is as less intrusive as possible, updating the component 
        /// too often may result in framerate inacurracy, as the component starts adding some execution 
        /// overhead that could possibly result in lowering the overall game framerate.
        /// <para></para>
        /// <para></para>
        /// Usually, values between 250 and 1000 ms are common for framerate updates and are as well safe.
        /// <para></para>
        /// <para></para>
        /// <remarks>
        /// Do mind however that the the Update Interval is the interval in which the Text components are
        /// updated, so higher values will yield better perofmance.
        /// </remarks>
        /// </summary>
        [Header("Behaviour")]
        [Tooltip(
            "This setting controlls how fast the components updates both in terms of visuals and probing.\n\r\n\r" +
            "Although the framerate probing mechanism is as less intrusive as possible, updating the component " +
            "too often may result in framerate inacurracy, as the component starts adding some execution " +
            "overhead that could possibly result in lowering the overall game framerate.\n\r\n\r" +
            "Usually, values between 250 and 1000 ms are common for framerate updates and are as well safe.\n\r\n\r" +
            "Do mind however that the the Update Interval is the interval in which the Text components are " +
            "updated, so higher values will yield better perofmance."
        )]
        [Range(0.05f, 2.0f)]
        [DisplayNameAttribute("Update Interval")]
        public float updateInterval = 0.5f;

        /// <summary>
        /// Registered frame time within the update interval.
        /// </summary>
        public float frameTime {
            get {
                return m_FrameTime;
            }
        }
        private float m_FrameTime = 0.0f;

        /// <summary>
        /// Minimum registered frame time within the update interval.
        /// </summary>
        public float minFrameTime {
            get {
                return m_MinFrameTime;
            }
        }
        private float m_MinFrameTime = 0.0f;

        /// <summary>
        /// Maximum registered frame time within the update interval.
        /// </summary>
        public float maxFrameTime {
            get {
                return m_MaxFrameTime;
            }
        }
        private float m_MaxFrameTime = 0.0f;

        /// <summary>
        /// Fluctuation of the registered frame time within the update interval.
        /// </summary>
        public float frameTimeFlutuation {
            get {
                return m_FrameTimeFlutuation;
            }
        }
        private float m_FrameTimeFlutuation = 0.0f;

        /// <summary>
        /// Registered framerate within the update interval.
        /// </summary>
        public float frameRate {
            get {
                return m_FrameRate;
            }
        }
        private float m_FrameRate = 0.0f;

        /// <summary>
        /// Minimum registered framerate within the update interval.
        /// </summary>
        public float minFrameRate {
            get {
                return m_MinFrameRate;
            }
        }
        private float m_MinFrameRate = 0.0f;

        /// <summary>
        /// Maximum registered framerate within the update interval.
        /// </summary>
        public float maxFrameRate {
            get {
                return m_MaxFrameRate;
            }
        }
        private float m_MaxFrameRate = 0.0f;

        /// <summary>
        /// Framerate fluctuation within the update interval.
        /// </summary>
        public float frameRateFlutuation {
            get {
                return m_FrameRateFlutuation;
            }
        }
        private float m_FrameRateFlutuation = 0.0f;

        private float m_AccumulatedTime;
        private int m_AccumulatedFrames;
        private float m_LastUpdateTime;
        private string m_StaticInfoDisplay;
        private string m_DynamicConfigurationFormat;

        private static float MinTime = 0.000000001f; // equivalent to 1B fps
        private Vector2 TextOffset = new Vector2(8.0f, 8.0f);
        private float StaticInfoYOffset = 4.0f;

        /// <summary>
        /// Initializes (and resets) the component.
        /// <para></para>
        /// <para></para>
        /// <remarks>
        /// The initialization only targets the component's internal data.
        /// </remarks>
        /// </summary>
        public void Initialize() {
            Reset();
            UpdateInternals();
        }

        /// <summary>
        /// Resets the framerate probing data.
        /// <para></para>
        /// <para></para>
        /// <remarks>
        /// This does not reset the component's inspector state.
        /// </remarks>
        /// </summary>
        public void Reset() {
            ResetProbingData();

            m_LastUpdateTime = Time.realtimeSinceStartup;
        }

        private void Start() {
            Initialize();
        }

        private void OnEnable() {
            Initialize();
        }

#if __APPLY_SETTINGS_ON_VALIDATE__
        private void OnValidate() {
            // issues warnings due to RectTransform.set_anchorMin/Max calling
            // SendMessage and that's not allowed in OnValidate
            ApplySettings();
        }
#endif

        private void UpdateTextSettings() {
            UpdateTextVisibility();
            UpdateTextEnhancement();
            UpdateTextSize();
            UpdateTextPosition();
        }

        private void UpdateContent() {
            UpdateStaticContentAndData();
            UpdateDynamicContent();
        }

        private void UpdateInternals() {
            UpdateContent();
            UpdateTextSettings();
            Canvas.ForceUpdateCanvases();
        }

        public void ApplySettings() {
            UpdateInternals();
        }

        private void UpdateStaticContentAndData() {
            m_DynamicConfigurationFormat = string.Format(
                "{0} FPS {1} ms {2}"   + Environment.NewLine +
                "{3} FPS {4} ms {5}"   + Environment.NewLine +
                "{6} FPS {7} ms {8}"   + Environment.NewLine +
                "{9} FPS {10} ms {11}",

                ColorHelper.ColorText("{0}", fpsFieldsColor),
                ColorHelper.ColorText("{1}", fpsFieldsColor),
                ColorHelper.ColorText("Σ", fpsFieldsColor),

                ColorHelper.ColorText("{2}", fpsMinFieldsColor),
                ColorHelper.ColorText("{3}", fpsMinFieldsColor),
                ColorHelper.ColorText("⇓", fpsMinFieldsColor),

                ColorHelper.ColorText("{4}", fpsMaxFieldsColor),
                ColorHelper.ColorText("{5}", fpsMaxFieldsColor),
                ColorHelper.ColorText("⇑", fpsMaxFieldsColor),

                ColorHelper.ColorText("{6}", fpsFluctuationFieldsColor),
                ColorHelper.ColorText("{7}", fpsFluctuationFieldsColor),
                ColorHelper.ColorText("∿", fpsFluctuationFieldsColor)
            );

            if (!staticInfoText) {
                return;
            }

            staticInfoText.text = string.Format(
                "{0} {1}" + Environment.NewLine +
                "{2} MB VRAM" + Environment.NewLine +
                "{3}" + Environment.NewLine +
                "{4} MB RAM" + Environment.NewLine +
                "{5}",
                ColorHelper.ColorText(SystemInfo.graphicsDeviceName, gpuFieldsColor),
                ColorHelper.ColorText("[" + SystemInfo.graphicsDeviceType.ToString() + "]", gpuDetailFieldsColor),
                ColorHelper.ColorText(SystemInfo.graphicsMemorySize.ToString(), gpuFieldsColor),
                ColorHelper.ColorText(SystemInfo.processorType, cpuFieldsColor),
                ColorHelper.ColorText(SystemInfo.systemMemorySize.ToString(), cpuFieldsColor),
                ColorHelper.ColorText(SystemInfo.operatingSystem, sysFieldsColor)
            );
        }

        private void UpdateDynamicContent() {
            if (!dynamicInfoText) {
                return;
            }

            dynamicInfoText.text = string.Format(
                m_DynamicConfigurationFormat,
                m_FrameRate.ToString("F2"),           (m_FrameTime * 1000.0f).ToString("F3"),
                m_MinFrameRate.ToString("F2"),        (m_MaxFrameTime * 1000.0f).ToString("F3"),
                m_MaxFrameRate.ToString("F2"),        (m_MinFrameTime * 1000.0f).ToString("F3"),
                m_FrameRateFlutuation.ToString("F2"), (m_FrameTimeFlutuation * 1000.0f).ToString("F3")
            );
        }

        private float GetTextLineHeight(Text text) {
            if (!text) {
                return 0;
            }

            if (null == text.canvas) {
                return 0;
            }

            TextGenerationSettings generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size); 
            return text.cachedTextGenerator.GetPreferredHeight("0", generationSettings) * 1.25f / text.canvas.scaleFactor + 0.55f;
        }

        private bool UpdateTextPosition(Text text, Vector2 offset, float staticInfoYOffset, int dynamicInfoNumLines, int staticInfoNumLines, bool staticInfo) {
            if (!text) {
                return false;
            }

            RectTransform rectTransform = text.rectTransform;
            float lineHeight = GetTextLineHeight(text);
            float dynamicInfoHeight = staticInfo ? lineHeight * dynamicInfoNumLines + staticInfoYOffset : 0;
            float staticInfoHeight = staticInfo ? lineHeight * staticInfoNumLines : 0;

            switch (textPosition) {
                /*
                 * top
                 */

                case TextPosition.TopLeft: {
                    rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
                    rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
                    rectTransform.anchoredPosition = new Vector2(
                        +offset.x,
                        -offset.y - dynamicInfoHeight
                    );
                    text.alignment = TextAnchor.UpperLeft;
                    break;
                }
                
                case TextPosition.TopCenter: {
                    rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
                    rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
                    rectTransform.anchoredPosition = new Vector2(
                        0.0f,
                        -offset.y - dynamicInfoHeight
                    );
                    text.alignment = TextAnchor.UpperCenter;
                    break;
                }
                
                case TextPosition.TopRight: {
                    rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
                    rectTransform.anchorMin = new Vector2(1.0f, 1.0f);
                    rectTransform.anchoredPosition = new Vector2(
                        -offset.x,
                        -offset.y - dynamicInfoHeight
                    );
                    text.alignment = TextAnchor.UpperRight;
                    break;
                }
                
                /*
                 * center
                 */
                
                case TextPosition.CenterLeft: {
                    rectTransform.anchorMax = new Vector2(0.0f, 0.5f);
                    rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
                    rectTransform.anchoredPosition = new Vector2(
                        +offset.x,
                        0.0f - (dynamicInfoHeight + staticInfoHeight) * 0.5f
                    );
                    text.alignment = TextAnchor.MiddleLeft;
                    break;
                }
                
                case TextPosition.Center: {
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchoredPosition = new Vector2(
                        0.0f,
                        0.0f - (dynamicInfoHeight + staticInfoHeight) * 0.5f
                    );
                    text.alignment = TextAnchor.MiddleCenter;
                    break;
                }
                
                case TextPosition.CenterRight: {
                    rectTransform.anchorMax = new Vector2(1.0f, 0.5f);
                    rectTransform.anchorMin = new Vector2(1.0f, 0.5f);
                    rectTransform.anchoredPosition = new Vector2(
                        -offset.x,
                        0.0f - (dynamicInfoHeight + staticInfoHeight) * 0.5f
                    );
                    text.alignment = TextAnchor.MiddleRight;
                    break;
                }
                
                /*
                 * bottom
                 */
                
                case TextPosition.BottomLeft: {
                    rectTransform.anchorMax = new Vector2(0.0f, 0.0f);
                    rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                    rectTransform.anchoredPosition = new Vector2(
                        +offset.x,
                        +offset.y + dynamicInfoHeight
                    );
                    text.alignment = TextAnchor.LowerLeft;
                    break;
                }
                
                case TextPosition.BottomCenter: {
                    rectTransform.anchorMax = new Vector2(0.5f, 0.0f);
                    rectTransform.anchorMin = new Vector2(0.5f, 0.0f);
                    rectTransform.anchoredPosition = new Vector2(
                        0.0f,
                        +offset.y + dynamicInfoHeight
                    );
                    text.alignment = TextAnchor.LowerCenter;
                    break;
                }
                
                case TextPosition.BottomRight: {
                    rectTransform.anchorMax = new Vector2(1.0f, 0.0f);
                    rectTransform.anchorMin = new Vector2(1.0f, 0.0f);
                    rectTransform.anchoredPosition = new Vector2(
                        -offset.x,
                        +offset.y + dynamicInfoHeight
                    );
                    text.alignment = TextAnchor.LowerRight;
                    break;
                }
            }

            return true;
        }

        private void UpdateTextPosition() {
            // text.cachedTextGenerator.lineCount doesn't always
            // correctly return the actual number of lines
            //int dynamicInfoNumLines = dynamicInfoText.cachedTextGenerator.lineCount;
            //int staticInfoNumLines = staticInfoText.cachedTextGenerator.lineCount;
            string[] splitSeparator = new string[] { Environment.NewLine };
            int dynamicInfoNumLines = dynamicInfoTextVisible ? dynamicInfoText.text.Split(splitSeparator, 0).Length : 0;
            int staticInfoNumLines = staticInfoText.text.Split(splitSeparator, 0).Length;

            UpdateTextPosition(
                dynamicInfoText,
                TextOffset,
                StaticInfoYOffset,
                dynamicInfoNumLines,
                staticInfoNumLines,
                false
            );

            UpdateTextPosition(
                staticInfoText,
                TextOffset,
                StaticInfoYOffset,
                dynamicInfoNumLines,
                staticInfoNumLines,
                true
            );
        }

        private bool UpdateTextSize(Text text) {
            if (!text) {
                return false;
            }

            text.fontSize = (int)textSize;
            return true;
        }

        private void UpdateTextSize() {
            UpdateTextSize(dynamicInfoText);
            UpdateTextSize(staticInfoText);
        }

        private T GetTextComponent<T>(Text text) where T : BaseMeshEffect {
            if (!text) {
                return null;
            }

            // if called for Shadow, it will still return the Outline
            // component because the Outline class extends Shadow
            T[] components = text.gameObject.GetComponents<T>();

            // so we have to iterate and filter
            T component = null;
            for (int i = 0; i < components.Length; i++) {
                if (components[i].GetType() == typeof(Shadow)) {
                    component = components[i];
                    // do not break
                }

                if (components[i].GetType() == typeof(Outline)) {
                    component = components[i];
                    // do not break
                }
            }

            return component;
        }

        private bool EnableTextComponent<T>(Text text, bool enable) where T : BaseMeshEffect {
            if (!text) {
                return false;
            }

            T component = GetTextComponent<T>(text);

            if (null == component) {
                return false;
            }

            component.enabled = enable;
            return true;
        }

        private bool UpdateTextComponentEnhancement(Text text) {
            if (!text) {
                return false;
            }

            bool success = true;

            switch (textEnhancement) {
                case TextEnhancement.None: {
                    success &= EnableTextComponent<Shadow>(text, false);
                    success &= EnableTextComponent<Outline>(text, false);
                    break;
                }
                
                case TextEnhancement.Shadow: {
                    success &= EnableTextComponent<Shadow>(text, true);
                    success &= EnableTextComponent<Outline>(text, false);
                    break;
                }
                
                case TextEnhancement.Outline: {
                    success &= EnableTextComponent<Shadow>(text, false);
                    success &= EnableTextComponent<Outline>(text, true);
                    break;
                }
                
                case TextEnhancement.AllCombined: {
                    success &= EnableTextComponent<Shadow>(text, true);
                    success &= EnableTextComponent<Outline>(text, true);
                    break;
                }
            }

            return success;
        }

        private void UpdateTextEnhancement() {
            UpdateTextComponentEnhancement(dynamicInfoText);
            UpdateTextComponentEnhancement(staticInfoText);
        }

        private void UpdateTextVisibility() {
            if (null != dynamicInfoText) {
                dynamicInfoText.gameObject.SetActive(dynamicInfoTextVisible);
            }

            if (null != staticInfoText) {
                staticInfoText.gameObject.SetActive(staticInfoTextVisible);
            }
        }

        private void ResetProbingData() {
            m_MinFrameTime = float.MaxValue;
            m_MaxFrameTime = float.MinValue;
            m_AccumulatedTime = 0.0f;
            m_AccumulatedFrames = 0;
        }

        private void UpdateFPS() {
            if (!dynamicInfoText) {
                return;
            }

            float deltaTime = Time.unscaledDeltaTime;

            m_AccumulatedTime += deltaTime;
            m_AccumulatedFrames++;

            if (deltaTime < MinTime) {
                deltaTime = MinTime;
            }

            if (deltaTime < m_MinFrameTime) {
                m_MinFrameTime = deltaTime;
            }

            if (deltaTime > m_MaxFrameTime) {
                m_MaxFrameTime = deltaTime;
            }

            float nowTime = Time.realtimeSinceStartup;
            if (nowTime - m_LastUpdateTime < updateInterval) {
                return;
            }

            if (m_AccumulatedTime < MinTime) {
                m_AccumulatedTime = MinTime;
            }

            if (m_AccumulatedFrames < 1) {
                m_AccumulatedFrames = 1;
            }

            m_FrameTime = m_AccumulatedTime / m_AccumulatedFrames;
            m_FrameRate = 1.0f / m_FrameTime;

            m_MinFrameRate = 1.0f / m_MaxFrameTime;
            m_MaxFrameRate = 1.0f / m_MinFrameTime;

            m_FrameTimeFlutuation = Mathf.Abs(m_MaxFrameTime - m_MinFrameTime) / 2.0f;
            m_FrameRateFlutuation = Mathf.Abs(m_MaxFrameRate - m_MinFrameRate) / 2.0f;

            UpdateDynamicContent();

            ResetProbingData();
            m_LastUpdateTime = nowTime;
        }

        private void Update() {
            UpdateFPS();
        }
    }
}