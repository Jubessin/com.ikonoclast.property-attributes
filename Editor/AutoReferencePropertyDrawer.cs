using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Ikonoclast.PropertyAttributes.Editor
{
    using TypeRestriction = TypeRestrictionAttribute;

    [CustomPropertyDrawer(typeof(AutoReferenceAttribute))]
    internal sealed class AutoReferencePropertyDrawer : PropertyDrawer
    {
        private bool hideInInspectorCache;
        private string nameInHierarchyCache;
        private MonoBehaviour targetObjectCache;
        private AutoReferenceMethod autoReferenceMethodCache;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (targetObjectCache == null)
            {
                var attr = attribute as AutoReferenceAttribute;

                hideInInspectorCache = attr.hideInInspector;
                nameInHierarchyCache = attr.nameInHierarchy;
                autoReferenceMethodCache = attr.autoReferenceMethod;
                targetObjectCache = (MonoBehaviour)property.serializedObject.targetObject;
            }

            if (hideInInspectorCache == false)
            {
                EditorGUI.PropertyField(position, property, label);
            }

            var inspectedType = fieldInfo.FieldType;

            if (!inspectedType.IsClass)
            {
                Debug.LogError(
                    $"{nameof(AutoReferenceAttribute)} cannot be used with non-class types.");

                return;
            }

            if (inspectedType.IsSubclassOf(typeof(ScriptableObject)))
            {
                Debug.LogError(
                    $"{nameof(AutoReferenceAttribute)} cannot be used with {nameof(ScriptableObject)} types.");

                return;
            }

            if (property.objectReferenceValue)
                return;

            var componentType = fieldInfo.GetCustomAttribute<TypeRestriction>()?.Type ?? inspectedType;

            if (!(componentType.IsInterface ||
                componentType == typeof(Component) ||
                componentType.IsSubclassOf(typeof(Component))))
            {
                Debug.LogError(
                    $"{nameof(AutoReferenceAttribute)} can only be used with " +
                        $"interfaces (via {nameof(TypeRestriction)}) or {nameof(Component)} subclasses.");

                return;
            }

            if (property.serializedObject != null)
            {
                if (property.serializedObject.isEditingMultipleObjects)
                    throw new Exception($"Editing multiple objects, " +
                        $"with null reference on field with {nameof(AutoReferenceAttribute)} applied.");

                if (autoReferenceMethodCache.HasFlag(AutoReferenceMethod.Self))
                {
                    if (targetObjectCache && targetObjectCache.TryGetComponent(componentType, out var refValue))
                    {
                        property.objectReferenceValue = refValue;
                        return;
                    }
                }

                if (autoReferenceMethodCache.HasFlag(AutoReferenceMethod.Child))
                {
                    if (targetObjectCache != null)
                    {
                        Component refValue = null;

                        Predicate<Transform> TryGetComponent;

                        if (nameInHierarchyCache != null)
                        {
                            TryGetComponent = (t) => t.name == nameInHierarchyCache
                                                     && t.TryGetComponent(componentType, out refValue);
                        }
                        else
                        {
                            TryGetComponent = (t) => t.TryGetComponent(componentType, out refValue);
                        }

                        foreach (Transform child in targetObjectCache.transform)
                            if (TryGetComponent(child))
                                break;

                        if (refValue)
                        {
                            property.objectReferenceValue = refValue;

                            return;
                        }
                    }
                }

                if (autoReferenceMethodCache.HasFlag(AutoReferenceMethod.Parent))
                {
                    if (targetObjectCache != null)
                    {
                        Component refValue = null;

                        Predicate<Transform> TryGetComponent;

                        if (nameInHierarchyCache != null)
                        {
                            TryGetComponent = (t) => t.name == nameInHierarchyCache
                                                     && t.TryGetComponent(componentType, out refValue);
                        }
                        else
                        {
                            TryGetComponent = (t) => t.TryGetComponent(componentType, out refValue);
                        }

                        for (var parent = targetObjectCache.transform.parent; parent != null; parent = parent.parent)
                            if (TryGetComponent(parent))
                                break;

                        if (refValue)
                        {
                            property.objectReferenceValue = refValue;

                            return;
                        }
                    }
                }

                if (autoReferenceMethodCache.HasFlag(AutoReferenceMethod.Scene))
                {
                    if (nameInHierarchyCache != null)
                    {
                        var go = GameObject.Find(nameInHierarchyCache);

                        if (go && go.TryGetComponent(componentType, out var refValue))
                        {
                            property.objectReferenceValue = refValue;
                            return;
                        }
                    }
                }

                Debug.LogWarning(
                    $"Unable to get component of type {componentType} on object {Selection.activeGameObject}.");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            hideInInspectorCache
                ? -EditorGUIUtility.standardVerticalSpacing
                : base.GetPropertyHeight(property, label);
    }
}
