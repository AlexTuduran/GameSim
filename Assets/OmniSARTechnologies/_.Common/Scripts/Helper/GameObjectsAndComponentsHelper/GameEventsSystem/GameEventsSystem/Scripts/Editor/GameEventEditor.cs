//
// Game Event Editor
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;

namespace OmniSARTechnologies.GameEventsSystem {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor {
        private GameEvent _component {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return (GameEvent)target;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            bool isPlaying = Application.isPlaying;
            if (isPlaying) {
                int count = _component.gameEventReceivers.Count;

                GUILayout.Label("Receivers", EditorStyles.boldLabel);
                if (0 == count) {
                    GUILayout.Label("<None>");
                } else {
                    
                    for (int i = 0; i < count; i++) {
                        EditorGUILayout.ObjectField(_component.gameEventReceivers[i].gameObject, typeof(GameObject), true);
                    }
                }
            } else {
                GUILayout.Label("Receives available in play mode.", EditorStyles.helpBox);
            }

            GUI.enabled = isPlaying;
            if (GUILayout.Button("Send Game Event")) {
                _component.SendGameEvent();
            }
            GUI.enabled = true;
        }
    }
}
