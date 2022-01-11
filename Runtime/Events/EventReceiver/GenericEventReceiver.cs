using UnityEngine;
using UnityEngine.Events;

namespace Daniell.Runtime.Events
{
    /// <summary>
    /// Handles creating dynamic unity event types and dispatching an Event
    /// </summary>
    /// <typeparam name="TValue">Type of the event argument</typeparam>
    /// <typeparam name="TEvent">Type of the event</typeparam>
    public class GenericEventReceiver<TValue, TEvent> : EventReceiver where TEvent : GenericScriptableEvent<TValue>
    {
#if UNITY_EDITOR
        public override ScriptableEvent Event => _event;
#endif

        [System.Serializable]
        public class GenericUnityEvent : UnityEvent<TValue> { }

        [SerializeField]
        [Tooltip("Event linked to this receiver")]
        private TEvent _event;

        [SerializeField]
        [Tooltip("Response called when the linked event is raised")]
        private GenericUnityEvent _response;
        private void OnEnable()
        {
            _event.AddListener(OnEventReceived);
        }

        private void OnDisable()
        {
            _event.RemoveListener(OnEventReceived);
        }

        private void OnEventReceived(TValue value)
        {
            _response.Invoke(value);
        }
    }
}