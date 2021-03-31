using UnityEngine;

namespace Daniell.EventSystem.Scriptable
{
    [CreateAssetMenu(fileName = "New Float Event", menuName = MENU_PATH_BASE + "Float")]
    public class FloatEvent : ScriptableEvent<float> { }
}