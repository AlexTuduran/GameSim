//
// Frame Capturer
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OmniSARTechnologies.Utils {
    public enum ImageFormat {
        JPG,
        PNG,
        EXR
    }

    [AddComponentMenu("OmniSAR Technologies/Utils/Frame Capturer")]
    public class FrameCapturer : MonoBehaviour {
        [Header("Capturing")]
        public bool continuousCapture = false;
        public bool useSequentialNaming = false;
        public string sequentialNamingFormat = "frame-{0:D06}";
        public string path = "_.captures";
        public string fileTag = "final";
        [Range(1, 16)] public int superSize = 1; 
        public int frameRate = 30;
        public ImageFormat outputFormat = ImageFormat.PNG;
        public TextureFormat internalTextureFormat = TextureFormat.RGB24;
        public RenderTexture renderTextureToCaptureFrom;

        [Header("Off-Screen Capturing")]
        public bool captureOffScreen = false;
        public int offScreenWidth = 3840;
        public int offScreenHeight = 2160;

        private int m_CurrentFrame = 1;
        
        private Camera m_CurrentCamera;
        public Camera currentCamera {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (!m_CurrentCamera) {
                    m_CurrentCamera = GetComponent<Camera>();
                }

                return m_CurrentCamera;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetCapturePath() {
#if UNITY_EDITOR
            return path;
#else
            return Path.Combine(Application.persistentDataPath, path);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetCurrentFileName(int width, int height, string ext, string extendedTag) {
            string fileName = useSequentialNaming ?
                string.Format(sequentialNamingFormat + "{1}", m_CurrentFrame, ext) :
                string.Format(
                    "P[{0}]-S[{1}]{2}-[W{3}xH{4}]-[F{5:D06}]-[T{6:D12}]-[{7}]{8}",
                    Application.productName,
                    SceneManager.GetActiveScene().name,
                    fileTag.Length > 0 ? "-t[" + fileTag + "]" : "",
                    width,
                    height,
                    Time.frameCount,
                    Mathf.FloorToInt(Time.fixedUnscaledTime * 1000.0f),
                    extendedTag,
                    ext
                );

            return Path.Combine(GetCapturePath(), fileName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetCurrentFileName(int width, int height, string ext, bool offScreen) {
            return GetCurrentFileName(width, height, ext, offScreen ? "OffScreen" : "OnScreen");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ForcePath(string path) {
            if (Directory.Exists(path)) {
                return true;
            }

            return Directory.CreateDirectory(path).Exists;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool EnsureCapturePathAndReport() {
            string capturePath = GetCapturePath();

            if (!ForcePath(capturePath)) {
                Debug.LogError("Path '" + capturePath + "' does not exist and could not be created.");
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    	private void Start() {
            EnsureCapturePathAndReport();

            Time.captureFramerate = continuousCapture ? frameRate : 0;
            m_CurrentFrame = 1;
        }
    	
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ImageFormatToString(ImageFormat imageFormat) {
            return imageFormat.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ImageFormatToExtension(ImageFormat imageFormat) {
            return "." + ImageFormatToString(imageFormat).ToUpperInvariant();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SaveTexture(Texture2D texture, string fileName, ImageFormat imageFormat) {
            if (!texture) {
                return false;
            }

            if (fileName.Length < 1) {
                return false;
            }

            int maxFilex = 8;
            while (File.Exists(fileName) && (maxFilex --> 0)) {
                fileName += ImageFormatToExtension(imageFormat);
            }

            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }

            byte[] bytes = null;
            switch (imageFormat) { 
                case ImageFormat.JPG:
                    bytes = texture.EncodeToJPG();
                    break;

                case ImageFormat.PNG:
                    bytes = texture.EncodeToPNG();
                    break;

                case ImageFormat.EXR:
                    bytes = texture.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat | Texture2D.EXRFlags.CompressZIP);
                    break;
            }

            if (null == bytes) {
                return false;
            }

            System.IO.File.WriteAllBytes(fileName, bytes);

            return File.Exists(fileName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool SaveTexture(RenderTexture texture, string fileName, ImageFormat imageFormat) {
            if (!texture) {
                return false;
            }

            Texture2D texture2D = new Texture2D(texture.width, texture.height, internalTextureFormat, false);
            if (!texture2D) {
                return false;
            }

            texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture2D.Apply();

            return SaveTexture(texture2D, fileName, imageFormat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CaptureOnScreenFrame() {
            string failureMsg = "On-screen frame capture failed.";

            if (!EnsureCapturePathAndReport()) {
                Debug.Log(failureMsg);
                return false;
            }

            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

#if UNITY_EDITOR
            // because Screen.width / Screen.height return the size of the inspector window
            string[] res = UnityStats.screenRes.Split('x');
            screenWidth = int.Parse(res[0]);
            screenHeight = int.Parse(res[1]);
#endif // UNITY_EDITOR

            string fileName = GetCurrentFileName(
                screenWidth * superSize,
                screenHeight * superSize,
                ImageFormatToExtension(outputFormat),
                offScreen: false
            );

            ScreenCapture.CaptureScreenshot(fileName, superSize);

            Debug.Log("Captured on-screen frame to '" + fileName + "'.");
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CaptureOffScreenFrame(int width, int height) {
            string failureMsg = "Off-screen frame capture failed.";

            if (!EnsureCapturePathAndReport()) {
                Debug.Log(failureMsg);
                return false;
            }

            Camera cam = currentCamera;
            if (!cam) {
                Debug.Log(failureMsg);
                return false;
            }

            RenderTexture temp = cam.targetTexture;

            cam.targetTexture = RenderTexture.GetTemporary(width, height, 24);
            if (!cam.targetTexture) {
                Debug.Log(failureMsg);
                return false;
            }

            RenderTexture.active = cam.targetTexture;
            cam.Render();

            string fileName = GetCurrentFileName(
                width,
                height,
                ImageFormatToExtension(outputFormat),
                offScreen: true
            );
            
            if (!SaveTexture(cam.targetTexture, fileName, outputFormat)) {
                Debug.Log(failureMsg);
                return false;
            }

            RenderTexture.ReleaseTemporary(cam.targetTexture);
            cam.targetTexture = temp;
            RenderTexture.active = temp;

            Debug.Log("Captured off-screen frame to '" + fileName + "'.");
            return true;
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CaptureFrame(bool offScreen) {
            if (offScreen) {
                CaptureOffScreenFrame(offScreenWidth, offScreenHeight);
            } else {
                CaptureOnScreenFrame();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CaptureFrame() {
            CaptureFrame(captureOffScreen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CaptureHDRRenderTargetFrame() {
            if (!renderTextureToCaptureFrom) {
                Debug.LogWarningFormat("{0} is not assigned: Operation aborted.", nameof(renderTextureToCaptureFrom));
                return;
            }

            int width = renderTextureToCaptureFrom.width;
            int height = renderTextureToCaptureFrom.height;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBAFloat, false);

            Graphics.SetRenderTarget(renderTextureToCaptureFrom);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
            string fileName = GetCurrentFileName(
                width,
                height,
                ImageFormatToExtension(ImageFormat.EXR),
                "HDR"
            );

            File.WriteAllBytes(fileName, bytes);
            DestroyImmediate(tex);
            
            Debug.Log("Captured on-screen HDR frame to '" + fileName + "'.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CaptureFrameTagged(string fileTag) {
            this.fileTag = fileTag;
            CaptureFrame();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CaptureOffScreenFrameTagged(string fileTag) {
            this.fileTag = fileTag;
            CaptureOffScreenFrame(offScreenWidth, offScreenHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CaptureOnScreenFrameTagged(string fileTag) {
            this.fileTag = fileTag;
            CaptureOnScreenFrame();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Capture() {
            Time.captureFramerate = continuousCapture ? frameRate : 0;

            if (!continuousCapture) {
                return;
            }

            CaptureFrame();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LateUpdate() {
            Capture();

            m_CurrentFrame++;
        }
    }
}