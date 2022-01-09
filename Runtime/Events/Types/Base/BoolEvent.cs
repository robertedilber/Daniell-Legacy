using UnityEngine;

namespace Daniell.Runtime.Events
{
    [CreateAssetMenu(fileName = "New Bool Event", menuName = MENU_PATH_BASE + "Bool")]
    public class BoolEvent : ScriptableEvent<bool> { }
}