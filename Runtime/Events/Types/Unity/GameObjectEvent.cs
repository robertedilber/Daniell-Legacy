using UnityEngine;

namespace Daniell.Runtime.Events
{
    [CreateAssetMenu(fileName = "New GameObject Event", menuName = MENU_PATH_BASE + "GameObject")]
    public class GameObjectEvent : GenericScriptableEvent<GameObject> { }
}