using UnityEngine;

namespace Daniell.Runtime.Events
{
    [CreateAssetMenu(fileName = "New Int Event", menuName = MENU_PATH_BASE + "Int")]
    public class IntEvent : GenericScriptableEvent<int> { }
}