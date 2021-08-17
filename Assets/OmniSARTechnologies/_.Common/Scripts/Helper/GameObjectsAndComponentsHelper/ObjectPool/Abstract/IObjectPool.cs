//
// Object Pool interface
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

namespace OmniSARTechnologies.Helper {
    public interface IObjectPool {
        IPoolObject AcquirePoolObject();
        bool ReleasePoolObject(IPoolObject poolObject);
    }
}