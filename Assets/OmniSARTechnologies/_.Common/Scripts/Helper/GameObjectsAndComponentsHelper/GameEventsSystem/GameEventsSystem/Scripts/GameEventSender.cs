//
// Game Event Sender
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.GameEventsSystem {
    public class GameEventSender : MonoBehaviour {
        public GameEvent gameEvent;
        public bool autoSendInAwake = false;
        public bool autoSendInStart = false;
        public bool autoSendInOnEnable = false;
        public bool autoSendInOnDisable = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Awake() {
            if (autoSendInAwake) {
                SendGameEvent();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start() {
            if (autoSendInStart) {
                SendGameEvent();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnEnable() {
            if (autoSendInOnEnable) {
                SendGameEvent();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDisable() {
            if (autoSendInOnDisable) {
                SendGameEvent();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendGameEvent() {
            if (!gameEvent) {
                Debug.LogErrorFormat("{0} was called on '{1}', but {2} is NULL: No {3} was sent!", nameof(SendGameEvent), name, nameof(gameEvent), nameof(GameEvent));
                return;
            }

            Debug.LogFormat("{0} '{1}' sent from '{2}'.", nameof(GameEvent), gameEvent.name, name);

            gameEvent.SendGameEvent("GO:" + name);
        }
    }
}