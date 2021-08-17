//
// Game Object Extensions
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

namespace OmniSARTechnologies.Helper {
    public static class GameObjectExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAsset(this GameObject gameObject) {
            if (!gameObject) {
                return false;
            }

            return string.IsNullOrEmpty(gameObject.scene.name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Select(this GameObject gameObject) {
#if UNITY_EDITOR
            if (gameObject.IsAsset()) {
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(gameObject));
            } else {
                Selection.activeGameObject = gameObject;
            }

            EditorGUIUtility.PingObject(gameObject);
#endif // UNITY_EDITOR
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this GameObject gameObject) {
            GameObject.Destroy(gameObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyImmediate(this GameObject gameObject, [DefaultValue("false")] bool allowDestroyingAssets) {
            GameObject.DestroyImmediate(gameObject, allowDestroyingAssets);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<GameObject> GetAllGameObjectsInScene(this GameObject gameObject, Scene scene, Predicate<GameObject> isAutoSettable, [DefaultValue("false")] bool includeInactive) {
            List<GameObject> gameObjects = new List<GameObject>(10);

            var rootGameObjects = scene.GetRootGameObjects();

            for (int j = 0; j < rootGameObjects.Length; j++) {
                var components = rootGameObjects[j].GetComponentsInChildren<Component>(includeInactive);
                for (int i = 0; i < components.Length; i++) {
                    var go = components[i].gameObject;
                    if (isAutoSettable(go)) {
                        if (!gameObjects.Contains(go)) {
                            gameObjects.Add(go);
                        }
                    }
                }
            }

            return gameObjects;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<GameObject> GetAllGameObjectsInScene(this GameObject gameObject, Predicate<GameObject> isAutoSettable, [DefaultValue("false")] bool includeInactive) {
            return GetAllGameObjectsInScene(gameObject, SceneManager.GetActiveScene(), isAutoSettable, includeInactive);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<GameObject> GetAllGameObjectsInScene(this GameObject gameObject, [DefaultValue("false")] bool includeInactive) {
            return GetAllGameObjectsInScene(gameObject, SceneManager.GetActiveScene(), x => true, includeInactive);
        }

        // adapted from https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html?_ga=2.45248743.275573370.1590756765-147773489.1555672997
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetCopyOf<T>(this Component component, T other) where T : Component {
            var type = component.GetType();

            // type mis-match
            if (type != other.GetType()) {
                return null;
            }

            BindingFlags flags =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Default |
                BindingFlags.DeclaredOnly;

            var propertyInfos = type.GetProperties(flags);
            for (int i = 0; i < propertyInfos.Length; i++) {
                var propertyInfo = propertyInfos[i];

                if (propertyInfo.CanWrite) {
                    try {
                        propertyInfo.SetValue(component, propertyInfo.GetValue(other, null), null);
                    } catch {
                        // In case of NotImplementedException being thrown. For some reason
                        // specifying that exception didn't seem to catch it, so I didn't
                        // catch anything specific.
                    }
                }
            }

            var fieldInfos = type.GetFields(flags);
            for (int i = 0; i < fieldInfos.Length; i++) {
                var fieldInfo = fieldInfos[i];
                fieldInfo.SetValue(component, fieldInfo.GetValue(other));
            }

            return component as T;
        }

        // adapted from https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html?_ga=2.45248743.275573370.1590756765-147773489.1555672997
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AddComponent<T>(this GameObject gameObject, T toAdd) where T : Component {
            return gameObject.AddComponent<T>().GetCopyOf(toAdd);
        }
    }
}