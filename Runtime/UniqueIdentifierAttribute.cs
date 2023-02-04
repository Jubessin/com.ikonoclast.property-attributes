using System;
using UnityEngine;

namespace Ikonoclast.PropertyAttributes
{
    /// <summary>
    /// Generates a non-editable <see cref="Guid"/> for a string property.
    /// </summary>
    public sealed class UniqueIdentifierAttribute : PropertyAttribute, IPropertyAttribute { }
}
