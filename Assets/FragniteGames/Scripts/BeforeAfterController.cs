using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace FragniteGames {
    [ExecuteInEditMode]
    public class BeforeAfterController : MonoBehaviour {
        [Header("References")]
        public RawImage[] beforeRawImages = null;
        public RawImage[] afterRawImages = null;
        public Text[] beforeTexts = null;
        public Text[] afterTexts = null;
        public Text titleText = null;
        public MaterialsFloatPropertyController materialsFloatPropertyController = null;
        public GameObject[] leftGameObjectsToActivate = null;
        public GameObject[] rightGameObjectsToActivate = null;

        [Header("Inputs")]
        public Texture2D beforeTexture = null;
        public Texture2D afterTexture = null;
        public string beforeCaption = "Before";
        public string afterCaption = "After";
        public string titleCaption = "The Doppler Effect";

        [Header("Parameters")]
        [Range(0, 1)] public float beforeAfterAmount = 0.5f;

        [Header("Animation")]
        public bool animate = false;
        public AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float animationLength = 3.0f;

        private float m_InitialTime = 0.0f;
        private bool m_LastAnimate = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            InitializeTime();

            UpdateAll();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValidate() {
            animationLength = Mathf.Max(0.01f, animationLength);

            UpdateAll();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitializeTime() {
            m_InitialTime = GetTime();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetTime() {
            return Time.time;
            //return Time.realtimeSinceStartup;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetElapsedTime() {
            return GetTime() - m_InitialTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateRawImages() {
            if (beforeRawImages == null) {
                return;
            }

            if (afterRawImages == null) {
                return;
            }

            int bLength = beforeRawImages.Length;
            if (bLength < 1) {
                return;
            }

            int aLength = afterRawImages.Length;
            if (aLength < 1) {
                return;
            }

            for (int i = 0; i < bLength; i++) {
                var rawImage = beforeRawImages[i];

                if (!rawImage) {
                    continue;
                }

                rawImage.texture = beforeTexture;
            }

            for (int i = 0; i < aLength; i++) {
                var rawImage = afterRawImages[i];

                if (!rawImage) {
                    continue;
                }

                rawImage.texture = afterTexture;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateTexts() {
            if (beforeTexts == null) {
                return;
            }

            if (afterTexts == null) {
                return;
            }

            if (titleText == null) {
                return;
            }

            int bLength = beforeTexts.Length;
            if (bLength < 1) {
                return;
            }

            int aLength = afterTexts.Length;
            if (aLength < 1) {
                return;
            }

            for (int i = 0; i < bLength; i++) {
                var text = beforeTexts[i];

                if (!text) {
                    continue;
                }

                text.text = beforeCaption;
            }

            for (int i = 0; i < aLength; i++) {
                var text = afterTexts[i];

                if (!text) {
                    continue;
                }

                text.text = afterCaption;
            }

            titleText.text = titleCaption;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateMaterials() {
            if (!materialsFloatPropertyController) {
                return;
            }

            if (animate) {
                if (animate != m_LastAnimate) {
                    InitializeTime();
                    m_LastAnimate = animate;
                }

                beforeAfterAmount = animationCurve.Evaluate(GetElapsedTime() / animationLength);
                beforeAfterAmount = Mathf.Clamp01(beforeAfterAmount);
            }

            materialsFloatPropertyController.materialPropertyValue = beforeAfterAmount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateGameObjects() {
            if (leftGameObjectsToActivate == null) {
                return;
            }

            if (rightGameObjectsToActivate == null) {
                return;
            }

            int lLength = leftGameObjectsToActivate.Length;
            if (lLength < 1) {
                return;
            }

            int rLength = rightGameObjectsToActivate.Length;
            if (rLength < 1) {
                return;
            }

            bool lEnabled = beforeAfterAmount < 0.5f;
            bool rEnabled = !lEnabled;

            for (int i = 0; i < lLength; i++) {
                var go = leftGameObjectsToActivate[i];

                if (!go) {
                    continue;
                }

                go.SetActive(lEnabled);
            }
            
            for (int i = 0; i < rLength; i++) {
                var go = rightGameObjectsToActivate[i];

                if (!go) {
                    continue;
                }

                go.SetActive(rEnabled);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateAll() {
            UpdateRawImages();
            UpdateTexts();
            UpdateMaterials();
            UpdateGameObjects();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            UpdateAll();
        }
    }
}