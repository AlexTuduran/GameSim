//
// Transform Rotate
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

// For user: Uncomment this to execute rotation while the game is not running
//#define ___EXECUTE_IN_EDITOR___

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
#if ___EXECUTE_IN_EDITOR___
    [ExecuteInEditMode]
#endif // ___EXECUTE_IN_EDITOR___
    [AddComponentMenu("OmniSAR Technologies/Common/Helper/Transform/Transform Rotate")]
    public class TransformRotate : MonoBehaviour {
        public Vector3 eulerAnglesSpeed;
        public bool localRotation = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate() {
            if (!isActiveAndEnabled) {
                return;
            }

            if (localRotation) {
                transform.localEulerAngles += eulerAnglesSpeed * Time.deltaTime;
            } else {
                transform.RotateAround(transform.position, Vector3.right, eulerAnglesSpeed.x * Time.deltaTime);
                transform.RotateAround(transform.position, Vector3.up, eulerAnglesSpeed.y * Time.deltaTime);
                transform.RotateAround(transform.position, Vector3.forward, eulerAnglesSpeed.z * Time.deltaTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            Rotate();
        }
    }
}