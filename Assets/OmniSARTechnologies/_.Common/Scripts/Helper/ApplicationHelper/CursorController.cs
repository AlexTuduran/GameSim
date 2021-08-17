//
// Cursor Controller
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public static class CursorController {
       [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LockCursor(bool lockCursor) {
            //Debug.LogFormat("LockCursor({0})", lockCursor);

            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }
    }
}