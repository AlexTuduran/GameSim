//
// Transform Inside Sphere Random Rotate
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OmniSARTechnologies.Helper
{
    [AddComponentMenu("OmniSAR Technologies/Common/Helper/Transform/Transform Inside Sphere Random Rotate")]
    public class TransformInsideSphereRandomRotate : MonoBehaviour
    {
        public Vector3 magnitude = Vector3.one;

        private Vector3 _eulerAngles;
        
        public void OnEnable()
        {
            Random.InitState((int)(DateTime.Now.Ticks % int.MaxValue));
        }

        private float GetRand()
        {
            return Random.value * 2.0f - 1.0f;
        }

        public void Update()
        {
            if (!isActiveAndEnabled) {
                return;
            }
            
#if true
#if !true
            Vector2 rand = Random.insideUnitCircle;
            _eulerAngles.x = rand.x;
            _eulerAngles.y = rand.y;
            _eulerAngles.z = 0.0f;
#else
            //_eulerAngles = Random.insideUnitSphere;
            
            //_eulerAngles = Random.onUnitSphere;
            //_eulerAngles.z = 0.0f;
            //_eulerAngles = _eulerAngles.normalized;
            float t = Time.timeSinceLevelLoad % 1000.0f;
            t *= Mathf.PI * 2.0f * 1587.35791f * (1.0f + Random.value);
            _eulerAngles.x = Mathf.Sin(t);
            _eulerAngles.y = Mathf.Cos(t);
            _eulerAngles.z = 0.0f;
            float r = Random.value;
            r = Mathf.Pow(r, 0.5f);
            _eulerAngles *= r;
#endif
            
            _eulerAngles.x *= magnitude.x;
            _eulerAngles.y *= magnitude.y;
            _eulerAngles.z *= magnitude.z;
#else    
            _eulerAngles.x = GetRand() * magnitude.x;
            _eulerAngles.y = GetRand() * magnitude.y;
            _eulerAngles.z = GetRand() * magnitude.z;
#endif

            transform.localEulerAngles = _eulerAngles;
        }
    }
}
