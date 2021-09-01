using System.Runtime.CompilerServices;
using UnityEngine;


namespace FragniteGames {
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CameraMappedGrid : MonoBehaviour {
        public int density;
        public Texture2D depthTexture;
        public bool invert = false;
        public float minDepth = 0.0f;
        public float maxDepth = 1.0f;
        public bool cameraMapping = false;
        public Camera mappingCamera;

        private Mesh _mesh;
        private Vector3[] _vertices;
        private int _numTriangles;
        private bool _isGenerated;

        public bool IsGenerated => _isGenerated && NumVertices > 0 && NumTriangles > 0;

        public int NumVertices {
            get {
                if (null != _vertices) {
                    return _vertices.Length;
                }

                return 0;
            }
        }

        public int NumTriangles => _numTriangles;

        public Bounds MeshBounds {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (null != _mesh) {
                    return _mesh.bounds;
                }

                return default;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TraceScreenSpaceDepthTexture() {
            int width = mappingCamera.pixelWidth;
            int height = mappingCamera.pixelHeight;
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenerateGrid() {
            _isGenerated = false;
            _numTriangles = 0;

            GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
            _mesh.name = "Procedural Grid";

            _vertices = new Vector3[(density + 1) * (density + 1)];
            Vector2[] uv = new Vector2[_vertices.Length];
            Vector4[] tangents = new Vector4[_vertices.Length];
            Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
            Vector2 cellSize = new Vector2(1.0f / density, 1.0f / density);
            float ratio = depthTexture ? (float)depthTexture.width / depthTexture.height : 1.0f;

            for (int i = 0, y = 0; y <= density; y++) {
                for (int x = 0; x <= density; x++, i++) {
                    uv[i] = new Vector2(x, y) * cellSize;

                    float depth = minDepth;

                    if (depthTexture) {
                        depth = depthTexture.GetPixelBilinear(uv[i].x, uv[i].y).r;

                        if (invert) {
                            depth = 1.0f - depth;
                        }

                        depth = Mathf.Lerp(maxDepth, minDepth, depth);
                        depth = Mathf.Pow(depth, 2.0f);
                    }

                    Vector3 worldPosition = new Vector3(
                        (uv[i].x - 0.5f) * ratio,
                        uv[i].y - 0.5f,
                        depth
                    );

                    if (cameraMapping) {
                        Vector3 screenPosition = new Vector3(
                            uv[i].x,
                            uv[i].y,
                            depth
                        );

                        worldPosition = mappingCamera.ViewportToWorldPoint(screenPosition);
                    }

                    _vertices[i] = worldPosition;

                    tangents[i] = tangent;
                }
            }

            _mesh.vertices = _vertices;
            _mesh.uv = uv;
            _mesh.tangents = tangents;

            int[] triangles = new int[density * density * 6];
            for (int ti = 0, vi = 0, y = 0; y < density; y++, vi++) {
                for (int x = 0; x < density; x++, ti += 6, vi++) {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + density + 1;
                    triangles[ti + 5] = vi + density + 2;
                }
            }

            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();

            _numTriangles = triangles.Length;
            _isGenerated = true;
        }
    }
}