using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FragniteGames {
    public class MaterialTextureSwitch : MonoBehaviour {
        public Material targetMaterial = null;
        public List<Texture2D> textures = null;

        private int m_CurrentTextureIndex = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            SetCurrentTexture();
        }

        public void OnValidate() {
            SetCurrentTexture();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetTexture(Texture2D texture) {
            targetMaterial.mainTexture = texture;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetCurrentTexture() {
            if (m_CurrentTextureIndex < 0) {
                return;
            }

            if (m_CurrentTextureIndex > textures.Count - 1) {
                return;
            }

            SetTexture(textures[m_CurrentTextureIndex]);
        }

        [ContextMenu("Set Previous Texture Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPreviousTexture() {
            m_CurrentTextureIndex = ((m_CurrentTextureIndex - 1) + textures.Count + textures.Count) % textures.Count;
            SetCurrentTexture();
        }

        [ContextMenu("Set Next Texture Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNextTexture() {
            m_CurrentTextureIndex = ((m_CurrentTextureIndex + 1) + textures.Count + textures.Count) % textures.Count;
            SetCurrentTexture();
        }
    }
}
