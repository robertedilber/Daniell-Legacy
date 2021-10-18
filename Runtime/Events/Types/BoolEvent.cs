using UnityEngine;

namespace Daniell.EventSystem.Scriptable
{
    [CreateAssetMenu(fileName = "New Bool Event", menuName = MENU_PATH_BASE + "Bool")]
    public class BoolEvent : ScriptableEvent<bool> { }
}