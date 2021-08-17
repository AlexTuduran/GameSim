//
// Simple Scene Switcher
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace OmniSARTechnologies.Helper {
    [ExecuteInEditMode]
    public class SimpleSceneSwitcher : MonoBehaviour {
        public Text sceneNameText;

        private void OnEnable() {
            UpdateSceneNameText();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeActiveScene(int buildIndexOffset) {
            LoadSceneByIndex(
                SceneManager.GetActiveScene().buildIndex +
                buildIndexOffset
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadSceneByIndex(int index) {
            if (SceneManager.sceneCountInBuildSettings < 1) {
                return;
            }

            index += SceneManager.sceneCountInBuildSettings << 10;
            index %= SceneManager.sceneCountInBuildSettings;
            index = Mathf.Clamp(index, 0, SceneManager.sceneCountInBuildSettings - 1);

            Debug.LogFormat("LoadSceneByIndex: {0}  ({1} / {2})", index, index + 1, SceneManager.sceneCountInBuildSettings);

#if UNITY_EDITOR
            if (Application.isPlaying) {
                SceneManager.LoadScene(index);
            } else {
                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
            }
#else
            SceneManager.LoadScene(index);
#endif
            UpdateSceneNameText();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadFirstScene() {
            LoadSceneByIndex(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetCurrentSceneName() {
            return SceneManager.GetActiveScene().name;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSceneNameText() {
            if (!sceneNameText) {
                return;
            }

            sceneNameText.text = GetCurrentSceneName();
        }
    }
}
