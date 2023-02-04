using System;
using UnityEngine;

namespace Ikonoclast.PropertyAttributes
{
    [Flags]
    public enum AutoReferenceMethod
    {
        /// <summary>
        /// Search for the reference on its game object.
        /// </summary>
        Self = 1 << 0,

        /// <summary>
        /// Search for the reference on child game objects (nameInHierarchy optional).
        /// </summary>
        Child = 1 << 1,

        /// <summary>
        /// Search for the reference on parent game objects (nameInHierarchy optional).
        /// </summary>
        Parent = 1 << 2,

        /// <summary>
        /// Search for the reference in the scene (requires nameInHierarchy).
        /// </summary>
        Scene = 1 << 3,
    }

    /// <summary>
    /// Searches for and automatically assigns a reference to the given type if found.
    /// </summary>
    /// ---
    /// Note: When used in conjunction with the TypeRestricion attribute, the reference can be 
    /// inspected in the debug inspector via right-clicking and selecting the 'properties' option.
    public sealed class AutoReferenceAttribute : PropertyAttribute, IPropertyAttribute
    {
        #region Properties

        public bool hideInInspector
        {
            get;
        } = true;
        public string nameInHierarchy
        {
            get;
        } = null;
        public AutoReferenceMethod autoReferenceMethod
        {
            get;
        } = AutoReferenceMethod.Self;

        #endregion

        #region Constructors

        public AutoReferenceAttribute(bool hideInInspector = true)
        {
            this.hideInInspector = hideInInspector;
        }

        public AutoReferenceAttribute(
            string nameInHierarchy,
            bool hideInInspector = true,
            AutoReferenceMethod autoReferenceMethod = AutoReferenceMethod.Scene)
        {
            if (autoReferenceMethod == AutoReferenceMethod.Scene && nameInHierarchy == null)
                throw new AutoReferenceException<AutoReferenceAttribute>($"cannot be used with " +
                    $"{nameof(AutoReferenceMethod)}.{nameof(AutoReferenceMethod.Scene)} and null {nameof(nameInHierarchy)}.");

            this.nameInHierarchy = nameInHierarchy;
            this.hideInInspector = hideInInspector;
            this.autoReferenceMethod = autoReferenceMethod;
        }

        public AutoReferenceAttribute(
            AutoReferenceMethod autoReferenceMethod,
            string nameInHierarchy = null,
            bool hideInInspector = true) : this(nameInHierarchy, hideInInspector, autoReferenceMethod) { }

        #endregion
    }

    public sealed class AutoReferenceException<T> : Exception where T : class
    {
        #region Properties

        public string message
        {
            get;
        } = null;

        #endregion

        #region Constructors

        public AutoReferenceException() { }

        public AutoReferenceException(string message) : base(message)
        {
            this.message = message;
        }

        #endregion

        #region Exception Implementations

        public override string Message =>
            message == null
                ? $"{typeof(T).Name}"
                : $"{typeof(T).Name} : {message}";

        #endregion
    }
}

