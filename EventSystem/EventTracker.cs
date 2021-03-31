using Daniell.EventSystem.Scriptable;
using System.Collections;
using UnityEngine;

namespace Daniell.EventSystem.Components
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
            if (Instance != null)
            {
                Instance.StartCoroutine(ResetOnEndOfFrame());
            }
            else
            {
                throw new System.Exception("Event Tracker is not yet ready");
            }

            IEnumerator ResetOnEndOfFrame()
            {
                yield return new WaitForEndOfFrame();
                scriptableEvent.Reset();
            }
        }
    }
}