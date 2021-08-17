//
// Game Event
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

#define ___FORCED_LOGGING___

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.GameEventsSystem {
    [CreateAssetMenu(menuName = "OmniSAR Technologies/Game Event System/Game Event", order = 10, fileName = "New GameEvent")]
    public class GameEvent : ScriptableObject {
        public bool warnIfNoReceiver = false;
        public bool log = false;
        public bool deepLog = false;

        private readonly List<GameEventReceiver> _gameEventReceivers = new List<GameEventReceiver>();

        public List<GameEventReceiver> gameEventReceivers {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return _gameEventReceivers;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendGameEvent(string sender) {
            int length = _gameEventReceivers.Count;

            if (length < 1 && warnIfNoReceiver) {
                Debug.LogWarningFormat("[{0}]: {1} was called on '{2}', but there are no receivers: {3} NOT sent!", sender, nameof(SendGameEvent), name, nameof(GameEvent));
                return;
            }

            if (deepLog) {
                Debug.LogFormat("[{0}]: Sending {1} '{2}' to {3} receiver{4}..", sender, nameof(GameEvent), name, length, length == 1 ? "" : "s");
            }

            for (int i = _gameEventReceivers.Count - 1; i >= 0; i--) {
                var receiverName = _gameEventReceivers[i].name;

                if (log) {
                    if (deepLog) {
                        Debug.LogFormat("[{0}]: {1} '{2}' sent to receiver -> '{3}'.", sender, nameof(GameEvent), name, receiverName);
                    } else {
                        Debug.LogFormat("[{0}]: '{1}' -> '{2}'.", sender, name, receiverName);
                    }
                }
#if ___FORCED_LOGGING___
                else {
                    Debug.LogFormat("* [{0}]: '{1}' -> '{2}'.", sender, name, receiverName);
                }
#endif

                _gameEventReceivers[i].OnGameEventReceived();
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendGameEvent() {
            SendGameEvent("?");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterGameEventReceiver(GameEventReceiver gameEventReceiver) {
            if (!gameEventReceiver) {
                Debug.LogErrorFormat("Can't register {0} on '{1}': NULL {2} param!", nameof(GameEventReceiver), name, nameof(gameEventReceiver));
                return;
            }

            if (_gameEventReceivers.Contains(gameEventReceiver)) {
                return;
            }

            _gameEventReceivers.Add(gameEventReceiver);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnregisterGameEventReceiver(GameEventReceiver gameEventReceiver) {
            if (!gameEventReceiver) {
                Debug.LogErrorFormat("Can't UN-REGISTER {0} on '{1}': NULL {2} param!", nameof(GameEventReceiver), name, nameof(gameEventReceiver));
                return;
            }

            if (!_gameEventReceivers.Contains(gameEventReceiver)) {
                return;
            }

            _gameEventReceivers.Remove(gameEventReceiver);
        }
    }
}