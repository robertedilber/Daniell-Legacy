using Daniell.Runtime.Save;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.Save
{
    /// <summary>
    /// Custom Drawer for SaveableEntities
    /// </summary>
    [CustomPropertyDrawer(typeof(SaveableEntity))]
    public class SaveableEntityDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Find the save ID of the target
            SerializedProperty saveID = property.FindPropertyRelative("_saveID");

            // Create the property
            EditorGUI.BeginProperty(position, label, property);

            // Display error if ID is undefined
            if (saveID.stringValue == SaveableEntity.UNDEFINED_SAVE_ID)
            {
                EditorGUILayout.HelpBox("Invalid Save ID. Please Generate a new ID.", MessageType.Error);
            }

            // Show button to generate ID
            if (GUI.Button(new Rect(position.x, position.y, 100, 18), "Generate ID"))
            {
                // Generate ID
                saveID.stringValue = GUID.Generate().ToString();
            }

            // Show current ID
            EditorGUI.LabelField(new Rect(position.x + 102, position.y, position.width - 102, 18), "Save ID: " + saveID.stringValue, EditorStyles.helpBox);

            EditorGUI.EndProperty();
        }
    }
}