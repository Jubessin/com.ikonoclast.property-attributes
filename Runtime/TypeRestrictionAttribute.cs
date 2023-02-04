using System;
using UnityEngine;

namespace Ikonoclast.PropertyAttributes
{
    /// <summary>
    /// Restricts property type within the inspector.
    /// </summary>
    public sealed class TypeRestrictionAttribute : PropertyAttribute, IPropertyAttribute
    {
        #region Properties

        public Type Type
        {
            get;
        }

        #endregion

        #region Constructors

        public TypeRestrictionAttribute(Type Type)
        {
            this.Type = Type;
        }

        #endregion
    }
}
