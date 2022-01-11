using UnityEngine;

namespace Daniell.Runtime.Events
{
    /// <summary>
    /// Base class for receiving an event
    /// </summary>
    public abstract class EventReceiver : MonoBehaviour
    {
#if UNITY_EDITOR
        public abstract ScriptableEvent Event { get; }
#endif
    }
}