//
// Worker
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

namespace OmniSARTechnologies.Helper {
    public delegate void OnResetAllProgressDelegate();
    public delegate void OnSetProgressDelegate(float progress);

    public class Worker {
        protected bool m_IsBusy = false;
        public bool isBusy {
            get {
                return m_IsBusy;
            }
        }
    }
}
