using UnityEditor;
using UnityEngine;

namespace Ikonoclast.PropertyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TypeRestrictionAttribute))]
    internal sealed class TypeRestrictionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                Debug.LogError(
                    $"{nameof(TypeRestrictionAttribute)} is only usable on " +
                        $"{SerializedPropertyType.ObjectReference} types.");

                return;
            }

            EditorGUI.ObjectField(position, property, (attribute as TypeRestrictionAttribute).Type, label);
        }
    }
}
