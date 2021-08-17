//
// File System Utilities
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace OmniSARTechnologies.Helper {
    public static class FileSystemUtilities {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertPathTotAltSeparator(string path) {
            return path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetRelativePath(string absolutePath) {
            if (absolutePath.StartsWith(Application.dataPath)) {
                absolutePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }

            return absolutePath;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveFileExtension(string fileName) {
            fileName = Path.ChangeExtension(fileName, "");
            return fileName.Remove(fileName.Length - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetAppendedFileName(string fileName, string postFix) {
            return Path.ChangeExtension(RemoveFileExtension(fileName) + postFix, Path.GetExtension(fileName));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetUniqueFileName(string fileName, int maxNumRetries = 32) {
            while (File.Exists(fileName) && maxNumRetries --> 0) {
                fileName = GetAppendedFileName(fileName, "~");
            }

            return fileName;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ForcePath(string path) {
            if (Directory.Exists(path)) {
                return true;
            }

            DirectoryInfo info = Directory.CreateDirectory(path);
            if (null == info) {
                return false;
            }

            return info.Exists;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FileExistsInProjectAssets(string fileName) {
#if UNITY_EDITOR
            if (fileName.Length < 1) {
                return false;
            }

            string[] allPaths = AssetDatabase.GetAllAssetPaths();
            if (allPaths.Length < 1) {
                return false;
            }

            fileName = ConvertPathTotAltSeparator(fileName);
            string absolutePath;
            for (int i = 0; i < allPaths.Length; i++) {
                absolutePath = Path.GetFullPath(allPaths[i]);
                absolutePath = ConvertPathTotAltSeparator(absolutePath);
                if (absolutePath.Equals(fileName)) {
                    return true;
                }
            }
#endif // UNITY_EDITOR

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T LoadAssetInteractive<T>(
            string filePath,
            string subFolder,
            bool overwriteIfFileExists,
            string openPanelTitle,
            string[] filters,
            out bool canceled
        ) where T : UnityEngine.Object {
#if UNITY_EDITOR
            canceled = false;
            string importedFilePath = Path.Combine(filePath, subFolder + Path.AltDirectorySeparatorChar);
            string fileName = EditorUtility.OpenFilePanelWithFilters(openPanelTitle, filePath, filters);
            if (fileName.Length > 0) {
                fileName = FileSystemUtilities.ConvertPathTotAltSeparator(fileName);
                if (!FileSystemUtilities.FileExistsInProjectAssets(fileName)) {
                    if (
                        EditorUtility.DisplayDialog(
                            "File outside of your project's structure",
                            string.Format(
                                "File \"{0}\" is not a project asset.\n\r\n\r" +
                                "The file must be in your project's asset structure in order to be used as an asset.\n\r\n\r" +
                                "Do you want to import it in \"{1}\"?",
                                fileName,
                                importedFilePath
                            ),
                            "Import",
                            "Cancel"
                        )) {

                        string fileNameOnly = Path.GetFileName(fileName);
                        EditorUtility.DisplayProgressBar("Importing", string.Format("Importing \"{0}\" to \"{1}\"...", fileNameOnly, importedFilePath), 1.0f);
                        string dstFileName = Path.Combine(importedFilePath, fileNameOnly);
                        dstFileName = GetUniqueFileName(dstFileName);

                        if (File.Exists(dstFileName)) {
                            if (overwriteIfFileExists) {
                                File.Delete(dstFileName);
                            } else {
                                EditorUtility.ClearProgressBar();
                                return default(T);
                            }
                        }

                        if (FileSystemUtilities.ForcePath(importedFilePath)) {
                            File.Copy(fileName, dstFileName);
                            AssetDatabase.Refresh();
                            Debug.LogFormat("File \"{0}\" imported to \"{1}\"", fileName, importedFilePath);
                        } else {
                            Debug.LogErrorFormat("Could not create path \"{0}\": Importing failed: Operation aborted", importedFilePath);
                        }

                        EditorUtility.ClearProgressBar();
                        fileName = dstFileName;
                    } else {
                        Debug.LogFormat("File \"{0}\" NOT imported: Operation aborted", fileName);
                        fileName = default(string);
                    }
                }

                if (!File.Exists(fileName)) {
                    Debug.LogErrorFormat("File \"{0}\" does not exist: Operation aborted", fileName);
                    return default(T);
                }

                fileName = FileSystemUtilities.GetRelativePath(fileName);
                if (!File.Exists(fileName)) {
                    Debug.LogErrorFormat("File \"{0}\" does not exist: Operation aborted", fileName);
                    return default(T);
                }

                T asset = (T)AssetDatabase.LoadAssetAtPath(fileName, typeof(T));
                if (!asset) {
                    Debug.LogErrorFormat("Asset \"{0}\" could not be loaded as {1}: Operation aborted", fileName, typeof(T).Name);
                    return default(T);
                }

                return asset;
            } else {
                canceled = true;
            }
            
            return default;

#else // UNITY_EDITOR

            canceled = false;
            return default;

#endif // UNITY_EDITOR
        }
    }
}
