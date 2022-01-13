using Daniell.Runtime.Systems.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Daniell.Runtime.Systems.Input
{
    /// <summary>
    /// Generic Input Relay using custom value. Raise events that have a value parameter on Input Action callback.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event</typeparam>
    /// <typeparam name="TValue">Type of value that should be read from the input</typeparam>
    public abstract class ValueInputRelay<TEvent, TValue> : VoidInputRelay<TEvent> 
        where TEvent : GenericScriptableEvent<TValue> 
        where TValue : struct
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventUpdated()
        {
            ScriptableEvent.RaiseEvent(_onPerformedEvent, GetValue(_inputActionReference.action));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventStarted(CallbackContext callbackContext)
        {
            ScriptableEvent.RaiseEvent(_onPressedEvent, GetValue(callbackContext));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventPerformed(CallbackContext callbackContext)
        {
            ScriptableEvent.RaiseEvent(_onPerformedEvent, GetValue(callbackContext));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void EventCanceled(CallbackContext callbackContext)
        {
            ScriptableEvent.RaiseEvent(_onReleasedEvent, GetValue(callbackContext));
        }

        private TValue GetValue(CallbackContext callbackContext)
        {
            return GetValue(callbackContext.action);
        }

        private TValue GetValue(InputAction inputAction)
        {
            return inputAction.ReadValue<TValue>();
        }
    }
}