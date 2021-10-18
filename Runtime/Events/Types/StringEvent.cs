using UnityEngine;

namespace Daniell.EventSystem.Scriptable
{
    [CreateAssetMenu(fileName = " New String Event", menuName = MENU_PATH_BASE + "String")]
    public class StringEvent : ScriptableEvent<string> { }
}