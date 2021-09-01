using System.Collections;
using System.Runtime.CompilerServices;
using OmniSARTechnologies.Utils;
using UnityEngine;

namespace FragniteGames {
    public enum BeforeAfterSequenceRenderingStatus {
        Ready,
        NullFrameCapturer,
        NullBeforeAfterController,
        NullBeforeTextures,
        NullAfterTextures,
        EmptyBeforeTextures,
        DifferentArraysLength,
        NullBeforeTexture,
        NullAfterTexture,
        NotReady
    }

    [ExecuteInEditMode]
    public class BeforeAfterSequenceController : MonoBehaviour {
        [Header("References")]
        public FrameCapturer frameCapturer = null;
        public BeforeAfterController beforeAfterController = null;

        [Header("Inputs")]
        public Texture2D[] beforeTextures = null;
        public Texture2D[] afterTextures = null;

        [Header("Sequences")]
        public bool previewOnly = false;
        public bool renderAllSequences = false;
        public int sequenceIndexToRender = 0;
        public int framesPerSecond = 24;
        public int framesPerSequence = 49;
        public AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public int NumSequences => beforeTextures?.Length ?? 0;
        public int LastSequenceIndex => Mathf.Max(0, NumSequences - 1);

        private BeforeAfterSequenceRenderingStatus m_RenderingStatus = BeforeAfterSequenceRenderingStatus.NotReady;
        public BeforeAfterSequenceRenderingStatus RenderingStatus {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return m_RenderingStatus;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string BeforeAfterSequenceRenderingStatusToString(BeforeAfterSequenceRenderingStatus beforeAfterSequenceRenderingStatus) {
            switch (beforeAfterSequenceRenderingStatus) {
                case BeforeAfterSequenceRenderingStatus.Ready                     : return "Ready for rendering";
                case BeforeAfterSequenceRenderingStatus.NullFrameCapturer         : return "'Frame Capturer' is NULL";
                case BeforeAfterSequenceRenderingStatus.NullBeforeAfterController : return "'Before After Controller' is NULL";
                case BeforeAfterSequenceRenderingStatus.NullBeforeTextures        : return "'Before Textures' array is NULL";
                case BeforeAfterSequenceRenderingStatus.NullAfterTextures         : return "'After Textures' array is NULL";
                case BeforeAfterSequenceRenderingStatus.EmptyBeforeTextures       : return "'Before Textures' array is empty";
                case BeforeAfterSequenceRenderingStatus.DifferentArraysLength     : return "'After Textures' and 'Before Textures' arrays lengths are different";
                case BeforeAfterSequenceRenderingStatus.NullBeforeTexture         : return "A slot in the 'Before Textures' array is NULL";
                case BeforeAfterSequenceRenderingStatus.NullAfterTexture          : return "A slot in the 'After Textures' array is NULL";
                case BeforeAfterSequenceRenderingStatus.NotReady                  : return "Not ready for rendering";
                default: return "Unknown";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValidate() {
            sequenceIndexToRender = Mathf.Clamp(sequenceIndexToRender, 0, LastSequenceIndex);
            m_RenderingStatus = ComputeRenderingStatus();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private BeforeAfterSequenceRenderingStatus ComputeRenderingStatus() {
            if (!frameCapturer) {
                return BeforeAfterSequenceRenderingStatus.NullFrameCapturer;
            }

            if (!beforeAfterController) {
                return BeforeAfterSequenceRenderingStatus.NullBeforeAfterController;
            }

            if (beforeTextures == null) {
                return BeforeAfterSequenceRenderingStatus.NullBeforeTextures;
            }

            if (afterTextures == null) {
                return BeforeAfterSequenceRenderingStatus.NullAfterTextures;
            }

            int length = beforeTextures.Length;
            if (length < 1) {
                return BeforeAfterSequenceRenderingStatus.EmptyBeforeTextures;
            }

            if (afterTextures.Length != length) {
                return BeforeAfterSequenceRenderingStatus.DifferentArraysLength;
            }

            for (int i = 0; i < length; i++) {
                var tex = beforeTextures[i];

                if (!tex) {
                    return BeforeAfterSequenceRenderingStatus.NullBeforeTexture;
                }
            }

            for (int i = 0; i < length; i++) {
                var tex = afterTextures[i];

                if (!tex) {
                    return BeforeAfterSequenceRenderingStatus.NullAfterTexture;
                }
            }

            return BeforeAfterSequenceRenderingStatus.Ready;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator StartRenderingCoroutine() {
            Debug.LogFormat("Rendering started{0}.", previewOnly ? " (preview)" : string.Empty);
            yield return null;

            int numFrames = framesPerSequence;
            int lastFrameIndex = numFrames - 1;
            float lastFrameIndexRcp = 1.0f / lastFrameIndex;
            float frameTime = 1.0f / framesPerSecond;
            yield return null;

            if (renderAllSequences) {
                int totalNumFrames = numFrames * NumSequences;
                int totalNumRenderedFrames = 0;
                yield return null;

                for (int j = 0; j < NumSequences; j++) {
                    beforeAfterController.beforeTexture = beforeTextures[j];
                    beforeAfterController.afterTexture = afterTextures[j];
                    beforeAfterController.beforeAfterAmount = 1.0f;
                    beforeAfterController.Update();

                    if (!previewOnly) {
                        yield return new WaitForEndOfFrame();
                    }

                    for (int i = 0; i < numFrames; i++) {
                        float t = i * lastFrameIndexRcp;
                        float animatedT = animationCurve.Evaluate(t);

                        beforeAfterController.beforeAfterAmount = animatedT;
                        beforeAfterController.Update();

                        if (previewOnly) {
                            yield return new WaitForSeconds(frameTime);
                        } else {
                            yield return new WaitForEndOfFrame();
                            
                            frameCapturer.CaptureFrameTagged(string.Format("SEQ-{0:D06}", j));
                            totalNumRenderedFrames++;

                            Debug.LogFormat(
                                "Sequence {0} of {1}: Rendered frame {2} of {3} ({4:F2}% completed). Time.FrameCount = {5}",
                                j + 1,
                                NumSequences,
                                totalNumRenderedFrames,
                                totalNumFrames,
                                (float)totalNumRenderedFrames / totalNumFrames * 100.0f,
                                Time.frameCount
                            );
                            yield return null;
                        }
                    }
                    yield return null;
                }
            } else {
                beforeAfterController.beforeTexture = beforeTextures[sequenceIndexToRender];
                beforeAfterController.afterTexture = afterTextures[sequenceIndexToRender];
                beforeAfterController.beforeAfterAmount = 1.0f;
                beforeAfterController.Update();

                if (!previewOnly) {
                    yield return new WaitForEndOfFrame();
                }

                for (int i = 0; i < numFrames; i++) {
                    float t = i * lastFrameIndexRcp;
                    float animatedT = animationCurve.Evaluate(t);

                    beforeAfterController.beforeAfterAmount = animatedT;
                    beforeAfterController.Update();

                    if (previewOnly) {
                        yield return new WaitForSeconds(frameTime);
                    } else {
                        yield return new WaitForEndOfFrame();

                        frameCapturer.CaptureFrameTagged(string.Format("SEQ-{0:D06}", sequenceIndexToRender));

                        Debug.LogFormat(
                            "Rendered frame {0} of {1} ({2:F2}% completed). Time.FrameCount = {3}",
                            i + 1,
                            numFrames,
                            (float)(i + 1) / numFrames * 100.0f,
                            Time.frameCount
                        );
                        yield return null;
                    }
                }
                yield return null;
            }

            Debug.LogFormat("Rendering stopped{0}.", previewOnly ? " (preview)" : string.Empty);
            yield return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartRendering() {
            StopRendering();

            StartCoroutine(StartRenderingCoroutine());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopRendering() {
            StopAllCoroutines();
        }
    }
}