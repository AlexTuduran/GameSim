//
// Math Helper
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public static class MathHelper {
        public static readonly float kInvertedEpsilon = 1.0f / float.Epsilon;
        public static readonly double kDoubleInvertedEpsilon = 1.0 / double.Epsilon;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double a, double b, double t) {
            return a + t * (b - a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LinearToExponential(float value, float exponentiality) {
            if (Mathf.Abs(exponentiality - 1.0f) <= float.Epsilon) {
                return value;
            }

            return (Mathf.Pow(exponentiality, value) - 1.0f) / (exponentiality - 1.0f);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double LinearToExponential(double value, double exponentiality) {
            if (Math.Abs(exponentiality - 1.0) <= double.Epsilon) {
                return value;
            }

            return (Math.Pow(exponentiality, value) - 1.0) / (exponentiality - 1.0);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeInvert(float value) {
            if (value < float.Epsilon) {
                return kInvertedEpsilon;
            }

            return 1.0f / value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SafeInvert(double value) {
            if (value < double.Epsilon) {
                return kDoubleInvertedEpsilon;
            }

            return 1.0 / value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceToUnitInverse(float distance) {
            return 1.0f / (1.0f + Mathf.Abs(distance));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceToUnitInverse(double distance) {
            return 1.0 / (1.0 + Math.Abs(distance));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float QuadraticDistanceToUnitInverse(float distance) {
            return 1.0f / (1.0f + distance * distance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double QuadraticDistanceToUnitInverse(double distance) {
            return 1.0 / (1.0 + distance * distance);
        }
    }
}