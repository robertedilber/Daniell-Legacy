using UnityEngine;

namespace Daniell.Runtime.Events
{
    [CreateAssetMenu(fileName = "New Float Event", menuName = MENU_PATH_BASE + "Float")]
    public class FloatEvent : GenericScriptableEvent<float> { }
}
