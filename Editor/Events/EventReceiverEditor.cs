using Daniell.Runtime.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Daniell.Editor.Events
{
    [CustomEditor(typeof(EventReceiver), true)]
    public class EventReceiverEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var t = (EventReceiver)target;

            string text = "";
            string color = "#97f229";
            GUIStyle newStyle = new GUIStyle();
            newStyle.richText = true;
            newStyle.fontSize = 16;

            if (t.Event != null)
            {
                if (!string.IsNullOrEmpty(t.Event.Description))
                {
                    text = t.Event.Description;
                }
                else
                {
                    text = "No Description...";
                }
            }
            else
            {
                color = "#f43a72";
                text = "No event assigned";
            }

            EditorGUILayout.LabelField($"<color={color}><b>{text}</b></color>", newStyle);

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}