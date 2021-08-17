//
// Pool Object Factory
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    public class PoolObjectFactory : MonoBehaviour, IPoolObjectFactory {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual IPoolObject CreatePoolObject() {
            throw new NotImplementedException();
        }
    }
}