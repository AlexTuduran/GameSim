using System.Runtime.CompilerServices;
using UnityEngine;

namespace FragniteGames {
    public class QualitySettingsManager : MonoBehaviour {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetVSync(int vSyncCount) {
            QualitySettings.vSyncCount = vSyncCount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetVSyncEnabled(bool enabled) {
            QualitySettings.vSyncCount = enabled ? 1 : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetSetVSyncEnabled() {
            return QualitySettings.vSyncCount > 0;
        }

        [ContextMenu("Toggle VSync Enabled Now")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToggleVSyncEnabled() {
            SetVSyncEnabled(!GetSetVSyncEnabled());
        }
    }
}
