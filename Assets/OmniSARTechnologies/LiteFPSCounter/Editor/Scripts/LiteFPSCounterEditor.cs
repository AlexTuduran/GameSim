//
// Lite FPS Counter Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
//

// sync with LiteFPSCounter.cs
#define __APPLY_SETTINGS_ON_VALIDATE__

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using OmniSARTechnologies.Helper;

namespace OmniSARTechnologies.LiteFPSCounter {
    [CustomEditor(typeof(LiteFPSCounter))]
    [CanEditMultipleObjects]
    public class LiteFPSCounterEditor : Editor {
        private LiteFPSCounter m_Component {
            get {
                return target as LiteFPSCounter;
            }
        }

        private const string PackageName = "Lite FPS Counter";
        private const string GameObjectMenuRoot = "GameObject/OmniSAR Technologies/" + PackageName + "/";
        private const string HeaderGraphicPath = "Assets/OmniSARTechnologies/LiteFPSCounter/Editor/Resources/UI/Header/lite-fps-counter-header.png";
        private const float HeaderGraphicTileWidth = 5.0f;

        private SerializedProperty m_DynamicInfoText = null;
        private SerializedProperty m_DynamicInfoTextVisible = null;
        private SerializedProperty m_FPSFieldsColor = null;
        private SerializedProperty m_FPSMinFieldsColor = null;
        private SerializedProperty m_FPSMaxFieldsColor = null;
        private SerializedProperty m_FPSFluctuationFieldsColor = null;

        private SerializedProperty m_StaticInfoText = null;
        private SerializedProperty m_StaticInfoTextVisible = null;
        private SerializedProperty m_GPUFieldsColor = null;
        private SerializedProperty m_GPUDetailFieldsColor = null;
        private SerializedProperty m_CPUFieldsColor = null;
        private SerializedProperty m_SysFieldsColor = null;

        private SerializedProperty m_TextPosition = null;
        private SerializedProperty m_TextSize = null;
        private SerializedProperty m_TextEnhancement = null;
        private SerializedProperty m_UpdateInterval = null;

        private bool m_GUIOptionsFolded = true;
        private bool m_BehaviourOptionsFolded = true;

        private static Texture m_HeaderTex = null;
        private static Rect m_HeaderTileTexCoords;

        private void OnEnable() {
            m_DynamicInfoText = serializedObject.FindProperty("dynamicInfoText");
            m_DynamicInfoTextVisible = serializedObject.FindProperty("dynamicInfoTextVisible");
            m_FPSFieldsColor = serializedObject.FindProperty("fpsFieldsColor");
            m_FPSMinFieldsColor = serializedObject.FindProperty("fpsMinFieldsColor");
            m_FPSMaxFieldsColor = serializedObject.FindProperty("fpsMaxFieldsColor");
            m_FPSFluctuationFieldsColor = serializedObject.FindProperty("fpsFluctuationFieldsColor");

            m_StaticInfoText = serializedObject.FindProperty("staticInfoText");
            m_StaticInfoTextVisible = serializedObject.FindProperty("staticInfoTextVisible");
            m_GPUFieldsColor = serializedObject.FindProperty("gpuFieldsColor");
            m_GPUDetailFieldsColor = serializedObject.FindProperty("gpuDetailFieldsColor");
            m_CPUFieldsColor = serializedObject.FindProperty("cpuFieldsColor");
            m_SysFieldsColor = serializedObject.FindProperty("sysFieldsColor");

            m_TextPosition = serializedObject.FindProperty("textPosition");
            m_TextSize = serializedObject.FindProperty("textSize");
            m_TextEnhancement = serializedObject.FindProperty("textEnhancement");
            m_UpdateInterval = serializedObject.FindProperty("updateInterval");

            m_HeaderTex = (Texture2D)AssetDatabase.LoadMainAssetAtPath(HeaderGraphicPath);
            if (null == m_HeaderTex) {
                return;
            }

            m_HeaderTileTexCoords = new Rect(0, 0, HeaderGraphicTileWidth / (m_HeaderTex.width - 1), 1);
        }

        public static void DrawComponentHeader() {
            if (!m_HeaderTex) {
                return;
            }

            GUILayout.BeginHorizontal(); {
                Rect drawingAreaRect = EditorGUILayout.GetControlRect(GUILayout.MaxHeight(34));

                Rect headerRect = new Rect(
                    drawingAreaRect.xMin - 13,
                    drawingAreaRect.y + 2,
                    drawingAreaRect.xMax + 3,
                    m_HeaderTex.height
                );

                Rect headerImageRect = new Rect(headerRect) { width = m_HeaderTex.width };
                headerImageRect.width = m_HeaderTex.width;
                GUI.DrawTexture(headerImageRect, m_HeaderTex);

                Rect headerImageTileRect = new Rect(headerRect);
                headerImageTileRect.x = m_HeaderTex.width + 1;
                headerImageTileRect.width = headerRect.width - headerImageRect.width;
                GUI.DrawTextureWithTexCoords(headerImageTileRect, m_HeaderTex, m_HeaderTileTexCoords);
            } GUILayout.EndHorizontal();
        }

