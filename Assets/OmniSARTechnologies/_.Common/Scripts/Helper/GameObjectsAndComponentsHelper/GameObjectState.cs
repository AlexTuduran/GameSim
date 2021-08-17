//
// Game Object State
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class GameObjectState : MonoBehaviour {
        public bool active = true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Awake() {
            gameObject.SetActive(active);
        }
    }
}