using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FragniteGames {
    public class FrameProvider : MonoBehaviour {
        [Header("Frames")]
        public List<Texture2D> colorTextures = null;

        public List<Texture2D> depthTextures = null;

        public int NumFrames => colorTextures?.Count ?? 0;
        public int LastFrameIndex => Mathf.Max(0, NumFrames - 1);

        private int m_CurrentFrameIndex = 0;
        public int CurrentFrameIndex => m_CurrentFrameIndex;

        public Texture2D CurrentColorTexture => colorTextures?[m_CurrentFrameIndex];
        public Texture2D CurrentDepthTexture => depthTextures?[m_CurrentFrameIndex];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToFrame(int frameIndex) {
            m_CurrentFrameIndex = Mathf.Clamp(frameIndex, 0, LastFrameIndex);
        }

        [ContextMenu("Go To First Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToFirstFrame() {
            m_CurrentFrameIndex = 0;
        }

        [ContextMenu("Go To Last Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToLastFrame() {
            m_CurrentFrameIndex = LastFrameIndex;
        }

        [ContextMenu("Go To Previous Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToPreviousFrame() {
            int numFrames = NumFrames;
            m_CurrentFrameIndex = ((m_CurrentFrameIndex - 1) + numFrames + numFrames) % numFrames;
        }

        [ContextMenu("Go To Next Frame Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToNextFrame() {
            int numFrames = NumFrames;
            m_CurrentFrameIndex = ((m_CurrentFrameIndex + 1) + numFrames + numFrames) % numFrames;
        }
    }
}