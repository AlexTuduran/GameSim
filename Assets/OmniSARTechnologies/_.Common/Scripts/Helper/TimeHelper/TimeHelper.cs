//
// Time Helper
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Runtime.CompilerServices;

namespace OmniSARTechnologies.Helper {
    public static class TimeHelper {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string DateTimeToString(DateTime dateTime) {
            return string.Format(
                "{0}-{1}-{2} {3}:{4}:{5}.{6}",
                dateTime.Year.ToString("D4"),
                dateTime.Month.ToString("D2"),
                dateTime.Day.ToString("D2"),
                dateTime.Hour.ToString("D2"),
                dateTime.Minute.ToString("D2"),
                dateTime.Second.ToString("D2"),
                dateTime.Millisecond.ToString("D3")
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string DateTimeToStringEnh(DateTime dateTime) {
            return string.Format(
                "{0}-{1}-{2} {3}:{4}:{5}.{6} [D:{7} TK:{8}]",
                dateTime.Year.ToString("D4"),
                dateTime.Month.ToString("D2"),
                dateTime.Day.ToString("D2"),
                dateTime.Hour.ToString("D2"),
                dateTime.Minute.ToString("D2"),
                dateTime.Second.ToString("D2"),
                dateTime.Millisecond.ToString("D3"),
                dateTime.DayOfYear.ToString("D3"),
                dateTime.Ticks
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TimeSpanToString(TimeSpan timeSpan) {
            return string.Format(
                "{0} {1}:{2}:{3}.{4}",
                StringHelper.DoubleToConstantLengthAutoPrecisionString(timeSpan.TotalDays, 10),
                timeSpan.Hours.ToString("D2"),
                timeSpan.Minutes.ToString("D2"),
                timeSpan.Seconds.ToString("D2"),
                timeSpan.Milliseconds.ToString("D3")
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TimeSpanToStringEnh(TimeSpan timeSpan) {
            return string.Format(
                "{0}:{1}:{2}.{3} [TD:{4} TH:{5} TM:{6} TS:{7} TMS:{8} TTK:{9}]",
                timeSpan.Hours.ToString("D2"),
                timeSpan.Minutes.ToString("D2"),
                timeSpan.Seconds.ToString("D2"),
                timeSpan.Milliseconds.ToString("D3"),
                timeSpan.TotalDays.ToString("F8"),
                timeSpan.TotalHours.ToString("F6"),
                timeSpan.TotalMinutes.ToString("F4"),
                timeSpan.TotalSeconds.ToString("F2"),
                timeSpan.TotalMilliseconds.ToString("F0"),
                timeSpan.Ticks
            );
        }
    }
}
