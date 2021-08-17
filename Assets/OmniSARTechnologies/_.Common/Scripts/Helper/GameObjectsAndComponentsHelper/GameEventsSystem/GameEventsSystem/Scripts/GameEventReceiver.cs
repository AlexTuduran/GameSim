//
// Game Event Receiver
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace OmniSARTechnologies.GameEventsSystem {
    public class GameEventReceiver : MonoBehaviour {
        public GameEvent gameEvent;
        public UnityEvent onGameEventReceived;
        public bool log = false;

        private bool _validOnGameEventReceived = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Awake() {
            _validOnGameEventReceived = null != onGameEventReceived;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            if (!gameEvent) {
                return;
            }

            gameEvent.RegisterGameEventReceiver(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDisable() {
            if (!gameEvent) {
                return;
            }

            gameEvent.UnregisterGameEventReceiver(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnGameEventReceived() {
            if (log) {
                Debug.LogFormat("{0} '{1}' received on '{2}'.", nameof(GameEvent), gameEvent ? gameEvent.name : "<NULL>", name);
            }

            if (!_validOnGameEventReceived) {
                Debug.LogErrorFormat("{0} '{1}' received on '{2}', but '{3}' is NULL: Unhandled {0} '{1}'!", nameof(GameEvent), gameEvent ? gameEvent.name : "<NULL>", name, nameof(onGameEventReceived));
                return;
            }

            onGameEventReceived.Invoke();
        }
    }
}
