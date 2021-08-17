//
// HighPerformanceTime
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OmniSARTechnologies.Helper {
    public class HighPerformanceTime {
        private static bool m_Initialized = false;
        private static long m_Frequency = 0;
        private static long m_Counter = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Init() {
            if (m_Initialized) {
                return true;
            }

            if (WinAPI.QueryPerformanceFrequency(out m_Frequency)) {
                m_Initialized = true;
                return true;
            }

            return false;
        }

        public static double time {
            get {
                if (0 == m_Frequency) {
                    return 0.0;
                }

                if (!WinAPI.QueryPerformanceCounter(out m_Counter)) {
                    return 0.0;
                }

                return (double)m_Counter / m_Frequency;
            }
        }

        internal class WinAPI {
            private const string Kernel32Lib = "Kernel32.dll";

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DllImport(Kernel32Lib)]
            public static extern bool QueryPerformanceCounter(out long counter);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DllImport(Kernel32Lib)]
            public static extern bool QueryPerformanceFrequency(out long frequency);
        }
    }
}