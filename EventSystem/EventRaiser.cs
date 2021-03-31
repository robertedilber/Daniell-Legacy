using UnityEngine;
using UnityEngine.Events;

namespace Daniell.EventSystem.Components
{
    /// <summary>
    /// Raise an event on start
    /// </summary>
    public class EventRaiser : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _response = null;

        private void Start()
        {
            _response.Invoke();
        }
    }
}
