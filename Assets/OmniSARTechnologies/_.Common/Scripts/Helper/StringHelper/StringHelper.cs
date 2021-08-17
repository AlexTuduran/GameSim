//
// String Helper
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public static class StringHelper {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RoundBracketedText(string text) {
            return "(" + text + ")";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CurlyBracketedText(string text) {
            return "{" + text + "}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string SquareBracketedText(string text) {
            return "[" + text + "]";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AngleBracketedText(string text) {
            return "<" + text + ">";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetPluralPostFix(int value, string singular = "", string plural = "s") {
            return value == 1 ? singular : plural;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string DoubleToConstantLengthAutoPrecisionString(double value, int length) {
            // early exit if smaller or equal to 0
            // sorry, we don't support negative numbers
            if (value <= 0) {
                return "".PadLeft(length, '0');
            }

            int numIntDigits = (int)Math.Floor(Math.Log10(value) + 1);

            // log10 can give negative numbers
            // in that case it simply means that the number is smaller than 1
            // and we can consider we have no integer digits at all
            if (numIntDigits < 0) {
                numIntDigits = 0;
            }

            // if the resulting int contains more digits than the requested
            // fixed length, convert to exponential form and add required
            // leading '0's to achieve the required length
            // we use "g" + (length - 6) to force the number before "e+XXX"
            // to be of required length minus 6 chars max
            // the subtracted 6 chars represent the exponent term "0...e+XXX"
            // we can rely on constant 6 here because the "XXX" part of the
            // exponent "0...e+XXX" has either 2 or 3 digits
            if (numIntDigits > length) {
                length = Mathf.Max(length, 6);
                return value.ToString("g" + (length - 6)).PadLeft(length, '0');
            }

            // if num int digits are equal or bigger than length - 1, just
            // return the integer part as string, as the number should not
            // contain any float separator char
            if (numIntDigits >= length - 1) {
                return ((long)(Math.Floor(value))).ToString("D" + length);
            }

            // if num int digits is 0, it means that we have a value < 1.0
            // in this case, we just return the value as string with
            // a length of (length - 2), where 2 represents the leading
            // "0." part of the float
            if (numIntDigits == 0) {
                return value.ToString("F" + (length - 2));
            }

            int numFloatDigits = length - 1 - numIntDigits; // 1 = '.
            return string.Format("{0}", value.ToString("F" + numFloatDigits));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ArrayMergeToString<T>(T[] array, string separator = ", ") {
            if (array == null) {
                return "<NULL>";
            }

            if (array.Length < 1) {
                return "<EMPTY>";
            }

            string result = "";

            for (int i = 0; i < array.Length; i++) {
                result += array[i].ToString();

                if (i < array.Length - 1) {
                    result += separator;
                }
            }

            return result;
        }
    }
}