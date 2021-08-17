//
// Key Handler
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace OmniSARTechnologies.Helper {
    public class KeyHandler : MonoBehaviour {
        public bool enableInvoke = true;
        public KeyCode keyCode = KeyCode.E;
        public UnityEvent onKey;
        public UnityEvent onKeyDown;
        public UnityEvent onKeyUp;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetEnableInvoke(bool enable) {
            enableInvoke = enable;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            if (!enableInvoke) {
                return;
            }

            if (Input.GetKey(keyCode)) {
                onKey.Invoke();
            }

            if (Input.GetKeyDown(keyCode)) {
                onKeyDown.Invoke();
            }

            if (Input.GetKeyUp(keyCode)) {
                onKeyUp.Invoke();
            }
        }
    }
}