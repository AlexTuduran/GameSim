//
// Simple Game Object Switcher
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace OmniSARTechnologies.Helper {
    [ExecuteInEditMode]
    public class SimpleGameObjectSwitcher : MonoBehaviour {
        [Header("References")]
        public Transform gameObjectsRoot;
        public Text gameObjectNameText;

        [Header("Messaging To Game Objects")]
        public bool sendMessageToGameObjects = false;
        public GameObject[] receiverGameObjects = null;
        public string messageToGameObjects = "";

        [Header("Debug / Output")]
        public GameObject currentGameObject = null;

        private List<GameObject> m_GameObjects = null;

        private int m_GameObjectIndex = 0;
        public int gameObjectIndex {
            get {
                return GetClampedGameObjectIndex(m_GameObjectIndex);
            }

            set {
                m_GameObjectIndex = GetClampedGameObjectIndex(value);
            }
        }

        public int gameObjectMaxIndex {
            get {
                if (null == m_GameObjects) {
                    return 0;
                }

                return m_GameObjects.Count - 1;
            }
        }

        private const int InvalidIndex = -1;
        private const string InvalidGameObjectName = "N/A";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEnable() {
            AcquireGameObjects();

#if !UNITY_EDITOR
            SetActiveGameObject(m_GameObjectIndex);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisable() {
            ReleaseGameObjects();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AcquireGameObjects() {
            ReleaseGameObjects();

            if (!gameObjectsRoot) {
                m_GameObjectIndex = InvalidIndex;
                return;
            }

            m_GameObjects = new List<GameObject>();

            for (int i = 0; i < gameObjectsRoot.childCount; i++) {
                m_GameObjects.Add(gameObjectsRoot.GetChild(i).gameObject);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseGameObjects() {
            if (null == m_GameObjects) {
                return;
            }

            m_GameObjects.Clear();
            m_GameObjects = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetClampedGameObjectIndex(int index) {
            if (null == m_GameObjects) {
                return 0;
            }

            if (m_GameObjects.Count < 1) {
                return 0;
            }

            index += m_GameObjects.Count << 10;
            index %= m_GameObjects.Count;
            index = Mathf.Clamp(index, 0, gameObjectMaxIndex);

            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeActiveGameObject(int indexOffset) {
            if (null == m_GameObjects) {
                return;
            }

            if (m_GameObjects.Count < 1) {
                return;
            }

            int newGameObjectIndex = GetClampedGameObjectIndex(m_GameObjectIndex + indexOffset);
            SetActiveGameObject(newGameObjectIndex);
            UpdateGameObjectNameText();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetActiveGameObject(int index) {
            if (null == m_GameObjects) {
                return;
            }

            if (m_GameObjects.Count < 1) {
                return;
            }

            for (int i = 0; i < m_GameObjects.Count; i++) {
                bool active = (i == index);
                m_GameObjects[i].SetActive(active);
                if (active) {
                    currentGameObject = m_GameObjects[i];
                }
            }

            m_GameObjectIndex = index;

            if (sendMessageToGameObjects && (messageToGameObjects.Length > 0)) {
                for (int i = 0; i < receiverGameObjects.Length; i++) {
                    GameObject go = receiverGameObjects[i];
                    if (!go) {
                        continue;
                    }

                    go.BroadcastMessage(messageToGameObjects);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetCurrentGameObjectName() {
            if (null == m_GameObjects) {
                return InvalidGameObjectName;
            }

            if (m_GameObjects.Count < 1) {
                return InvalidGameObjectName;
            }

            if (InvalidIndex == m_GameObjectIndex) {
                return InvalidGameObjectName;
            }

            return m_GameObjects[m_GameObjectIndex].name;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateGameObjectNameText() {
            if (!gameObjectNameText) {
                return;
            }

            gameObjectNameText.text = GetCurrentGameObjectName();
        }
    }
}
