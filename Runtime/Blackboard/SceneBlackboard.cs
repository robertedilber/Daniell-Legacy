using Daniell.Runtime.Singletons;
using System;
using System.Collections.Generic;

namespace Daniell.Runtime.Blackboard
{
    /// <summary>
    /// Holds values bound by an ID
    /// </summary>
    public class SceneBlackboard : SingletonMonoBehaviour<SceneBlackboard>
    {
        // Private fields
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        private Dictionary<string, Action<string, object>> _subscriptions = new Dictionary<string, Action<string, object>>();

        /// <summary>
        /// Set a value in the blackboard
        /// </summary>
        /// <param name="id">ID of the value to set</param>
        /// <param name="value">Value to be set</param>
        public static void SetValue(string id, object value)
        {
            DelayedInstanceCall(instance =>
            {
                if (instance._values.ContainsKey(id))
                {
                    instance._values[id] = value;
                }
                else
                {
                    instance._values.Add(id, value);
                }

                // Call corresponding subscriptions
                if (instance._subscriptions.TryGetValue(id, out Action<string, object> action))
                {
                    action?.Invoke(id, value);
                }
            });
        }

        /// <summary>
        /// Unset a value in the blackboard
        /// </summary>
        /// <param name="id">ID of the value to remove</param>
        public static void RemoveValue(string id)
        {
            DelayedInstanceCall(instance =>
            {
                // Call corresponding subscriptions
                if (instance._subscriptions.TryGetValue(id, out Action<string, object> action))
                {
                    action?.Invoke(id, null);
                }

                if (instance._values.ContainsKey(id))
                {
                    instance._values.Remove(id);
                }
            });
        }

        /// <summary>
        /// Try to get a value from the blackboard
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="id">ID of the value to get</param>
        /// <param name="value">Value if found</param>
        /// <returns>Did the value exist in the blackboard?</returns>
        public static bool TryGetValue<T>(string id, out T value)
        {
            // Default the value
            value = default;

            // Try to get the value in the dictionary and cast it to 'T'
            if (Instance._values.TryGetValue(id, out object dictValue))
            {
                value = (T)dictValue;
                return true;
            }

            // If everything failed, return false
            return false;
        }

        /// <summary>
        /// Subscribe an Action to an ID
        /// </summary>
        /// <param name="id">ID to call the action for</param>
        /// <param name="action">Action to be called</param>
        public static void SubscribeToID(string id, Action<string, object> action)
        {
            DelayedInstanceCall(instance =>
            {
                if (instance._subscriptions.ContainsKey(id))
                {
                    instance._subscriptions[id] += action;
                }
                else
                {
                    instance._subscriptions.Add(id, action);
                }

                // Invoke action if the ID is already set
                if (TryGetValue(id, out object value))
                {
                    action?.Invoke(id, value);
                }
            });
        }

        /// <summary>
        /// Unsubscribe an Action from an ID
        /// </summary>
        /// <param name="id">ID to remove</param>
        /// <param name="action">Action to unsubscribe</param>
        public static void UnsubscribeFromID(string id, Action<string, object> action)
        {
            DelayedInstanceCall(instance =>
            {
                if (instance._subscriptions.ContainsKey(id))
                {
                    instance._subscriptions[id] -= action;
                }
                else
                {
                    instance._subscriptions.Remove(id);
                }
            });
        }
    }
}