//
// Camera Frustum Image Effect
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class CameraFrustumImageEffect : CameraImageEffect {
        private static class ShaderPropertyToID {
            public static readonly int kViewWorldPosition = Shader.PropertyToID("_ViewWorldPosition");
            public static readonly int kViewToWorldMatrix = Shader.PropertyToID("_ViewToWorldMatrix");
            public static readonly int kWorldToViewMatrix = Shader.PropertyToID("_WorldToViewMatrix");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool UpdateMaterial() {
            if (!base.UpdateMaterial()) {
                return false;
            }

            if (!material) {
                return false;
            }

            // update material's camera related properties
            material.SetVector(ShaderPropertyToID.kViewWorldPosition, currentCamera.transform.position);
            material.SetMatrix(ShaderPropertyToID.kViewToWorldMatrix, currentCamera.cameraToWorldMatrix);
            material.SetMatrix(ShaderPropertyToID.kWorldToViewMatrix, currentCamera.worldToCameraMatrix);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetCameraFrustumCorners(
            Camera camera,
            out Vector3 leftTop,
            out Vector3 rightTop,
            out Vector3 rightBottom,
            out Vector3 leftBottom
        ) {
            if (!camera) {
                leftTop     = Vector3.zero;
                rightTop    = Vector3.zero;
                rightBottom = Vector3.zero;
                leftBottom  = Vector3.zero;
                return false;
            }

            float camFov = camera.fieldOfView;
            float camAspect = camera.aspect;
            float tanFov = Mathf.Tan(0.5f * camFov * Mathf.Deg2Rad);

            Vector3 toRight = camAspect * tanFov * Vector3.right;
            Vector3 toTop   = tanFov * Vector3.up;

            leftTop     = -Vector3.forward - toRight + toTop;
            rightTop    = -Vector3.forward + toRight + toTop;
            rightBottom = -Vector3.forward + toRight - toTop;
            leftBottom  = -Vector3.forward - toRight - toTop;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 GetCameraFrustumCorners(Camera camera) {
            Matrix4x4 frustumCorners = Matrix4x4.identity;

            if (!camera) {
                return frustumCorners;
            }

            if (!GetCameraFrustumCorners(
                camera,
                out var leftTop,
                out var rightTop,
                out var rightBottom,
                out var leftBottom
            )) {
                frustumCorners.SetRow(0, leftTop);
                frustumCorners.SetRow(1, rightTop);
                frustumCorners.SetRow(2, rightBottom);
                frustumCorners.SetRow(3, leftBottom);
            }

            return frustumCorners;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4 GetCameraFrustumCorners() {
            return GetCameraFrustumCorners(currentCamera);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mesh CreateQuad(
            Vector3 leftTop,
            Vector3 rightTop,
            Vector3 rightBottom,
            Vector3 leftBottom
        ) {
            Vector3[] vertices = new Vector3[4] {
                leftTop,     // 0 - LT
                rightTop,    // 1 - RT
                rightBottom, // 2 - RB
                leftBottom   // 3 - LB
            };

            int[] indices = new int[6] {
                0, 1, 2, // LT, RT, RB
                2, 3, 0  // RB, LB, LT
            };

            Vector2[] uvs = new Vector2[4] {
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f)
            };

            Mesh quad = new Mesh {
                vertices = vertices,
                triangles = indices,
                uv = uvs
            };

            return quad;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mesh CreateViewQuad(float scale = 1.0f) {
            return CreateQuad(
                new Vector3(-scale, -scale, scale),
                new Vector3(+scale, -scale, scale),
                new Vector3(+scale, +scale, scale),
                new Vector3(-scale, +scale, scale)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mesh CreateFrustumQuad(Camera camera, float scale) {
            if (!camera) {
                return null;
            }

            if (!GetCameraFrustumCorners(
                camera,
                out var leftTop,
                out var rightTop,
                out var rightBottom,
                out var leftBottom
            )) {
                return null;
            }

            return CreateQuad(
                leftTop     * scale,
                rightTop    * scale,
                rightBottom * scale,
                leftBottom  * scale
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitFrustumQuadWithMaterial(
            RenderTexture src,
            RenderTexture dst,
            Material mat,
            int pass,
            float z0 = 1.0f,
            float z1 = 1.0f,
            float z2 = 1.0f,
            float z3 = 1.0f
        ) {
            RenderTexture.active = dst;

            mat.SetTexture(GetShaderInputTexturePropertyID(), src);
            mat.SetPass(pass);

            GL.PushMatrix();
            GL.LoadOrtho();

            GL.Begin(GL.QUADS);

            GL.MultiTexCoord2(0, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, z0);

            GL.MultiTexCoord2(0, 1.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, z1);

            GL.MultiTexCoord2(0, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, z2);

            GL.MultiTexCoord2(0, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, z3);

            GL.End();
            GL.PopMatrix();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitFrustumQuadWithMaterial(RenderTexture src, RenderTexture dst, Material mat, int pass) {
            BlitFrustumQuadWithMaterial(src, dst, mat, pass, 1.0f, 1.0f, 1.0f, 1.0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitFrustumQuadWithMaterial(RenderTexture src, RenderTexture dst, int pass) {
            BlitFrustumQuadWithMaterial(src, dst, material, pass);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitZEncodedFrustumQuadWithMaterial(RenderTexture src, RenderTexture dst, Material mat, int pass) {
            BlitFrustumQuadWithMaterial(src, dst, mat, pass, 3.0f, 2.0f, 1.0f, 0.0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitZEncodedFrustumQuadWithMaterial(RenderTexture src, RenderTexture dst, int pass) {
            BlitZEncodedFrustumQuadWithMaterial(src, dst, material, pass);
        }
    }
}
