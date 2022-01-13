using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Daniell.Runtime.Systems.Input
{
    /// <summary>
    /// Base Input Relay class for registering callbacks from an Input Action
    /// </summary>
    public abstract class InputRelay : ScriptableObject
    {
        /* ==========================
         * > Serialized Fields
         * -------------------------- */

        [Header("Input Settings")]

        [SerializeField]
        [Tooltip("Input Action that this event will listen to")]
        protected InputActionReference _inputActionReference;

        [SerializeField]
        [Tooltip("Should this event be sent every frame?")]
        private bool _alwaysUpdate;

        
        /* ==========================
         * > Protected Fields
         * -------------------------- */

        protected InputAction _inputAction;


        /* ==========================
         * > Methods
         * -------------------------- */

        #region Input Update

        /// <summary>
        /// Register to Input Action Events
        /// </summary>
        public void Register()
        {
            if (!_inputActionReference.asset.enabled)
            {
                _inputActionReference.asset.Enable();
            }

            _inputAction = _inputActionReference.action;

            _inputAction.started += EventStarted;
            _inputAction.canceled += EventCanceled;

            if (!_alwaysUpdate)
            {
                _inputAction.performed += EventPerformed;
            }
        }

        /// <summary>
        /// Unregister from Input Action Events
        /// </summary>
        public void Unregister()
        {
            _inputAction.started -= EventStarted;
            _inputAction.canceled -= EventCanceled;

            if (!_alwaysUpdate)
            {
                _inputAction.performed -= EventPerformed;
            }
        }

        /// <summary>
        /// Update the input state
        /// </summary>
        public void Update()
        {
            if (_alwaysUpdate)
            {
                EventUpdated();
            }
        }

        #endregion

        #region Abstract Implementation

        /// <summary>
        /// Called when this event is updated
        /// </summary>
        protected abstract void EventUpdated();

        /// <summary>
        /// Called when the input action sends the Started callback
        /// </summary>
        /// <param name="callbackContext">Input Action value</param>
        protected abstract void EventStarted(CallbackContext callbackContext);

        /// <summary>
        /// Called when the input action sends the Performed callback
        /// </summary>
        /// <param name="callbackContext">Input Action value</param>
        protected abstract void EventPerformed(CallbackContext callbackContext);

        /// <summary>
        /// Called when the input action sends the Canceled callback
        /// </summary>
        /// <param name="callbackContext">Input Action value</param>
        protected abstract void EventCanceled(CallbackContext callbackContext);

        #endregion
    }
}