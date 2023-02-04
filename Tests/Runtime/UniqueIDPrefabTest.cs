using UnityEngine;

namespace Ikonoclast.PropertyAttributes.Tests
{
    public class UniqueIDPrefabTest : MonoBehaviour
    {
        [UniqueIdentifier]
        public string stringTest;

        [UniqueIdentifier]
        public MonoBehaviour monoBehaviourTest;
    }
}