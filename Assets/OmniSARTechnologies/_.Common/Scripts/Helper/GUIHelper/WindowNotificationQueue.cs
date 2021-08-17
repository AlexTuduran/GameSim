//
// Window Notification Queue
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace OmniSARTechnologies.Helper {
    public class WindowNotification {
        public int id;
        public GUIContent content;
        public Predicate<object> mustShow;
        public Func<WindowNotificationQueue, WindowNotification, object, object> onHideAction;
        public object obj;

        public WindowNotification(
            int id,
            GUIContent content,
            Predicate<object> mustShow,
            Func<WindowNotificationQueue, WindowNotification, object, object> onHideAction,
            object obj = null
        ) {
            this.id = id;
            this.content = content;
            this.mustShow = mustShow;
            this.onHideAction = onHideAction;
            this.obj = obj;
        }
    }

    public class WindowNotificationQueue {
#if UNITY_EDITOR
        private EditorWindow m_EditorWindow = null;
        private Queue<WindowNotification> m_Queue = null;
        private bool m_NotificationRemoved = false;

        public WindowNotificationQueue(EditorWindow editorWindow) {
            m_EditorWindow = editorWindow;
            m_Queue = new Queue<WindowNotification>();
        }

        public void Enqueue(WindowNotification notification) {
            m_Queue.Enqueue(notification);
        }

        public WindowNotification Dequeue() {
            return m_Queue.Dequeue();
        }

        public WindowNotification Peek() {
            return m_Queue.Peek();
        }

        public void Execute() {
            WindowNotification notification = m_Queue.Peek();
            if (null == notification) {
                return;
            }

            if (null != notification.mustShow) {
                if (notification.mustShow(notification.obj)) {
                    ShowNotification(notification);
                    return;
                }
            }

            RemoveNotification(notification);
            notification = m_Queue.Dequeue();

            if (null == notification) {
                return;
            }

            if (null != notification.onHideAction) {
                notification.onHideAction(this, notification, notification.obj);
            }
        }

        public void ShowNotification(WindowNotification notification) {
            if (null == notification) {
                return;
            }

            if (!m_EditorWindow) {
                return;
            }

            m_EditorWindow.ShowNotification(notification.content);
            m_NotificationRemoved = false;
        }

        public void RemoveNotification(WindowNotification notification) {
            if (m_NotificationRemoved) {
                return;
            }

            if (!m_EditorWindow) {
                return;
            }

            m_EditorWindow.RemoveNotification();
            m_NotificationRemoved = true;
        }
#endif // UNITY_EDITOR
    }
}
