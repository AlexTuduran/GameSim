//
// Toggle Game Object
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class ToggleGameObject : MonoBehaviour {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Toggle() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOn() {
            gameObject.SetActive(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOff() {
            gameObject.SetActive(false);
        }
    }
}
