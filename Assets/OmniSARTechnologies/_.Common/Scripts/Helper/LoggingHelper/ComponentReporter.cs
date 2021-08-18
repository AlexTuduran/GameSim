//
// Component Reporter
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public static class ComponentReporter {
        public static void Report(string text, Color color = default(Color), bool warning = false, MonoBehaviour monoBehaviour = null) {
            string reportText = ColorHelper.ColorText(text, color);

            if (monoBehaviour) {
                reportText = string.Format("{0} ({1}): {2}", monoBehaviour.GetType().Name, monoBehaviour.name, reportText);
            }

            if (warning) {
                Debug.LogWarning(reportText);
            } else {
                Debug.Log(reportText);
            }
        }

        public static void Report(string text, Color color = default(Color), MonoBehaviour monoBehaviour = null) {
            Report(text, color, false, monoBehaviour);
        }

        public static void ReportWarning(string text, Color color = default(Color), MonoBehaviour monoBehaviour = null) {
            Report(text, color, true, monoBehaviour);
        }

        public static void Report(string text, MonoBehaviour monoBehaviour = null) {
            Report(text, default(Color), false, monoBehaviour);
        }

        public static void ReportWarning(string text, MonoBehaviour monoBehaviour = null) {
            Report(text, default(Color), true, monoBehaviour);
        }

        public static void ReportDisabledFunctionality(string reason, MonoBehaviour monoBehaviour = null) {
            ReportWarning(reason + ": Component functionality DISABLED", monoBehaviour: monoBehaviour);
        }
    }
}
