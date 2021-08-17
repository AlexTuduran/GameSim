//
// Game Object Activator
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class GameObjectActivator : MonoBehaviour {
        public GameObject targetGameObject;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTargetActive(bool active) {
            if (!targetGameObject) {
                return;
            }

            targetGameObject.SetActive(active);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ActivateTarget() {
            SetTargetActive(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeactivateTarget() {
            SetTargetActive(false);
        }
    }
}
