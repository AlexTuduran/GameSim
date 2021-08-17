//
// Volume Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;

namespace OmniSARTechnologies.Helper {
    [CustomEditor(typeof(Volume))]
    public class VolumeEditor : Editor {
        private Volume _component {
            get {
                return (Volume)target;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            var textWidth = EditorGUIHelper.GUIContentHelper.GetStringWidth("W / W Diagonal", EditorStyles.label); // the largest label string in the following labes 

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            {
                
                EditorGUILayout.LabelField("X Size", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeSize.x),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Y Size", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeSize.y),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Z Size", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeSize.z),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("X / Y Area", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeXYArea),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Y / Z Area", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeYZArea),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Z / X Area", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeZXArea),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("X / Y Diagonal", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeXYDiagonal),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Y / Z Diagonal", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeYZDiagonal),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Z / X Diagonal", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeZXDiagonal),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Volume", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeVolume),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("X Center", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeCenter.x),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Y Center", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeCenter.y),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Z Center", GUILayout.Width(textWidth));
                EditorGUILayout.SelectableLabel(
                    string.Format("{0:F6}", _component.VolumeCenter.z),
                    EditorStyles.helpBox,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)
                );
            }
            EditorGUILayout.EndHorizontal();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnSceneGUI() {
            Vector3 center = (_component.minBoundaries + _component.maxBoundaries) * 0.5f;

            if (_component.showGizmoName) {
                Handles.Label(
                    _component.transform.position + Vector3.up * 2.0f,
                    string.Format("{0} ({1})", _component.name, nameof(Volume)),
                    EditorStyles.whiteLabel
                );
            }

            if (!_component.showGizmoVolume) {
                return;
            }

            if (!_component.showHandles) {
                return;
            }

            Color invertedColor = Color.white - _component.gizmoVolumeColor;
            invertedColor.a = 1.0f;

            float transformMul = _component.ignoreTransform ? 0 : 1;

            EditorGUI.BeginChangeCheck();

            float minXBoundary = _component.minBoundaries.x;
            float maxXBoundary = _component.maxBoundaries.x;
            float minYBoundary = _component.minBoundaries.y;
            float maxYBoundary = _component.maxBoundaries.y;
            float minZBoundary = _component.minBoundaries.z;
            float maxZBoundary = _component.maxBoundaries.z;

            if (_component.showSideHandles) {
                Handles.color = _component.sideHandlesInvertColor ? invertedColor : _component.gizmoVolumeColor;
                float sideHandlesCapSize = _component.handlesCapSize * _component.sideHandlesCapSizeRatio;

                {
                    minXBoundary = Handles.Slider(
                        new Vector3(_component.minBoundaries.x, center.y, center.z) + transformMul * _component.transform.position,
                        Vector3.left,
                        sideHandlesCapSize,
                        Handles.SphereHandleCap,
                        _component.handlesSnapping
                    ).x - transformMul * _component.transform.position.x;

                    maxXBoundary = Handles.Slider(
                        new Vector3(_component.maxBoundaries.x, center.y, center.z) + transformMul * _component.transform.position,
                        Vector3.right,
                        sideHandlesCapSize,
                        Handles.SphereHandleCap,
                        _component.handlesSnapping
                    ).x - transformMul * _component.transform.position.x;
                }

                {
                    minYBoundary = Handles.Slider(
                        new Vector3(center.x, _component.minBoundaries.y, center.z) + transformMul * _component.transform.position,
                        Vector3.up,
                        sideHandlesCapSize,
                        Handles.SphereHandleCap,
                        _component.handlesSnapping
                    ).y - transformMul * _component.transform.position.y;

                    maxYBoundary = Handles.Slider(
                        new Vector3(center.x, _component.maxBoundaries.y, center.z) + transformMul * _component.transform.position,
                        Vector3.down,
                        sideHandlesCapSize,
                        Handles.SphereHandleCap,
                        _component.handlesSnapping
                    ).y - transformMul * _component.transform.position.y;
                }

                {
                    minZBoundary = Handles.Slider(
                        new Vector3(center.x, center.y, _component.minBoundaries.z) + transformMul * _component.transform.position,
                        Vector3.back,
                        sideHandlesCapSize,
                        Handles.SphereHandleCap,
                        _component.handlesSnapping
                    ).z - transformMul * _component.transform.position.z;

                    maxZBoundary = Handles.Slider(
                        new Vector3(center.x, center.y, _component.maxBoundaries.z) + transformMul * _component.transform.position,
                        Vector3.forward,
                        sideHandlesCapSize,
                        Handles.SphereHandleCap,
                        _component.handlesSnapping
                    ).z - transformMul * _component.transform.position.z;
                }
            }

            if (_component.showCornerHandles) {
                Vector3 vec;

                Handles.color = _component.cornerHandlesInvertColor ? invertedColor : _component.gizmoVolumeColor;

                {
                    {
                        vec = Handles.FreeMoveHandle(
                            new Vector3(minXBoundary, minYBoundary, minZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;

                        minXBoundary = vec.x;
                        minYBoundary = vec.y;
                        minZBoundary = vec.z;

                        vec = Handles.FreeMoveHandle(
                            new Vector3(maxXBoundary, minYBoundary, minZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;
                        maxXBoundary = vec.x;
                        minYBoundary = vec.y;
                        minZBoundary = vec.z;
                    }

                    {
                        vec = Handles.FreeMoveHandle(
                            new Vector3(minXBoundary, maxYBoundary, minZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;
                        minXBoundary = vec.x;
                        maxYBoundary = vec.y;
                        minZBoundary = vec.z;

                        vec = Handles.FreeMoveHandle(
                            new Vector3(maxXBoundary, maxYBoundary, minZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;
                        maxXBoundary = vec.x;
                        maxYBoundary = vec.y;
                        minZBoundary = vec.z;
                    }
                }

                {
                    {
                        vec = Handles.FreeMoveHandle(
                            new Vector3(minXBoundary, minYBoundary, maxZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;
                        minXBoundary = vec.x;
                        minYBoundary = vec.y;
                        maxZBoundary = vec.z;

                        vec = Handles.FreeMoveHandle(
                            new Vector3(maxXBoundary, minYBoundary, maxZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;
                        maxXBoundary = vec.x;
                        minYBoundary = vec.y;
                        maxZBoundary = vec.z;
                    }

                    {
                        vec = Handles.FreeMoveHandle(
                            new Vector3(minXBoundary, maxYBoundary, maxZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;
                        minXBoundary = vec.x;
                        maxYBoundary = vec.y;
                        maxZBoundary = vec.z;

                        vec = Handles.FreeMoveHandle(
                            new Vector3(maxXBoundary, maxYBoundary, maxZBoundary) + transformMul * _component.transform.position,
                            Quaternion.identity,
                            _component.handlesCapSize,
                            Vector3.one * _component.handlesSnapping,
                            Handles.SphereHandleCap
                        ) - transformMul * _component.transform.position;
                        maxXBoundary = vec.x;
                        maxYBoundary = vec.y;
                        maxZBoundary = vec.z;
                    }
                }
            }

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(_component, "Change Volume Boundaries");
                _component.minBoundaries.x = minXBoundary;
                _component.maxBoundaries.x = maxXBoundary;
                _component.minBoundaries.y = minYBoundary;
                _component.maxBoundaries.y = maxYBoundary;
                _component.minBoundaries.z = minZBoundary;
                _component.maxBoundaries.z = maxZBoundary;
            }
        }
    }
}