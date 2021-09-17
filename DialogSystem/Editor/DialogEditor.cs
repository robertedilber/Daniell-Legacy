using Daniell.DialogSystem;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Dependencies.Daniell.DialogSystem.Editor
{
    public class DialogEditor : EditorWindow
    {
        private const int MAIN_GROUP_TOP_PADDING = 30;
        private const int NODE_WIDTH = 300;
        private const int NODE_HEIGHT = 150;

        private Dialog _dialog;

        #region Editor Calls

        [MenuItem("Daniell/Dialog Editor")]
        private static void Open()
        {
            // Get existing open window or if none, make a new one:
            DialogEditor window = GetWindow<DialogEditor>();
            window.Show();
        }

        private void OnGUI()
        {
            // Dialog To Edit
            _dialog = (Dialog)EditorGUILayout.ObjectField("Test", _dialog, typeof(Dialog), false);

            // Do not show editor if the dialog is null
            if (_dialog == null)
            {
                return;
            }

            // Begin main window group
            Rect mainGroupRect = new Rect(0, MAIN_GROUP_TOP_PADDING, position.width, position.height);
            GUI.BeginGroup(mainGroupRect);

            // Draw main group backround
            DrawBackgroundForControl(mainGroupRect.width, mainGroupRect.height, 0.1f);

            // Draw Nodes
            DrawLineNode(_dialog.DialogLine, 20, 20);

            GUI.EndGroup();
        }

        #endregion

        #region Draw Nodes

        /// <summary>
        /// Draw a DialogLine Node
        /// </summary>
        /// <param name="dialogLine">DialogLine to be drawn</param>
        /// <param name="x">X position of the node</param>
        /// <param name="y">Y position of the node</param>
        private void DrawLineNode(DialogLine dialogLine, int x, int y)
        {
            // Begin a new group
            GUI.BeginGroup(new Rect(x, y, NODE_WIDTH, NODE_HEIGHT));

            // Draw background
            DrawBackgroundForControl(NODE_WIDTH, NODE_HEIGHT, 0.5f);

            // Draw Dialog Line
            dialogLine.Character = (Character)EditorGUI.ObjectField(new Rect(20, 20, NODE_WIDTH - 40, 20),"", dialogLine.Character, typeof(Character), false);
            dialogLine.Text = EditorGUI.TextArea(new Rect(20, 50, NODE_WIDTH - 40, 80), dialogLine.Text);

            GUI.EndGroup();
        }

        /// <summary>
        /// Draw a DialogChoice Node
        /// </summary>
        /// <param name="dialogChoice">DialogChoice to be drawn</param>
        /// <param name="x">X position of the node</param>
        /// <param name="y">Y position of the node</param>
        private void DrawChoiceNode(DialogChoice dialogChoice, int x, int y)
        {

        }

        #endregion

        #region Helpers

        private void DrawBackgroundForControl(float controlWidth, float controlHeight, float brigthness)
        {
            DrawBackgroundForControl(controlWidth, controlHeight, new Color(brigthness, brigthness, brigthness));
        }

        private void DrawBackgroundForControl(float controlWidth, float controlHeight, Color color)
        {
            EditorGUI.DrawRect(new Rect(0, 0, controlWidth, controlHeight), color);
        }

        #endregion
    }
}