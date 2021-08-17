//
// Image Effect
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class ImageEffect : MonoBehaviour {
        protected static readonly int kDefaultRequiredShaderLevel = 30;
        protected static readonly int kDefaultShaderInputTexturePropertyId = Shader.PropertyToID("_MainTex");

        private Shader _shader = null;
        public Shader shader {
            get {
                return _shader;
            }
        }

        private Material _material = null;
        public Material material {
            get {
                return _material;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int GetRequiredShaderLevel() {
            return kDefaultRequiredShaderLevel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual string GetShaderName() {
            return string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int GetShaderInputTexturePropertyID() {
            return kDefaultShaderInputTexturePropertyId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CleanUp() {
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CheckSystemSupport() {
            if (SystemInfo.graphicsShaderLevel < GetRequiredShaderLevel()) {
                ComponentReporter.ReportDisabledFunctionality(string.Format("Graphics shader level {0} not supported (max supported is {1})", GetRequiredShaderLevel(), SystemInfo.graphicsShaderLevel), this);
                return false;
            }

            if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                ComponentReporter.ReportDisabledFunctionality("No depth render texture format support", this);
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CreateMaterial() {
            if (_material) {
                return true;
            }

            // validate system
            if (!CheckSystemSupport()) {
                ComponentReporter.ReportDisabledFunctionality("System support check failed", this);
                return false;
            }

            //
            // find and validate shader
            //

            _shader = Shader.Find(GetShaderName());

            if (!_shader) {
                ComponentReporter.ReportDisabledFunctionality(string.Format("Shader \"{0}\" not found", GetShaderName()), this);
                return false;
            }

            if (!_shader.isSupported) {
                ComponentReporter.ReportDisabledFunctionality(string.Format("Shader \"{0}\" not supported on the current system", GetShaderName()), this);
                return false;
            }

            //
            // create and validate material
            //

            _material = new Material(_shader);

            if (!_material) {
                ComponentReporter.ReportDisabledFunctionality(string.Format("Failed to create material with shader \"{0}\": new Material() returned NULL", GetShaderName()), this);
                return false;
            }

            _material.hideFlags = HideFlags.HideAndDontSave;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool UpdateMaterial() {
            if (!CreateMaterial()) {
                ComponentReporter.ReportDisabledFunctionality("Could not create material", this);
                return false;
            }

            if (!_material) {
                ComponentReporter.ReportDisabledFunctionality("Material not created", this);
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool UpdateInternals() {
            if (!UpdateMaterial()) {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Start() {
            UpdateInternals();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnValidate() {
            UpdateInternals();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnEnable() {
            UpdateInternals();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnDisable() {
            CleanUp();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetKeyword(string keyword, bool enable) {
            if (!_material) {
                return false;
            }

            if (enable) {
                if (!_material.IsKeywordEnabled(keyword)) {
                    _material.EnableKeyword(keyword);
                }
            } else {
                if (_material.IsKeywordEnabled(keyword)) {
                    _material.DisableKeyword(keyword);
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitWithMaterial(RenderTexture src, RenderTexture dst, Material mat, int pass) {
            mat.SetTexture(GetShaderInputTexturePropertyID(), src);
            mat.SetPass(pass);

            Graphics.Blit(src, dst, mat, pass);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitWithMaterial(RenderTexture src, RenderTexture dst, int pass) {
            BlitWithMaterial(src, dst, _material, pass);
        }
    }
}
