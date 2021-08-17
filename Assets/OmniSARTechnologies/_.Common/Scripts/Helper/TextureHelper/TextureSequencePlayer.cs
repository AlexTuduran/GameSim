//
// Texture Sequence Player
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace OmniSARTechnologies.Helper {
    public enum FrameWrappingType {
        None,
        Hold,
        Loop,
        PingPong
    }

    //[ExecuteInEditMode]
    public class TextureSequencePlayer : MonoBehaviour {
        public Texture2D[] textureSequence;
        public float frameRate = 30.0f;
        public bool playOnAwake = false;
        public int frame = 0;
        public FrameWrappingType preStartWrapping = FrameWrappingType.Hold;
        public FrameWrappingType postEndWrapping = FrameWrappingType.Hold;
        public bool outputToRenderTexture = true;
        public RenderTexture targetTexture;
        public bool outputToRawImage = true;
        public RawImage targetRawImage;

        private float _startTime;
        private bool _isPlaying;

        public int FrameCount {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return GetNumFrames();
            }
        }

        public const int kInvalidFrame = -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Awake() {
            if (playOnAwake) {
                Play();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            UpdateOutput();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnValidate() {
            UpdateOutput();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GetTime() {
            return Time.timeSinceLevelLoad;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GetPlayTime() {
            return GetTime() - _startTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetPlayFrame() {
            return Mathf.FloorToInt(GetPlayTime() * frameRate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Mod(float value, float period) {
            if (value < 0.0f) {
                return value % period + period;
            }

            return value % period;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float TriangleFunction(float value) {
            return Mathf.Abs(1.0f - Mod((value - 1.0f) * 0.5f, 1.0f) * 2.0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ScaledTriangleFunction(float value, float scale) {
            return TriangleFunction(value / scale) * scale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int WrapFrame(int frameNumber) {
            int numFrames = GetNumFrames();

            if (numFrames < 1) {
                return kInvalidFrame;
            }

            if (numFrames < 2) {
                return 0;
            }

            int lastFrame = numFrames - 1;

            if (frameNumber < 0) {
                switch (preStartWrapping) {
                    case FrameWrappingType.None: return kInvalidFrame;
                    case FrameWrappingType.Hold: return 0;
                    case FrameWrappingType.Loop: return (frameNumber + numFrames * 1000000) % numFrames;
                    case FrameWrappingType.PingPong: return Mathf.FloorToInt(ScaledTriangleFunction(frameNumber, lastFrame));
                    default: return kInvalidFrame;
                }
            }

            if (frameNumber > lastFrame) {
                switch (postEndWrapping) {
                    case FrameWrappingType.None: return kInvalidFrame;
                    case FrameWrappingType.Hold: return lastFrame;
                    case FrameWrappingType.Loop: return frameNumber % numFrames;
                    case FrameWrappingType.PingPong: return Mathf.FloorToInt(ScaledTriangleFunction(frameNumber, lastFrame));
                    default: return kInvalidFrame;
                }
            }

            return frameNumber;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetNumFrames() {
            if (null == textureSequence) {
                return 0;
            }

            return textureSequence.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLastFrame() {
            return GetNumFrames() - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearOutput() {
            if (outputToRenderTexture) {
                RenderTexture currentActiveRT = RenderTexture.active;
                RenderTexture.active = targetTexture;
                GL.Clear(true, true, Color.black);
                RenderTexture.active = currentActiveRT;
            }

            if (outputToRawImage) {
                targetRawImage.texture = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateOutput() {
            int wrappedFrame = WrapFrame(frame);

#if !true
            Debug.LogFormat("{0} ({1})", wrappedFrame, frame);
#endif

            if (kInvalidFrame == wrappedFrame) {
                ClearOutput();
                return;
            }

            Texture2D frameTex = textureSequence[wrappedFrame];
            
            if (outputToRenderTexture) {
                Graphics.Blit(frameTex, targetTexture);
            }

            if (outputToRawImage) {
                targetRawImage.texture = frameTex;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Stop() {
            Pause();

            frame = 0;
            UpdateOutput();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Play() {
            _startTime = GetTime();
            _isPlaying = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pause() {
            _isPlaying = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            if (!_isPlaying) {
                return;
            }

            frame = GetPlayFrame();
            UpdateOutput();
        }
    }
}