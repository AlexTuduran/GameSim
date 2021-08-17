//
// Game Object State Setter
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace OmniSARTechnologies.Helper {
    public class GameObjectStateSetter : MonoBehaviour {
        public bool setStateOnAwake = true;
        public UnityEvent onStateSet;

        private const string kEnablePrefix = "-[ENABLE]";
        private const string kDisablePrefix = "-[DISABLE]";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Awake() {
            if (setStateOnAwake) {
                AutoSetStateOnAllGameObjectsInScene();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsAutoStateSettable(GameObject go) {
            if (!go) {
                return false;
            }

            return
                go.name.EndsWith(kEnablePrefix) ||
                go.name.EndsWith(kDisablePrefix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool AutoSetState(GameObject go) {
            if (!go) {
                return false;
            }

            bool wasSet = false;

            if (go.name.EndsWith(kEnablePrefix)) {
                go.SetActive(true);
                wasSet = true;
            }

            if (go.name.EndsWith(kDisablePrefix)) {
                go.SetActive(false);
                wasSet = true;
            }

            return wasSet;
        }

        [ContextMenu("Auto Set State On All Game Objects In Scene (You know what you're doing.)")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AutoSetStateOnAllGameObjectsInScene() {
            List<GameObject> settableGameObjects = gameObject.GetAllGameObjectsInScene(IsAutoStateSettable, true);
            
            Debug.LogFormat("================================");
            Debug.LogFormat(
                "{0} GameObject{1}.",
                settableGameObjects.Count,
                settableGameObjects.Count == 1 ? "" : "s"
            );
            Debug.LogFormat("================================");

            for (int i = 0; i < settableGameObjects.Count; i++) {
                var wasSet = AutoSetState(settableGameObjects[i]);
#if !true
                    Debug.LogFormat(
                        "'{0}' was {1}set to active.",
                        components[i].gameObject.name,
                        wasSet ? "" : "NOT "
                    );
#else
                if (wasSet) {
                    Debug.LogFormat(
                        "'{0}' was set to active.",
                        settableGameObjects[i].name
                    );
                }
#endif
            }

            if (null != onStateSet) {
                onStateSet.Invoke();
            }
        }
    }
}