using System;

namespace Daniell.Runtime.Events
{
    /// <summary>
    /// Generic event that can be saved and referenced
    /// </summary>
    /// <typeparam name="T">Event value type</typeparam>
    public abstract class ScriptableEvent<T> : ScriptableEvent
    {
        /// <summary>
        /// Internal event
        /// </summary>
        private event Action<T> _event;

        /// <summary>
        /// Curent active value
        /// </summary>
        private T _activeValue;

        /// <summary>
        /// Raise event with value of type T
        /// </summary>
        /// <param name="value">Value of the event</param>
        public void Raise(T value)
        {
            IsActive = true;
            _activeValue = value;
            _event?.Invoke(value);

            // If the event tracker exists, find register this event for reset
            if (EventTracker.IsInstanceReady)
            {
                EventTracker.RegisterEventForReset(this);
            }
        }

        /// <summary>
        /// Reset value retriever
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _activeValue = default;
        }

        /// <summary>
        /// Add a new listener to the event
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="retrieveLast">Should this event be triggered if subscribed at the same frame?</param>
        public void AddListener(Action<T> action, bool retrieveLast = true)
        {
            _event += action;
            if (retrieveLast && IsActive)
                action?.Invoke(_activeValue);
        }

        /// <summary>
        /// Remove a listener from the event
        /// </summary>
        /// <param name="action">Delegate to unsubscribe</param>
        public void RemoveListener(Action<T> action) => _event -= action;
    }
}
