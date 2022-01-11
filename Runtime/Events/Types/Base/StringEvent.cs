using UnityEngine;

namespace Daniell.Runtime.Events
{
    [CreateAssetMenu(fileName = " New String Event", menuName = MENU_PATH_BASE + "String")]
    public class StringEvent : GenericScriptableEvent<string> { }
}