//
// Audio Settings Manager
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public delegate void OnAudioSettingsChanged();

    public static class AudioSettingsManager {
        public static OnAudioSettingsChanged onAudioSettingsChanged = null;

        private static int m_SampleRate = 0;
        public static int sampleRate {
            get {
                return m_SampleRate;
            }
        }

        public static int clampedSampleRate {
            get {
                return ClampSampleRate(m_SampleRate);
            }
        }

        private static int m_NumChannels = 0;
        public static int numChannels {
            get {
                return m_NumChannels;
            }
        }

        public static int clampedNumChannels {
            get {
                return ClampNumChannels(m_NumChannels);
            }
        }

        private static int m_BufferLength = 0;
        public static int bufferLength {
            get {
                return m_BufferLength;
            }
        }

        public static int clampedBufferLength {
            get {
                return ClampBufferLength(m_BufferLength);
            }
        }

        private static int m_NumBuffers = 0;
        public static int numBuffers {
            get {
                return m_NumBuffers;
            }
        }

        public static int clampedNumBuffers {
            get {
                return ClampNumBuffers(m_NumBuffers);
            }
        }

        public const int MinSampleRate = 11025;
        public const int MaxSampleRate = 96000;
        public const int MinNumChannels = 1;
        public const int MaxNumChannels = 8;
        public const int MinBuffferLength = 32;
        public const int MaxBuffferLength = 8192;
        public const int MinNumBufffers = 1;
        public const int MaxNumBufffers = 1 << 16;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AudioSpeakerModeToNumChannels(AudioSpeakerMode audioSpeakerMode) {
            switch (audioSpeakerMode) { 
                case AudioSpeakerMode.Mono          : return 1;
                case AudioSpeakerMode.Quad          : return 4;
                case AudioSpeakerMode.Surround      : return 5;
                case AudioSpeakerMode.Mode5point1   : return 6;
                case AudioSpeakerMode.Mode7point1   : return 8;
                case AudioSpeakerMode.Stereo        : // fall-through
                case AudioSpeakerMode.Prologic      : // fall-through
                default                             : return 2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AudioSpeakerModeToString(AudioSpeakerMode audioSpeakerMode) {
            switch (audioSpeakerMode) { 
                case AudioSpeakerMode.Mono          : return "Mono";
                case AudioSpeakerMode.Quad          : return "Quad";
                case AudioSpeakerMode.Surround      : return "Surround 4.1";
                case AudioSpeakerMode.Mode5point1   : return "Surround 5.1";
                case AudioSpeakerMode.Mode7point1   : return "Surround 5.1";
                case AudioSpeakerMode.Stereo        : return "Stereo";
                case AudioSpeakerMode.Prologic      : return "Surround Dolby Pro Logic";
                default                             : return "<Unsupported>";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NumChannelsToString(int numChannels) {
            switch (numChannels) { 
                case 1  : return "Mono";
                case 2  : return "Stereo";
                case 4  : return "Quad";
                case 5  : return "Surround 4.1";
                case 6  : return "Surround 5.1";
                case 8  : return "Surround 7.1";
                default : return "<Unsupported>";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnAudioConfigurationChanged(bool deviceWasChanged) {
            UpdateAudioSettings();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UpdateAudioSettings(bool logChanges = true) {
            if (!AcquireAudioSettings(logChanges)) {
                return false;
            }

            if (null != onAudioSettingsChanged) {
                onAudioSettingsChanged();
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AcquireAudioSettings(bool logChanges = true) {
            AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;

            m_SampleRate = AudioSettings.outputSampleRate;

            if (m_SampleRate < MinSampleRate) {
                return false;
            }

            if (m_SampleRate > MaxSampleRate) {
                return false;
            }

            m_NumChannels = AudioSpeakerModeToNumChannels(AudioSettings.speakerMode);

            if (m_NumChannels < MinNumChannels) {
                return false;
            }

            if (m_NumChannels > MaxNumChannels) {
                return false;
            }

            AudioSettings.GetDSPBufferSize(out m_BufferLength, out m_NumBuffers);

            if (m_BufferLength < MinBuffferLength) {
                return false;
            }

            if (m_NumChannels > MaxBuffferLength) {
                return false;
            }

            if (m_NumBuffers < MinNumBufffers) {
                return false;
            }

            if (m_NumBuffers > MaxNumBufffers) {
                return false;
            }

            if (logChanges) {
                Debug.LogFormat(
                    "Audio Settings: sampleRate={0} numChannels={1} bufferLenght={2} numBuffers={3}",
                    m_SampleRate,
                    m_NumChannels,
                    m_BufferLength,
                    m_NumBuffers
                );
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampSampleRate(int sampleRate) {
            if (sampleRate < MinSampleRate) {
                sampleRate = MinSampleRate;
            }

            if (sampleRate > MaxSampleRate) {
                sampleRate = MaxSampleRate;
            }

            return sampleRate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampNumChannels(int numChannels) {
            if (numChannels < MinNumChannels) {
                numChannels = MinNumChannels;
            }

            if (numChannels > MaxNumChannels) {
                numChannels = MaxNumChannels;
            }

            return numChannels;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampBufferLength(int bufferLength) {
            if (bufferLength < MinBuffferLength) {
                bufferLength = MinBuffferLength;
            }

            if (bufferLength > MaxBuffferLength) {
                bufferLength = MaxBuffferLength;
            }

            return bufferLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampNumBuffers(int numBuffers) {
            if (numBuffers < MinNumBufffers) {
                numBuffers = MinNumBufffers;
            }

            if (numBuffers > MaxNumBufffers) {
                numBuffers = MaxNumBufffers;
            }

            return numBuffers;
        }
    }
}
