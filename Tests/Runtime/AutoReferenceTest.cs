using UnityEngine;

namespace Ikonoclast.PropertyAttributes.Tests
{
    public class AutoReferenceTest : MonoBehaviour
    {
        [AutoReference]
        public float test0;

        [AutoReference]
        public AutoReferenceWithTypeRestrictionTest test1;

        [AutoReference]
        [TypeRestriction(typeof(IAutoReferenceWithTypeRestrictionTest))]
        public Component test2;

        [AutoReference(AutoReferenceMethod.Scene, nameof(test3))]
        [TypeRestriction(typeof(IAutoReferenceWithTypeRestrictionTest))]
        public Component test3;

        [AutoReference(AutoReferenceMethod.Child, nameof(test4))]
        [TypeRestriction(typeof(IAutoReferenceWithTypeRestrictionTest))]
        public Component test4;

        [AutoReference(AutoReferenceMethod.Parent, nameof(test5))]
        [TypeRestriction(typeof(IAutoReferenceWithTypeRestrictionTest))]
        public Component test5;

        [field: SerializeField]
        [field: AutoReference(
            AutoReferenceMethod.Self |
            AutoReferenceMethod.Child |
            AutoReferenceMethod.Parent |
            AutoReferenceMethod.Scene, nameof(Test6))]
        public AutoReferenceWithTypeRestrictionTest Test6
        {
            get;
            set;
        }

        [AutoReference]
        public Component test7;

        [AutoReference]
        public Component test8;

        [AutoReference]
        public ScriptableObject test9;
    }
}