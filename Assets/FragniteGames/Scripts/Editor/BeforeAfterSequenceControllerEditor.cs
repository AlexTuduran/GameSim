using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace FragniteGames {
    [CustomEditor(typeof(BeforeAfterSequenceController))]
    public class BeforeAfterSequenceControllerEditor : Editor {
        private BeforeAfterSequenceController _component {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return (BeforeAfterSequenceController)target;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            GUILayout.Label(string.Format("Sequence Time: {0:F3} Sec.", (float)_component.framesPerSequence / _component.framesPerSecond), EditorStyles.helpBox);
            GUILayout.Label(string.Format("Total Sequences Time: {0:F3} Sec.", (float)_component.framesPerSequence / _component.framesPerSecond * _component.NumSequences), EditorStyles.helpBox);
            GUILayout.Label(string.Format("Total Sequences Frames: {0} Frames", (float)_component.framesPerSequence * _component.NumSequences), EditorStyles.helpBox);

            bool temp = GUI.enabled;
            GUI.enabled = _component.RenderingStatus == BeforeAfterSequenceRenderingStatus.Ready;
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal(); {
                if (GUILayout.Button("Start Rendering")) {
                    _component.StartRendering();
                }

                if (GUILayout.Button("Stop Rendering")) {
                    _component.StopRendering();
                }
            } GUILayout.EndHorizontal();
            GUI.enabled = temp;

            if (_component.RenderingStatus != BeforeAfterSequenceRenderingStatus.Ready) {
                GUILayout.Label(
                    "The component is not ready for rendering. Make sure all component's fields are valid.\n\n" +
                    "Reason: " + BeforeAfterSequenceController.BeforeAfterSequenceRenderingStatusToString(_component.RenderingStatus) + ".",
                    EditorStyles.helpBox
                );
            }
        }
    }
}