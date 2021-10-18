using Daniell.EventSystem.Components;
using System;
using UnityEngine;

namespace Daniell.EventSystem.Scriptable
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
        /// Is this event currently active?
        /// Used to retrieve current frame value
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Internal Event
        /// </summary>
        private event Action _event;

        /// <summary>
        /// Raise void event
        /// </summary>
        public void Raise()
        {
            IsActive = true;
            _event?.Invoke();
            EventTracker.RegisterEventForReset(this);
        }

        /// <summary>
        /// Reset value retriever
        /// </summary>
        public virtual void Reset()
        {
            IsActive = false;
        }

        /// <summary>
        /// Add a new listener to the event
        /// </summary>
        /// <param name="action">Delegate to subscribe</param>
        /// <param name="retrieveLast">Should this event be triggered if subscribed at the same frame?</param>
        public void AddListener(Action action, bool retrieveLast = true)
        {
            _event += action;
            if (retrieveLast && IsActive)
                action?.Invoke();
        }

        /// <summary>
        /// Remove a listener from the event
        /// </summary>
        /// <param name="action">Delegate to unsubscribe</param>
        public void RemoveListener(Action action) => _event -= action;
    }
}
