//
// Game Object Destroyer
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class GameObjectDestroyer : MonoBehaviour {
        public bool destroyInBuildOnly = false;
        public bool destroyOnAwake = false;
        public bool destroyOnStart = false;
        public bool destroyOnEnable = false;
        public bool destroyOnDisable = false;
        public bool destroyDelayed = false;
        public float delay = 1.0f;
        public GameObject targetGameObject;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyTargetGameObject() {
            if (destroyInBuildOnly) {
#if UNITY_EDITOR
                if (null == targetGameObject) {
                    Debug.LogErrorFormat("(Editor) NULL {0} in '{1}': Will not destroy.", nameof(targetGameObject), name);
                    return;
                }

#if !true
#if UNITY_EDITOR
                DestroyImmediate(targetGameObject);
#else
                Destroy(targetGameObject);
#endif
#else
                Destroy(targetGameObject);
#if !true
                Debug.LogFormat("Called '{0}' on '{1}'.", nameof(Destroy), name);
#endif

                
#endif
            } else {
                if (null == targetGameObject) {
                    Debug.LogErrorFormat("(Player) NULL {0} in '{1}': Will not destroy.", nameof(targetGameObject), name);
                    return;
                }
                
                Destroy(targetGameObject);
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DelayedDestroyTargetGameObject() {
            StopAllCoroutines();
            StartCoroutine(DoDelayedDestroyTargetGameObject(delay));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator DoDelayedDestroyTargetGameObject(float delayTime) {
            yield return new WaitForSeconds(delayTime);
            DestroyTargetGameObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Awake() {
            if (destroyOnAwake) {
                DestroyTargetGameObject();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start() {
            if (destroyOnStart) {
                DestroyTargetGameObject();
            }

            if (destroyDelayed) {
                DelayedDestroyTargetGameObject();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            if (destroyOnEnable) {
                DestroyTargetGameObject();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDisable() {
            if (destroyOnDisable) {
                DestroyTargetGameObject();
            }
        }
    }
}