using UnityEngine;

namespace Daniell.EventSystem.Scriptable
{
    [CreateAssetMenu(fileName = "New Int Event", menuName = MENU_PATH_BASE + "Int")]
    public class IntEvent : ScriptableEvent<int> { }
}