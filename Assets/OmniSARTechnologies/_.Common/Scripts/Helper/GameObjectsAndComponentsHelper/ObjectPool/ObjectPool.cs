//
// Object Pool
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

#define n___DEBUG_LOG___

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class ObjectPool : MonoBehaviour, IObjectPool {
        [Header("Pool Settings")]
        public PoolObjectFactory poolObjectFactory;
        public int numObjects = 10;
        public Transform parentTo;
        
        private Queue<IPoolObject> _pool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start() {
            _pool = new Queue<IPoolObject>(numObjects);
            for (int i = 0; i < numObjects; i++) {
                IPoolObject poolObject = poolObjectFactory.CreatePoolObject();
                poolObject.GetGameObject().transform.parent = parentTo;
                poolObject.OnPoolObjectInstantiated(this);
                ReleasePoolObject(poolObject);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual IPoolObject AcquirePoolObject() {
            if (_pool.Count < 1) {
                Debug.LogWarning("Could not acquire pool object: Empty object pool");
                return null;
            }

            IPoolObject poolObject = _pool.Dequeue();

            if (null == poolObject) {
                Debug.LogWarning("Could not acquire pool object: Dequeue() returned NULL");
                return null;
            }

            poolObject.OnPoolObjectAcquired();

#if ___DEBUG_LOG___
            Debug.Log("Pool object acquired");
#endif // ___DEBUG_LOG___

            return poolObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool ReleasePoolObject(IPoolObject poolObject) {
            if (null == poolObject) {
                Debug.LogWarning("Could not release pool object: NULL pool object");
                return false;
            }

            poolObject.OnPoolObjectReleased();

            _pool.Enqueue(poolObject);

#if ___DEBUG_LOG___
            Debug.Log("Pool object released");
#endif // ___DEBUG_LOG___

            return true;
        }
    }
}