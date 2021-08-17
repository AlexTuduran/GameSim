﻿//
// Custom Attributes
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace OmniSARTechnologies.Helper {
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class DisplayNameAttribute : PropertyAttribute {
        public readonly string displayName;

        public DisplayNameAttribute(string displayName) {
            this.displayName = displayName;
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class WhatsThisAttribute : PropertyAttribute {
        public readonly string message;
#if UNITY_EDITOR
        public readonly MessageType messageType;
#endif // UNITY_EDITOR

#if UNITY_EDITOR
        public WhatsThisAttribute(string message, MessageType messageType = MessageType.None) {
            this.message = message;
            this.messageType = messageType;
        }
#else // UNITY_EDITOR
        public WhatsThisAttribute(string message, params object[] unused) {
            this.message = message;
        }
#endif // UNITY_EDITOR
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DocsAttribute : PropertyAttribute {
        public readonly string text;

        public DocsAttribute(string text = default(string)) {
            this.text = text;
        }
    }
}
