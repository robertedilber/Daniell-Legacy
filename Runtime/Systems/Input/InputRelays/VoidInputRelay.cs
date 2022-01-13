using Daniell.Runtime.Systems.Events;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Daniell.Runtime.Systems.Input
{
    /// <summary>
    /// Generic form of Input Relay. Raises events without values on Input Action callbacks
    /// </summary>
    /// <typeparam name="TEvent">Type of the event</typeparam>
    public abstract class VoidInputRelay<TEvent> : InputRelay where TEvent : ScriptableEvent
    {
        /* ==========================
         * > Serialized Fields
         * -------------------------- */

        [Header("Events")]

        [SerializeField]
        [Tooltip("Event fired when the input is pressed (guaranteed single frame)")]
        protected TEvent _onPressedEvent;

        [SerializeField]
        [Tooltip("Event fired while the input is pressed (can be multiple frame if Always Update is true)")]
        protected TEvent _onPerformedEvent;

        [SerializeField]
        [Tooltip("Event fired while the input is released (guaranteed single frame)")]
        protected TEvent _onReleasedEvent;


        /* ==========================
         * > Methods
         * -------------------------- */

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventUpdated()
        {
            ScriptableEvent.RaiseEvent(_onPerformedEvent);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventStarted(CallbackContext callbackContext)
        {
            ScriptableEvent.RaiseEvent(_onPressedEvent);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventPerformed(CallbackContext callbackContext)
        {
            ScriptableEvent.RaiseEvent(_onPerformedEvent);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventCanceled(CallbackContext callbackContext)
        {
            ScriptableEvent.RaiseEvent(_onReleasedEvent);
        }
    }
}