        private void DrawUIOptions<T>(SerializedProperty headerProperty) {
            HeaderAttribute header = EditorGUIHelper.Attributes.GetSerializedPropertyFirstAttribute<T, HeaderAttribute>(headerProperty);
            if (null != header) {
                m_GUIOptionsFolded = EditorGUILayout.Foldout(
                    m_GUIOptionsFolded,
                    header.header,
                    m_GUIOptionsFolded ? EditorGUIHelper.Styles.boldFoldout : EditorStyles.foldout
                );

                if (!m_GUIOptionsFolded) {
                    return;
                }
            }

            EditorGUIHelper.Drawing.DrawMultiValueObjectField<T>(m_DynamicInfoText);

            if (m_Component.dynamicInfoText) {
                EditorGUI.indentLevel++;
                EditorGUIHelper.Drawing.DrawMultiValueToggle<T>(m_DynamicInfoTextVisible, true);
                if (m_DynamicInfoTextVisible.boolValue) {
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_FPSFieldsColor);
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_FPSMinFieldsColor);
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_FPSMaxFieldsColor);
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_FPSFluctuationFieldsColor);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Separator();
            }

            EditorGUIHelper.Drawing.DrawMultiValueObjectField<T>(m_StaticInfoText);

            if (m_Component.staticInfoText) {
                EditorGUI.indentLevel++;
                EditorGUIHelper.Drawing.DrawMultiValueToggle<T>(m_StaticInfoTextVisible, true);
                if (m_StaticInfoTextVisible.boolValue) {
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_GPUFieldsColor);
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_GPUDetailFieldsColor);
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_CPUFieldsColor);
                    EditorGUIHelper.Drawing.DrawMultiValueColorField<T>(m_SysFieldsColor);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Separator();
            }

            EditorGUIHelper.Drawing.DrawMultiValueEnumPopup<T>(m_TextPosition);
            EditorGUIHelper.Drawing.DrawMultiValueEnumPopup<T>(m_TextSize);
            EditorGUIHelper.Drawing.DrawMultiValueEnumPopup<T>(m_TextEnhancement);

#if !__APPLY_SETTINGS_ON_VALIDATE__
            GUILayout.BeginHorizontal(); {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Apply")) {
                    m_Component.ApplySettings();
                }
            } GUILayout.EndHorizontal();
#endif

            if (null != header) {
                EditorGUILayout.Separator();
            }
        }
        
        private void DrawBehaviourOptions<T>(SerializedProperty headerProperty) {
            HeaderAttribute header = EditorGUIHelper.Attributes.GetSerializedPropertyFirstAttribute<T, HeaderAttribute>(headerProperty);
            if (null != header) {
                m_BehaviourOptionsFolded = EditorGUILayout.Foldout(
                    m_BehaviourOptionsFolded,
                    header.header,
                    m_BehaviourOptionsFolded ? EditorGUIHelper.Styles.boldFoldout : EditorStyles.foldout
                );

                if (!m_BehaviourOptionsFolded) {
                    return;
                }
            }

            EditorGUIHelper.Drawing.DrawMultiValueSlider<T>(m_UpdateInterval);

            if (null != header) {
                EditorGUILayout.Separator();
            }
        }

        public override void OnInspectorGUI() {
            DrawComponentHeader();
            DrawUIOptions<LiteFPSCounter>(m_DynamicInfoText);
            if (m_Component.dynamicInfoText) {
                DrawBehaviourOptions<LiteFPSCounter>(m_UpdateInterval);
            }
        }

        private static bool CreateGameObject(string prefabName, string commandName, string packageName) {
            string[] assets = AssetDatabase.FindAssets(prefabName + " t:Prefab");

            if (null == assets) {
                Debug.LogWarning(
                    ColorHelper.ColorText(
                        string.Format(
                            "Could not create {0}: " +
                            "Prefab \"{1}\" could not be found in the project: " +
                            "Please re-install the {2} package and try again",
                            commandName,
                            prefabName,
                            packageName
                        ),
                        Color.red
                    )
                );
                return false;
            }

            if (assets.Length < 1) {
                Debug.LogWarning(
                    ColorHelper.ColorText(
                        string.Format(
                            "Could not create {0}: " +
                            "Prefab \"{1}\" could not be found in the project: " +
                            "Please re-install the {2} package and try again",
                            commandName,
                            prefabName,
                            packageName
                        ),
                        Color.red
                    )
                );
                return false;
            }

            string prefabPath = AssetDatabase.GUIDToAssetPath(assets[0]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (!prefab) {
                Debug.LogWarning(
                    ColorHelper.ColorText(
                        string.Format(
                            "Could not create {0}: " +
                            "Prefab \"{1}\" could not be found in the project: " +
                            "Please re-install the {2} package and try again",
                            commandName,
                            prefabName,
                            packageName
                        ),
                        Color.red
                    )
                );
                return false;
            }

            GameObject go = Instantiate(prefab, Selection.activeGameObject ? Selection.activeGameObject.transform : null);
            go.name = prefabName;

            if (!go) {
                Debug.LogWarning(
                    ColorHelper.ColorText(
                        string.Format(
                            "Could not create {0}: " +
                            "Prefab \"{1}\" coult not be instantiated: " +
                            "Instantiate() returned NULL: " +
                            "Please manually add the prefab into the scene: " +
                            "Path: \"{2}\"",
                            commandName,
                            prefabName,
                            prefabPath
                        ),
                        Color.red
                    )
                );
                return false;
            }

            Selection.activeGameObject = go;
            Debug.Log(
                ColorHelper.ColorText(
                    string.Format(
                        "Game Object \"{0}\" added to scene (based on the \"{1}\" prefab)",
                        go.name,
                        prefabPath
                    ),
                    ColorHelper.HexStrToColor("#0C4366")
                )
            );
            return true;
        }

        [MenuItem(GameObjectMenuRoot + "Lite FPS Counter", priority = 10)] 
        private static bool CreateLiteFPSCounterGameObject() {
            return CreateGameObject(
                "LiteFPSCounter",
                "Lite FPS Counter",
                PackageName
            );
        }
    }
}