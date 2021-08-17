//
// Texture I / O
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public delegate void OnUpdatedTextureDelegate(Texture2D texture);

    public enum ImageOutputFormat {
        JPG,
        PNG,
        EXR
    }

    public static class TextureIO {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SaveTextureAutoName(
            Texture2D texture,
            string filePath,
            string fileTag,
            string meaning,
            ImageOutputFormat imageOutputFormat,
            bool overwriteIfFileExists,
            out string fileName
        ) {
            fileName = "";

            if (!texture) {
                return false;
            }

            if (!FileSystemUtilities.ForcePath(filePath)) {
                Debug.LogErrorFormat("Could not save image \"{0}\": Path cannot be created (\"{1}\"): Operation aborted", texture.name, filePath);
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
                texture.name,
                meaning,
                dateTimeStr,
                ImageFormatToExtension(imageOutputFormat)
            );

            fileName = Path.Combine(filePath, fileName);

            if (!SaveTexture(texture, ref fileName, imageOutputFormat, overwriteIfFileExists)) {
                Debug.LogErrorFormat("Could not save image \"{0}\" to \"{1}\": SaveTexture() failure: Operation failed", texture.name, fileName);
                return false;
            }

            Debug.LogFormat("Saved image \"{0}\" to \"{1}\"", texture.name, fileName);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SaveTexture(
            Texture2D texture,
            ref string fileName,
            ImageOutputFormat imageOutputFormat,
            bool overwriteIfFileExists
        ) {
            if (!texture) {
                return false;
            }

            if (fileName.Length < 1) {
                return false;
            }

            if (!overwriteIfFileExists) {
                fileName += ImageFormatToExtension(imageOutputFormat);
                fileName = FileSystemUtilities.GetUniqueFileName(fileName);

                if (File.Exists(fileName)) {
                    return false;
                }
            } else {
                if (File.Exists(fileName)) {
                    File.Delete(fileName);
                }
            }

            byte[] bytes = null;
            switch (imageOutputFormat) { 
                case ImageOutputFormat.JPG:
                    bytes = texture.EncodeToJPG(100);
                    break;

                case ImageOutputFormat.PNG:
                    bytes = texture.EncodeToPNG();
                    break;

                case ImageOutputFormat.EXR:
                    bytes = texture.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
                    break;

                default:
                    break;
            }

            if (null == bytes) {
                return false;
            }

            File.WriteAllBytes(fileName, bytes);

            if (!File.Exists(fileName)) {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ImageFormatToExtension(ImageOutputFormat imageOutputFormat) {
            return "." + imageOutputFormat.ToString().ToUpperInvariant();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TextureCanSerializeToFormat(Texture2D texture, ImageOutputFormat imageOutputFormat, out ImageOutputFormat suggestedImageOutputFormat) {
            suggestedImageOutputFormat = imageOutputFormat;

            if (ImageOutputFormat.JPG == imageOutputFormat) {
                return true;
            }

            if (ImageOutputFormat.PNG == imageOutputFormat) {
                return true;
            }

            if (ImageOutputFormat.EXR == imageOutputFormat) {
                if (TextureFormat.RGB9e5Float == texture.format) {
                    return true;
                }

                if (TextureFormat.RGBAFloat == texture.format) {
                    return true;
                }

                if (TextureFormat.RGBAHalf == texture.format) {
                    return true;
                }

                suggestedImageOutputFormat = ImageOutputFormat.PNG;

                return false;
            }

            return false;
        }
    }
}
