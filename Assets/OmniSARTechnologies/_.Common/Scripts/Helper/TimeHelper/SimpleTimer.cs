//
// SimpleTimer
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class SimpleTimer {
        private float m_StartTime = 0;

        public float timeSinceStart {
            get {
                return Time.realtimeSinceStartup - m_StartTime;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SimpleTimer() {
            Restart();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Restart() {
            m_StartTime = Time.realtimeSinceStartup;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasReachedDuration(float duration) {
            return timeSinceStart > duration;
        }
    }
}
