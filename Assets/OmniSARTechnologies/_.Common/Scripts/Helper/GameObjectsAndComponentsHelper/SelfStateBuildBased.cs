//
// Self-State Build-Based
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class SelfStateBuildBased : MonoBehaviour {
        public bool stateActiveInEditor = true;
        public bool stateActiveInBuild = false;

        public void OnEnable() {
#if UNITY_EDITOR
            gameObject.SetActive(stateActiveInEditor);
#else // UNITY_EDITOR
            gameObject.SetActive(stateActiveInBuild);
#endif // UNITY_EDITOR
        }
    }
}
