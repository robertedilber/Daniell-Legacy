using UnityEngine;

namespace Daniell.Runtime.Systems.Input
{
    /// <summary>
    /// Handles registering/unregistering and updating input events
    /// </summary>
    public class InputEventDriver : MonoBehaviour
    {
        /* ==========================
         * > Serialized Fields
         * -------------------------- */

        [SerializeField]
        [Tooltip("List of all the input events that need to be relayed")]
        private InputRelay[] _inputEvents;


        /* ==========================
         * > Methods
         * -------------------------- */

        #region Unity Messages

        private void OnEnable()
        {
            for (int i = 0; i < _inputEvents.Length; i++)
            {
                _inputEvents[i].Register();
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _inputEvents.Length; i++)
            {
                _inputEvents[i].Unregister();
            }
        }

        private void Update()
        {
            for (int i = 0; i < _inputEvents.Length; i++)
            {
                _inputEvents[i].Update();
            }
        }

        #endregion
    }
}