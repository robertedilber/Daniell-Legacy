using Daniell.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Blackboard
{
    /// <summary>
    /// Holds references to scene objects bound by an ID
    /// </summary>
    public class SceneBlackboard : SingletonMonoBehaviour<SceneBlackboard>
    {
        public static event Action<string> OnReferenceSet;
        public static event Action<string> OnReferenceUnset;

        private Dictionary<string, object> _references = new Dictionary<string, object>();
        private List<IBlackboardListener> _blackboardListeners = new List<IBlackboardListener>();

        #region References

        /// <summary>
        /// Add a new reference to the blackboard
        /// </summary>
        /// <param name="id">ID of the reference (must be unique)</param>
        /// <param name="o">Object reference</param>
        public static void AddReference(string id, object o)
        {
            // Delay call to add reference
            DelayedInstanceCall(instance =>
            {
                instance._references.Add(id, o);
                for (int i = 0; i < instance._blackboardListeners.Count; i++)
                {
                    instance._blackboardListeners[i].OnReferenceSet(id);
                }
            });

            OnReferenceSet?.Invoke(id);
        }

        /// <summary>
        /// Remove a reference from the blackboard
        /// </summary>
        /// <param name="id">ID of the reference to remove</param>
        public static void RemoveReference(string id)
        {
            // Delay call to remove reference
            DelayedInstanceCall(instance =>
            {
                instance._references.Remove(id);
                for (int i = 0; i < instance._blackboardListeners.Count; i++)
                {
                    instance._blackboardListeners[i].OnReferenceUnset(id);
                }
            });

            OnReferenceUnset?.Invoke(id);
        }

        /// <summary>
        /// Get a reference from the blackboard
        /// </summary>
        /// <typeparam name="T">Type of the reference</typeparam>
        /// <param name="id">ID of the reference to get</param>
        /// <returns>Null if the blackboard is not ready</returns>
        public static T GetReference<T>(string id)
        {
            return (T)Instance?._references[id];
        }

        #endregion

        #region Blackboard Events

        /// <summary>
        /// Register a new Blackboard Listener to this blackboard
        /// </summary>
        /// <param name="blackboardListener">Target blackboard listener</param>
        public static void Register(IBlackboardListener blackboardListener)
        {
            DelayedInstanceCall(x => x._blackboardListeners.Add(blackboardListener));
        }

        /// <summary>
        /// Unregister a new Blackboard Listener from this blackboard
        /// </summary>
        /// <param name="blackboardListener">Target blackboard listener</param>
        public static void Unregister(IBlackboardListener blackboardListener)
        {
            DelayedInstanceCall(x => x._blackboardListeners.Remove(blackboardListener));
        }

        #endregion
    }
}