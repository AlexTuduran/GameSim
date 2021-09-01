using System.Runtime.CompilerServices;
using UnityEngine;

namespace FragniteGames {
    [ExecuteInEditMode]
    public class PlayerLifecycleController : MonoBehaviour {
        public bool stopPlayer = false;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopPlayer() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            if (stopPlayer) {
                StopPlayer();
            }
        }

    }
}