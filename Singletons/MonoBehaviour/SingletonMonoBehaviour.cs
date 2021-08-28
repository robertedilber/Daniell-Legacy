using UnityEngine;
using System;
using System.Collections.Generic;

namespace Daniell.Singletons
{
    /// <summary>
    /// Singleton implementation for a MonoBehaviour
    /// </summary>
    /// <typeparam name="T">Type of the child class</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Instance of this singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogWarning($"{typeof(T).Name} is not yet ready.");
                }

                return _instance;
            }
            set { _instance = value; }
        }

        /// <summary>
        /// Is this singleton ready?
        /// </summary>
        public bool IsInstanceReady => _instance != null;

        // Private fields
        private static T _instance;
        private static List<Action<T>> _postponedActions = new List<Action<T>>();

        /// <summary>
        /// Called when the instance is ready
        /// </summary>
        private static event Action OnInstanceReady;

        protected virtual void Awake()
        {
            // If the Instance is already set, destroy this instance
            if (_instance != null)
                Destroy(this);
            else
            {
                Instance = this as T;
                OnInstanceReady?.Invoke();

                // Execute postponed actions
                for (int i = 0; i < _postponedActions.Count; i++)
                {
                    _postponedActions[i]?.Invoke(_instance);
                }

                _postponedActions.Clear();
            }
        }

        protected virtual void OnDestroy()
        {
            // Set the instance to null when this instance is destroyed
            Instance = null;
        }

        /// <summary>
        /// Delay an action to when the instance is ready
        /// </summary>
        /// <param name="action">Action to be delayed</param>
        protected static void DelayedInstanceCall(Action<T> action)
        {
            if (!Application.isPlaying)
            {
                Debug.Log("Action not called, application is not playing.");
                return;
            }

            if (_instance == null)
            {
                _postponedActions.Add(action);
            }
            else
            {
                action?.Invoke(_instance);
            }
        }
    }
}