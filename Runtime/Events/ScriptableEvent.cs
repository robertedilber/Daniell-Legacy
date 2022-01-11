using System;
using UnityEngine;

namespace Daniell.Runtime.Events
{
    /// <summary>
    /// Event that can be saved and referenced
    /// </summary>
    public abstract class ScriptableEvent : ScriptableObject
    {
        /// <summary>
        /// Base menu path
        /// </summary>
        public const string MENU_PATH_BASE = "Events/";

        /// <summary>
        /// Internal Event
        /// </summary>
        private event Action OnEventRaised;

#if UNITY_EDITOR

        /// <summary>
        /// Description of the event
        /// </summary>
        public string Description => _description;

        [SerializeField]
        [Tooltip("Description of the event")]
        private string _description;
#endif

        /// <summary>
        /// Raise void event
        /// </summary>
        public virtual void Raise()
        {
            OnEventRaised?.Invoke();
        }

        /// <summary>
        /// Add a new listener to the event
        /// </summary>
        /// <param name="action">Delegate to subscribe</param>
        public void AddListener(Action action)
        {
            OnEventRaised += action;
        }

        /// <summary>
        /// Remove a listener from the event
        /// </summary>
        /// <param name="action">Delegate to unsubscribe</param>
        public void RemoveListener(Action action)
        {
            OnEventRaised -= action;
        }
    }
}
