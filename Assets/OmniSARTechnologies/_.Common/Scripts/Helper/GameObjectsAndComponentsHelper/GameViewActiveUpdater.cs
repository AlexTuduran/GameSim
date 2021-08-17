//
// Game View Active Updater
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace OmniSARTechnologies.Helper {
    [ExecuteInEditMode]
    public class GameViewActiveUpdater : MonoBehaviour {
        public bool updateGameViewWhenPaused = true;

        [Range(1.0f, 100.0f)]
        public float pausedGameViewUpdateIntervalMS = 16.666666f; // equivalent of 60 fps

        public UnityEvent OnGameViewUpdate;
        public UnityEvent OnGameViewUpdated;

        private double m_LastGameViewRefreshT;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += ForceUpdateGameView;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDisable() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= ForceUpdateGameView;
#endif
        }

#if UNITY_EDITOR
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForceUpdateGameView() {
            if (!isActiveAndEnabled) {
                return;
            }

            if (updateGameViewWhenPaused && !UnityEditor.EditorApplication.isPlaying) {
                double tNow = UnityEditor.EditorApplication.timeSinceStartup;

                if (tNow - m_LastGameViewRefreshT > pausedGameViewUpdateIntervalMS * 0.001f) {
                    if (null != OnGameViewUpdate) {
                        OnGameViewUpdate.Invoke();
                    }

                    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();

                    if (null != OnGameViewUpdated) {
                        OnGameViewUpdated.Invoke();
                    }

                    m_LastGameViewRefreshT = tNow;
                }
            }
        }
#endif
    }
}