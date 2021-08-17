//
// App Controller
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Runtime.CompilerServices;
using OmniSARTechnologies.GameEventsSystem;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace OmniSARTechnologies.Helper {
    [Serializable]
    public class OnCursorLockEvent : UnityEvent<bool> { }
    
    [AddComponentMenu("OmniSAR Technologies/Common/Helper/Application/App Controller")]
    public class AppController : MonoBehaviour {
        [Header("App Settings")]
        public KeyCode quitAppKey = KeyCode.Escape;
        public bool executeArgumentsAsMethods = false;

        [Header("App Controller Settings")]
        public bool appControllerInputEnabled = false;
        public bool setStatesInStart = true;

        [Header("Gameplay Input Settings")]
        public bool gameplayInputEnabled = false;
        public KeyCode toggleGameplayInputEnabledKey = KeyCode.F1;
        public GameEvent gameplayInputDisabledGameEvent;
        public GameEvent gameplayInputEnabledGameEvent;

        [Header("Cursor Settings")]
        public bool cursorLocked = false;
        public KeyCode toggleCursorLockedKey = KeyCode.F2;
        public GameEvent cursorUnlockedGameEvent;
        public GameEvent cursorLockedGameEvent;

        [Header("Menu Settings")]
        public bool menuEnabled = false;
        public KeyCode toggleMenuKey = KeyCode.F4;
        public GameEvent menuDisabledGameEvent;
        public GameEvent menuEnabledGameEvent;
        public UnityEvent onMenuEnabledByInput;
        public UnityEvent onMenuDisabledByInput;
        public UnityEvent onMenuEnabledByInputOrGameEvent;
        public UnityEvent onMenuDisabledByInputOrGameEvent;
        
        [Header("Logging Settings")]
        public bool deepLog = false;

        private bool _validGameplayInputDisabledGameEvent = false;
        private bool _validGameplayInputEnabledGameEvent = false;
        private bool _validCursorUnlockedGameEvent = false;
        private bool _validCursorLockedGameEvent = false;
        private bool _validMenuDisabledGameEvent = false;
        private bool _validMenuEnabledGameEvent = false;
        private bool _validOnMenuEnabledByInput = false;
        private bool _validOnMenuDisabledByInput = false;
        private bool _validOnMenuEnabledByInputOrGameEvent = false;
        private bool _validOnMenuDisabledByInputOrGameEvent = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasArg(string argName) {
            string[] args = Environment.GetCommandLineArgs();
            argName = "--" + argName;
            for (int i = 0; i < args.Length; i++) {
                if (args[i] == argName) {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Awake() {
            UnityEngine.Random.InitState(DateTime.Now.GetHashCode());

            _validGameplayInputDisabledGameEvent = null != gameplayInputDisabledGameEvent;
            _validGameplayInputEnabledGameEvent = null != gameplayInputEnabledGameEvent;
            _validCursorUnlockedGameEvent = null != cursorUnlockedGameEvent;
            _validCursorLockedGameEvent = null != cursorLockedGameEvent;
            _validMenuDisabledGameEvent = null != menuDisabledGameEvent;
            _validMenuEnabledGameEvent = null != menuEnabledGameEvent;
            _validOnMenuEnabledByInput = null != onMenuEnabledByInput;
            _validOnMenuDisabledByInput = null != onMenuDisabledByInput;
            _validOnMenuEnabledByInputOrGameEvent = null != onMenuEnabledByInputOrGameEvent;
            _validOnMenuDisabledByInputOrGameEvent = null != onMenuDisabledByInputOrGameEvent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start() {
            if (setStatesInStart) {
                SetAppControllerInputEnabled(appControllerInputEnabled);
                SetGameplayInputEnabled(gameplayInputEnabled);
                SetCursorLocked(cursorLocked);
                SetMenuEnabled(menuEnabled);
            }

            if (executeArgumentsAsMethods) {
                ExecuteArgs();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExecuteArgs() {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length < 1) {
                Debug.Log("ExecArgs: No args");
                return;
            }

            for (int i = 0; i < args.Length; i++) {
                string arg = args[i];

                if (arg.Length < 1) {
                    continue;
                }

                string methodName = string.Format("{0}.{1}.{2}()", name, GetType().Name, arg);
                Debug.LogFormat("ExecArgs: Executing {0}...", methodName);
                Invoke(arg, 0.0f);
                Debug.LogFormat("ExecArgs: Executed {0}", methodName);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnApplicationFocus(bool focus) {
            if (deepLog) {
                Debug.LogFormat("OnApplicationFocus({0})", focus);
            }

            if (!appControllerInputEnabled) {
                Debug.LogWarningFormat("OnApplicationFocus({0}): {1} input is disabled on {2} '{3}'. {1} will not change app focus state.", focus, nameof(AppController), nameof(GameObject), name);
                return;
            }

            if (focus) {
                if (gameplayInputEnabled) {
                    SetGameplayInputEnabled(true);
                    SetCursorLocked(true);
                }
            } else {
                SetGameplayInputEnabled(false);
                SetCursorLocked(false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAppControllerInputEnabled(bool enabled) {
            appControllerInputEnabled = enabled;

            if (deepLog && !appControllerInputEnabled) {
                Debug.LogFormat("{0} input is disabled on {1} '{2}'. {0} will not handle any input (game events or input events).", nameof(AppController), nameof(GameObject), name);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetGameplayInputEnabled(bool enabled) {
            if (!appControllerInputEnabled) {
                Debug.LogWarningFormat("{0} input is disabled on {1} '{2}'. {0} will not set gameplay input state.", nameof(AppController), nameof(GameObject), name);
                return;
            }

            gameplayInputEnabled = enabled;

            if (enabled) {
                if (_validGameplayInputEnabledGameEvent) {
                    gameplayInputEnabledGameEvent.SendGameEvent(name);
                }
            } else {
                if (_validGameplayInputDisabledGameEvent) {
                    gameplayInputDisabledGameEvent.SendGameEvent(name);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCursorLocked(bool locked) {
            if (!appControllerInputEnabled) {
                Debug.LogWarningFormat("{0} input is disabled on {1} '{2}'. {0} will not set cursor lock state.", nameof(AppController), nameof(GameObject), name);
                return;
            }

            cursorLocked = locked;
            CursorController.LockCursor(cursorLocked);

            if (locked) {
                if (_validCursorLockedGameEvent) {
                    cursorLockedGameEvent.SendGameEvent(name);
                }
            } else {
                if (_validCursorUnlockedGameEvent) {
                    cursorUnlockedGameEvent.SendGameEvent(name);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMenuEnabled(bool enabled) {
            if (!appControllerInputEnabled) {
                Debug.LogWarningFormat("{0} input is disabled on {1} '{2}'! {0} will not set menu state!", nameof(AppController), nameof(GameObject), name);
                return;
            }

            menuEnabled = enabled;

            if (enabled) {
                if (_validMenuEnabledGameEvent) {
                    menuEnabledGameEvent.SendGameEvent(name);
                }

                if (_validOnMenuEnabledByInputOrGameEvent) {
                    onMenuEnabledByInputOrGameEvent.Invoke();
                }
            } else {
                if (_validMenuDisabledGameEvent) {
                    menuDisabledGameEvent.SendGameEvent(name);
                }

                if (_validOnMenuDisabledByInputOrGameEvent) {
                    onMenuDisabledByInputOrGameEvent.Invoke();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void QuitApp() {
            if (Application.isEditor) {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif // UNITY_EDITOR
            } else {
                Application.Quit();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseApp(bool pause) {
#if UNITY_EDITOR
            if (Application.isEditor) {
                EditorApplication.isPaused = pause;
            }
#endif // UNITY_EDITOR
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            if (!appControllerInputEnabled) {
                return;
            }

            if (Input.GetKeyDown(quitAppKey)) {
                QuitApp();
            }

            if (Input.GetKeyDown(toggleGameplayInputEnabledKey)) {
                gameplayInputEnabled = !gameplayInputEnabled;
                SetGameplayInputEnabled(gameplayInputEnabled);
            }

            if (Input.GetKeyDown(toggleCursorLockedKey)) {
                cursorLocked = !cursorLocked;
                SetCursorLocked(cursorLocked);
            }

            if (Input.GetKeyDown(toggleMenuKey)) {
                menuEnabled = !menuEnabled;
                SetMenuEnabled(menuEnabled);

                if (menuEnabled) {
                    if (_validOnMenuEnabledByInput) {
                        onMenuEnabledByInput.Invoke();
                    }
                } else {
                    if (_validOnMenuDisabledByInput) {
                        onMenuDisabledByInput.Invoke();
                    }
                }
            }
        }
    }
}