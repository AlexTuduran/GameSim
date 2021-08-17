//
// Pool Object
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

#define n___DEBUG_LOG___

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class PoolObject : MonoBehaviour, IPoolObject {
        protected IObjectPool ObjectPool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual IObjectPool GetObjectPool() {
            return ObjectPool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual GameObject GetGameObject() {
            return gameObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnPoolObjectInstantiated(IObjectPool objectPool) {
            ObjectPool = objectPool;

#if ___DEBUG_LOG___
            Debug.LogFormat("OnPoolObjectInstantiated({0})", ObjectPool);
#endif // ___DEBUG_LOG___

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnPoolObjectAcquired() {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnPoolObjectReleased() {
            throw new NotImplementedException();
        }
    }
}