//
// Game Object Info
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OmniSARTechnologies.Helper {
    public class GameObjectInfo : MonoBehaviour {
        public string GetInfo() {

#if UNITY_EDITOR

            string sceneStr = "Scene Info: ";
            Scene scene = gameObject.scene;
            if (string.IsNullOrEmpty(scene.name) || string.IsNullOrEmpty(scene.path)) {
                sceneStr += "N/A";
            } else {
                sceneStr += string.Format("\"{0}\" (\"{1}\") Loaded: {2}", scene.name, scene.path, scene.isLoaded ? "Y" : "N");
            }

            string assetStr = "Asset: ";
            if (AssetDatabase.Contains(gameObject)) {
                assetStr += "Path: " + AssetDatabase.GetAssetPath(gameObject);
            } else {
                sceneStr += "N/A";
            }

            return
                sceneStr + "\n\r" +
                assetStr;

#else // UNITY_EDITOR

            return default;

#endif // UNITY_EDITOR
        }
    }
}