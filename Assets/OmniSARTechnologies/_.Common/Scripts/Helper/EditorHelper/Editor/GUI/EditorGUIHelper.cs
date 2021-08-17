//
// Editor GUI Helper
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace OmniSARTechnologies.Helper {
    public static class EditorGUIHelper {
        public static class Attributes {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static PropertyAttribute[] GetFieldAttributes<PropertyAttribute>(FieldInfo field, bool inherit = true) {
                if (null == field) {
                    return default(PropertyAttribute[]);
                }

                return field.GetCustomAttributes(typeof(PropertyAttribute), inherit) as PropertyAttribute[];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static PropertyAttribute GetFieldFirstAttribute<PropertyAttribute>(FieldInfo field, bool inherit = true) {
                if (null == field) {
                    return default(PropertyAttribute);
                }

                PropertyAttribute[] attributes = GetFieldAttributes<PropertyAttribute>(field, inherit);

                if (attributes.Length < 1) {
                    return default(PropertyAttribute);
                }

                return attributes[0];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static PropertyAttribute[] GetSerializedPropertyAttributes<Type, PropertyAttribute>(SerializedProperty serializedProperty, bool inherit = true) {
                if (null == serializedProperty) {
                    return default(PropertyAttribute[]);
                }

                FieldInfo field = typeof(Type).GetField(serializedProperty.name);

                if (null == field) {
                    return default(PropertyAttribute[]);
                }

                return GetFieldAttributes<PropertyAttribute>(field, inherit);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static PropertyAttribute GetSerializedPropertyFirstAttribute<Type, PropertyAttribute>(SerializedProperty serializedProperty, bool inherit = true) {
                if (null == serializedProperty) {
                    return default(PropertyAttribute);
                }

                PropertyAttribute[] attributes = GetSerializedPropertyAttributes<Type, PropertyAttribute>(serializedProperty, inherit);

                if (default(PropertyAttribute[]) == attributes) { 
                    return default(PropertyAttribute);
                }
                    

                if (attributes.Length < 1) {
                    return default(PropertyAttribute);
                }

                return attributes[0];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string GetFieldDisplayName(FieldInfo field, bool inherit = true) {
                if (null == field) {
                    return default(string);
                }

                DisplayNameAttribute attribute = GetFieldFirstAttribute<DisplayNameAttribute>(field, inherit);

                if (null == attribute) {
                    return default(string);
                }

                return attribute.displayName;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string GetSerializedPropertyDisplayName<Type>(SerializedProperty serializedProperty, bool inherit = true) {
                if (null == serializedProperty) {
                    return default(string);
                }

                FieldInfo field = typeof(Type).GetField(serializedProperty.name);

                if (null == field) {
                    return default(string);
                }

                return GetFieldDisplayName(field, inherit);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string GetFieldTooltip(FieldInfo field, bool inherit = true) {
                if (null == field) {
                    return default(string);
                }

                TooltipAttribute attribute = GetFieldFirstAttribute<TooltipAttribute>(field, inherit);

                if (null == attribute) {
                    return default(string);
                }

                return attribute.tooltip;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string GetSerializedPropertyTooltip<Type>(SerializedProperty serializedProperty, bool inherit = true) {
                if (null == serializedProperty) {
                    return default(string);
                }

                FieldInfo field = typeof(Type).GetField(serializedProperty.name);

                if (null == field) {
                    return default(string);
                }

                return GetFieldTooltip(field, inherit);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector2 GetFieldFloatRange(FieldInfo field, bool inherit = true) {
                if (null == field) {
                    return default(Vector2);
                }

                RangeAttribute attribute = GetFieldFirstAttribute<RangeAttribute>(field, inherit);

                if (null == attribute) {
                    return default(Vector2);
                }

                return new Vector2(attribute.min, attribute.max);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector2 GetSerializedPropertyFloatRange<Type>(SerializedProperty serializedProperty, bool inherit = true) {
                if (null == serializedProperty) {
                    return default(Vector2);
                }

                FieldInfo field = typeof(Type).GetField(serializedProperty.name);

                if (null == field) {
                    return default(Vector2);
                }

                return GetFieldFloatRange(field, inherit);
            }
        }

        public static class Styles {
            public static GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout) {
                fontStyle = FontStyle.Bold
            };
        }

        public static class GUIContentHelper {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static GUIContent GetSerializedPropertyGUIContent<Type>(SerializedProperty serializedProperty, string displayName = default(string)) {
                if (default(string) == displayName) {
                    string attributeDisplayName = Attributes.GetSerializedPropertyDisplayName<Type>(serializedProperty);
                    displayName = default(string) == attributeDisplayName ? serializedProperty.displayName : attributeDisplayName;
                }

                string tooltip = Attributes.GetSerializedPropertyTooltip<Type>(serializedProperty);
                return new GUIContent(displayName, tooltip);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float GetStringWidth(string str, GUIStyle guiStyle) {
                return guiStyle.CalcSize(new GUIContent(str)).x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float GetStringHeight(string str, GUIStyle guiStyle) {
                return guiStyle.CalcSize(new GUIContent(str)).y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float GetStringHeightExt(string str, GUIStyle guiStyle) {
                return guiStyle.CalcHeight(new GUIContent(str), GetStringWidth(str, guiStyle));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float GetArrayMaxStringWidth<T>(T[] array, GUIStyle guiStyle, float defaultWidth) {
                if (null == array) {
                    return defaultWidth;
                }

                if (array.Length < 1) {
                    return defaultWidth;
                }

                float maxWidth = 0;
                int arrayLength = array.Length;
                for (int i = 0; i < arrayLength; i++) {
                    float width = GetStringWidth(array[i].ToString(), EditorStyles.toolbarPopup);
                    if (width > maxWidth) {
                        maxWidth = width;
                    }
                }

                if (maxWidth < 1) {
                    return defaultWidth;
                }

                return maxWidth;
            }
        }

        public static class Drawing {
            public const float kDefLabelWidth = 80.0f;
            public const float kDefControlWidth = 160.0f;
            public static readonly Color kDefLineColor = new Color32(0x99, 0x99, 0x99, 0xFF);
            public static readonly GUIContent kEmptyGUIContent = new GUIContent("");

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static GUIStyle MakeLabelGUIStyle(FontStyle fontStyle, int fontSizeIncrement) {
                GUIStyle guiStyle = new GUIStyle();

                guiStyle.font = GUI.skin.font;
                guiStyle.fontStyle = fontStyle;
                guiStyle.fontSize = GUI.skin.font.fontSize + fontSizeIncrement;

                return guiStyle;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void DrawLabel(GUIContent content, Vector2 position, Vector2 pivot = default(Vector2), Color fontColor = default(Color), FontStyle fontStyle = default(FontStyle), bool dropShadow = true) {
                GUIStyle labelStyle = new GUIStyle();
                labelStyle.normal.textColor = fontColor;
                labelStyle.font = GUI.skin.font;
                labelStyle.fontStyle = fontStyle;

                GUIContent textContent = content;
                Vector2 size = labelStyle.CalcSize(textContent);
                pivot.Scale(size);
                position -= pivot;

                if (dropShadow) {
                    EditorGUI.DropShadowLabel(new Rect(position, size), textContent, labelStyle);
                } else {
                    EditorGUI.LabelField(new Rect(position, size), textContent, labelStyle);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void DrawPreviewLabel(GUIContent content, Vector2 position, Color fontColor) {
                DrawLabel(
                    content,
                    position,
                    fontColor: fontColor,
                    fontStyle: FontStyle.Bold,
                    dropShadow: true
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawMultiValueTextField<Type>(
                SerializedProperty serializedProperty,
                string displayName = default(string),
                bool split = false,
                bool delayed = false,
                float labelWidth = kDefLabelWidth,
                float controlWidth = kDefControlWidth
            ) {
                GUIContent label = GUIContentHelper.GetSerializedPropertyGUIContent<Type>(serializedProperty, displayName);

                serializedProperty.serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                var newValue = serializedProperty.stringValue;

                if (serializedProperty.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        if (delayed) {
                            newValue = EditorGUILayout.DelayedTextField(default(string), GUILayout.Width(controlWidth));
                        } else {
                            newValue = EditorGUILayout.TextField(default(string), GUILayout.Width(controlWidth));
                        }
                    } else {
                        if (delayed) {
                            newValue = EditorGUILayout.DelayedTextField(label, default(string));
                        } else {
                            newValue = EditorGUILayout.TextField(label, default(string));
                        }
                    }
                    EditorGUI.showMixedValue = false;
                } else {
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        if (delayed) {
                            newValue = EditorGUILayout.DelayedTextField(newValue, GUILayout.Width(controlWidth));
                        } else {
                            newValue = EditorGUILayout.TextField(newValue, GUILayout.Width(controlWidth));
                        }
                    } else {
                        if (delayed) {
                            newValue = EditorGUILayout.DelayedTextField(label, newValue);
                        } else {
                            newValue = EditorGUILayout.TextField(label, newValue);
                        }
                    }
                }

                if (EditorGUI.EndChangeCheck()) {
                    serializedProperty.stringValue = newValue;
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }

                serializedProperty.serializedObject.Update();
                return !serializedProperty.hasMultipleDifferentValues;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawMultiValueEnumPopup<Type>(
                SerializedProperty serializedProperty,
                string displayName = default(string),
                bool enumItemsTooltips = true,
                bool split = false,
                float labelWidth = kDefLabelWidth,
                float controlWidth = kDefControlWidth
            ) {
                GUIContent label = GUIContentHelper.GetSerializedPropertyGUIContent<Type>(serializedProperty, displayName);

                GUIContent[] names = new GUIContent[serializedProperty.enumDisplayNames.Length];
                for (int i = 0; i < names.Length; i++) {
                    names[i] = new GUIContent(
                        serializedProperty.enumDisplayNames[i],
                        enumItemsTooltips ? serializedProperty.enumDisplayNames[i] + " " + displayName : default(string)
                    );
                }

                serializedProperty.serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                var newValue = serializedProperty.enumValueIndex;

                if (serializedProperty.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        newValue = EditorGUILayout.Popup(0, names, GUILayout.Width(controlWidth));
                    } else {
                        newValue = EditorGUILayout.Popup(label, 0, names);
                    }
                    EditorGUI.showMixedValue = false;
                } else {
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        newValue = EditorGUILayout.Popup(newValue, names, GUILayout.Width(controlWidth));
                    } else {
                        newValue = EditorGUILayout.Popup(label, newValue, names);
                    }
                }

                if (EditorGUI.EndChangeCheck()) {
                    serializedProperty.enumValueIndex = newValue;
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }

                serializedProperty.serializedObject.Update();
                return !serializedProperty.hasMultipleDifferentValues;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawMultiValueColorField<Type>(
                SerializedProperty serializedProperty,
                string displayName = default(string),
                bool split = false,
                float labelWidth = kDefLabelWidth,
                float controlWidth = kDefControlWidth
            ) {
                GUIContent label = GUIContentHelper.GetSerializedPropertyGUIContent<Type>(serializedProperty, displayName);

                serializedProperty.serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                var newValue = serializedProperty.colorValue;

                if (serializedProperty.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        newValue = EditorGUILayout.ColorField(default(Color), GUILayout.Width(controlWidth));
                    } else {
                        newValue = EditorGUILayout.ColorField(label, default(Color));
                    }
                    EditorGUI.showMixedValue = false;
                } else {
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        newValue = EditorGUILayout.ColorField(newValue, GUILayout.Width(controlWidth));
                    } else {
                        newValue = EditorGUILayout.ColorField(label, newValue);
                    }
                }

                if (EditorGUI.EndChangeCheck()) {
                    serializedProperty.colorValue = newValue;
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }

                serializedProperty.serializedObject.Update();
                return !serializedProperty.hasMultipleDifferentValues;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawMultiValueGradientField<Type>(
                SerializedProperty serializedProperty,
                string displayName = default(string),
                bool split = false,
                float labelWidth = kDefLabelWidth,
                float controlWidth = kDefControlWidth
            ) {
                GUIContent label = GUIContentHelper.GetSerializedPropertyGUIContent<Type>(serializedProperty, displayName);

                serializedProperty.serializedObject.Update();
                EditorGUI.BeginChangeCheck();

                if (serializedProperty.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        EditorGUILayout.PropertyField(serializedProperty, kEmptyGUIContent, GUILayout.Width(controlWidth));
                    } else {
                        EditorGUILayout.PropertyField(serializedProperty);
                    }
                    EditorGUI.showMixedValue = false;
                } else {
                    if (split) {
                        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
                        EditorGUILayout.PropertyField(serializedProperty, kEmptyGUIContent, GUILayout.Width(controlWidth));
                    } else {
                        EditorGUILayout.PropertyField(serializedProperty);
                    }
                }

                if (EditorGUI.EndChangeCheck()) {
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }

                serializedProperty.serializedObject.Update();
                return !serializedProperty.hasMultipleDifferentValues;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawMultiValueToggle<Type>(SerializedProperty serializedProperty, bool left = false, string displayName = default(string), float controlWidth = kDefControlWidth) {
                GUIContent label = GUIContentHelper.GetSerializedPropertyGUIContent<Type>(serializedProperty, displayName);

                serializedProperty.serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                var newValue = serializedProperty.boolValue;

                if (serializedProperty.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                    if (left) {
                        newValue = EditorGUILayout.ToggleLeft(label, default(bool), GUILayout.Width(controlWidth));
                    } else {
                        newValue = EditorGUILayout.Toggle(label, default(bool), GUILayout.Width(controlWidth));
                    }
                    EditorGUI.showMixedValue = false;
                } else {
                    if (left) {
                        newValue = EditorGUILayout.ToggleLeft(label, newValue, GUILayout.Width(controlWidth));
                    } else {
                        newValue = EditorGUILayout.Toggle(label, newValue, GUILayout.Width(controlWidth));
                    }
                }

                if (EditorGUI.EndChangeCheck()) {
                    serializedProperty.boolValue = newValue;
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }

                serializedProperty.serializedObject.Update();
                return !serializedProperty.hasMultipleDifferentValues;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawMultiValueObjectField<Type>(SerializedProperty serializedProperty, string displayName = default(string), bool allowSceneObjects = true) {
                GUIContent label = GUIContentHelper.GetSerializedPropertyGUIContent<Type>(serializedProperty, displayName);

                serializedProperty.serializedObject.Update();

                if (serializedProperty.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                    EditorGUILayout.ObjectField(serializedProperty, label);
                    EditorGUI.showMixedValue = false;
                } else {
                    EditorGUILayout.ObjectField(serializedProperty, label);
                }

                serializedProperty.serializedObject.ApplyModifiedProperties();
                serializedProperty.serializedObject.Update();
                return !serializedProperty.hasMultipleDifferentValues;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawMultiValueSlider<Type>(SerializedProperty serializedProperty, string displayName = default(string)) {
                GUIContent label = GUIContentHelper.GetSerializedPropertyGUIContent<Type>(serializedProperty, displayName);

                serializedProperty.serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                var newValue = serializedProperty.floatValue;

                var range = Attributes.GetSerializedPropertyFloatRange<Type>(serializedProperty);

                if (serializedProperty.hasMultipleDifferentValues) {
                    EditorGUI.showMixedValue = true;
                    if (default(Vector2) != range) {
                        newValue = EditorGUILayout.Slider(label, default(float), range.x, range.y);
                    } else {
                        newValue = EditorGUILayout.FloatField(label, default(float));
                    }
                    EditorGUI.showMixedValue = false;
                } else {
                    if (default(Vector2) != range) {
                        newValue = EditorGUILayout.Slider(label, newValue, range.x, range.y);
                    } else {
                        newValue = EditorGUILayout.FloatField(label, newValue);
                    }
                }

                if (EditorGUI.EndChangeCheck()) {
                    serializedProperty.floatValue = newValue;
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }

                serializedProperty.serializedObject.Update();
                return !serializedProperty.hasMultipleDifferentValues;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawWhatsThisSections<Type>(SerializedProperty serializedProperty) {
                WhatsThisAttribute[] attributes = Attributes.GetSerializedPropertyAttributes<Type, WhatsThisAttribute>(serializedProperty);

                if (null == attributes) {
                    return false;
                }

                if (0 == attributes.Length) {
                    return false;
                }

                bool result = false;

                for (int i = 0; i < attributes.Length; i++) {
                    WhatsThisAttribute attribute = attributes[i];

                    if (null == attribute) {
                        continue;
                    }

                    if (0 == attribute.message.Length) {
                        continue;
                    }

                    EditorGUILayout.HelpBox(attribute.message, attribute.messageType);

                    result = true;
                }

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool DrawWhatsThisFoldout<Type>(ref bool folded, SerializedProperty serializedProperty, GUIContent foldoutLabel) {
                folded = EditorGUILayout.Foldout(folded, foldoutLabel);

                if (!folded) {
                    return false;
                }

                return DrawWhatsThisSections<Type>(serializedProperty);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void DrawFullHorizontalLine(float yOffset, float widthExpand, Color color = default) {
                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(1));
                rect.height = 1;
                rect.y += yOffset;
                rect.xMin -= widthExpand;
                rect.xMax += widthExpand;
                EditorGUI.DrawRect(rect, color == default ? kDefLineColor : color);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void DrawVerticalLine(float xOffset, float yOffset, float height, float heightExpand, Color color = default) {
                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(1));
                rect.width = 1;
                rect.height = height;
                rect.x += xOffset;
                rect.y += yOffset;
                rect.yMin -= heightExpand;
                rect.yMax += heightExpand;
                EditorGUI.DrawRect(rect, color == default ? kDefLineColor : color);
            }
        }
    }
}
