//
// Pool Object interface
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public interface IPoolObject {
        IObjectPool GetObjectPool();
        GameObject GetGameObject();
        void OnPoolObjectInstantiated(IObjectPool objectPool);
        void OnPoolObjectAcquired();
        void OnPoolObjectReleased();
    }
}