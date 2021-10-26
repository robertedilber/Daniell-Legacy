using Daniell.Runtime.Singletons;
using System.Collections;
using UnityEngine;

namespace Daniell.Runtime.Events
{
    /// <summary>
    /// Allows events to retrieve their values from the curent frame
    /// </summary>
    public class EventTracker : SingletonMonoBehaviour<EventTracker>
    {
        /// <summary>
        /// Register an event to be reset at the end of a frame
        /// </summary>
        /// <param name="scriptableEvent">Event to be reset</param>
        public static void RegisterEventForReset(ScriptableEvent scriptableEvent)
        {
            DelayedInstanceCall(instance =>
            {
                instance.StartCoroutine(ResetOnEndOfFrame());
            });

            IEnumerator ResetOnEndOfFrame()
            {
                yield return new WaitForEndOfFrame();
                scriptableEvent.Reset();
            }
        }
    }
}