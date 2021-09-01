using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace FragniteGames {
    [CustomEditor(typeof(CameraMappedGrid))]
    public class CameraMappedGridEditor : Editor {
        private CameraMappedGrid _component {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return target as CameraMappedGrid;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if (_component.IsGenerated) {
                GUILayout.Label(
                    string.Format(
                        "Vertices: {0}\r\n" +
                        "Triangles: {1}\r\n" +
                        "Aspect Ratio: {2:F4}\r\n" +
                        "Bounds:\r\n" +
                        "    Size: {3}\r\n" +
                        "    Min: {4}\r\n" +
                        "    Max: {5}\r\n" +
                        "    Center: {6}\r\n" +
                        "    Extents: {7}",
                        _component.NumVertices,
                        _component.NumTriangles,
                        _component.MeshBounds.size.x / _component.MeshBounds.size.y,
                        _component.MeshBounds.size.ToString("F4"),
                        _component.MeshBounds.min.ToString("F4"),
                        _component.MeshBounds.max.ToString("F4"),
                        _component.MeshBounds.center.ToString("F4"),
                        _component.MeshBounds.extents.ToString("F4")
                    ),
                    EditorStyles.helpBox
                );
            } else {
                GUILayout.Label("<Not Generated>", EditorStyles.helpBox);
            }

            if (GUILayout.Button("Generate Grid")) {
                _component.GenerateGrid();
            }
        }
    }
}