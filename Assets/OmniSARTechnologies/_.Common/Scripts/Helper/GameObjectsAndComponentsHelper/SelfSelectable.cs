//
// Self Selectable
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace OmniSARTechnologies.Helper {
    [ExecuteInEditMode]
    public class SelfSelectable : MonoBehaviour {
        public bool selfSelectOnSceneLoad = false;

        public void Start() {
            if (!selfSelectOnSceneLoad) {
                return;
            }

            SelectSelfGameObject();
        }

    	public void SelectSelfGameObject() {
#if UNITY_EDITOR
    		Selection.activeGameObject = gameObject;
#endif // UNITY_EDITOR
    	}
    }
}