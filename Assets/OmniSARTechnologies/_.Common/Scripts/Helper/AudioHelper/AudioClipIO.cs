//
// AudioClip I / O
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.IO;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public static class AudioClipIO {
        public static bool SaveAudioClipAutoName(
            AudioClip audioClip,
            string filePath,
            string fileTag,
            string meaning,
            bool overwriteIfFileExists,
            out string fileName
        ) {
            fileName = "";

            if (!audioClip) {
                return false;
            }

            if (!FileSystemUtilities.ForcePath(filePath)) {
                Debug.LogErrorFormat("Could not save audio clip \"{0}\": Path cannot be created (\"{1}\"): Operation aborted", audioClip.name, filePath);
                return false;
            }

            System.DateTime dateTime = System.DateTime.Now;
            string dateTimeStr = string.Format(
                "[{0}{1}{2}]-[{3}{4}{5}{6}]",
                dateTime.Year.ToString("D4"),
                dateTime.Month.ToString("D2"),
                dateTime.Day.ToString("D2"),
                dateTime.Hour.ToString("D2"),
                dateTime.Minute.ToString("D2"),
                dateTime.Second.ToString("D2"),
                dateTime.Millisecond.ToString("D3")
            );

            fileName = string.Format(
                "{0}[{1}]-{2}-{3}{4}",
                fileTag.Length > 0 ? fileTag + "-" : "",
                audioClip.name,
                meaning,
                dateTimeStr,
                ".wav"
            );

            fileName = Path.Combine(filePath, fileName);

            if (!SaveAudioClip(audioClip, ref fileName, overwriteIfFileExists)) {
                Debug.LogErrorFormat("Could not save audio clip \"{0}\" to \"{1}\": SaveAudioClip() failure: Operation failed", audioClip.name, fileName);
                return false;
            }

            Debug.LogFormat("Saved image \"{0}\" to \"{1}\"", audioClip.name, fileName);
            return true;
        }

        private static bool SaveAudioClip(
            AudioClip audioClip,
            ref string fileName,
            bool overwriteIfFileExists
        ) {
            if (!audioClip) {
                return false;
            }

            if (audioClip.samples < 1) {
                return false;
            }

            if (audioClip.channels < 1) {
                return false;
            }

            if (audioClip.frequency < 1) {
                return false;
            }

            if (!overwriteIfFileExists) {
                fileName = FileSystemUtilities.GetUniqueFileName(fileName);

                if (File.Exists(fileName)) {
                    return false;
                }
            } else {
                if (File.Exists(fileName)) {
                    File.Delete(fileName);
                }
            }

            if (!SavWav.Save(fileName, audioClip)) {
                return false;
            }

            if (!File.Exists(fileName)) {
                return false;
            }

            return true;
        }
    }
}
