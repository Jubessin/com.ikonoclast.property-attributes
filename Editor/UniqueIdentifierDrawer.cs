using System;
using UnityEditor;
using UnityEngine;

namespace Ikonoclast.PropertyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(UniqueIdentifierAttribute))]
    internal sealed class UniqueIdentifierPropertyDrawer : PropertyDrawer
    {
        private const string UnpackPrefabUndoGroupName = "Unpack Prefab";

        #region Fields

        private static bool
            isSubscribed = false,
            shouldRegenerateID = false;

        #endregion

        #region Methods

        private void OnHierarchyChanged()
        {
            if (Undo.GetCurrentGroupName().Contains(UnpackPrefabUndoGroupName))
            {
                shouldRegenerateID = true;
            }
        }

        private void DrawLabelField(Rect position, SerializedProperty prop, GUIContent label) =>
            EditorGUI.LabelField(position, label, new GUIContent(prop.stringValue));

        #endregion

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            if (!isSubscribed)
            {
                EditorApplication.hierarchyChanged += OnHierarchyChanged;
                isSubscribed = true;
            }

            if (prop.propertyType != SerializedPropertyType.String)
            {
                Debug.LogError(
                    $"{nameof(UniqueIdentifierAttribute)} can only be used with strings.");

                EditorGUI.PropertyField(position, prop, label);

                return;
            }

            // Generate a unique ID, defaults to an empty string if nothing has been serialized yet
            if (prop.stringValue == string.Empty || prop.stringValue == null)
            {
                prop.stringValue = Guid.NewGuid().ToString();
            }

            if (shouldRegenerateID)
            {
                if (prop.serializedObject.isEditingMultipleObjects)
                {
                    foreach (var @object in prop.serializedObject.targetObjects)
                    {
                        var serializedObject = new SerializedObject(@object);

                        serializedObject.FindProperty(prop.propertyPath).stringValue = Guid.NewGuid().ToString();

                        serializedObject.ApplyModifiedPropertiesWithoutUndo();
                    }
                }
                else
                {
                    prop.stringValue = Guid.NewGuid().ToString();
                }

                shouldRegenerateID = false;

                return;
            }

            var textFieldPosition = position;

            textFieldPosition.height = 16;

            // Place a label so it can't be edited by accident
            DrawLabelField(textFieldPosition, prop, label);
        }
    }
}
