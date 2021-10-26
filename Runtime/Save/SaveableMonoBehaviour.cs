using UnityEngine;

namespace Daniell.Runtime.Save
{
    /// <summary>
    /// MonoBehaviour Implementation of the Save System
    /// </summary>
    /// <typeparam name="T">Type of Data to be saved</typeparam>
    public abstract class SaveableMonoBehaviour<T> : MonoBehaviour, ISaveable
    {
        public SaveableEntity SaveableEntity => _saveableEntity;

        [SerializeField]
        [Tooltip("Saveable Entity Instance")]
        private SaveableEntity<T> _saveableEntity;

        private void OnEnable()
        {

            // Register to the Save Manager
            SaveManager.Register(this);

            // Register to Save & Load events
            _saveableEntity.OnLoad += Load;
            _saveableEntity.OnSave += Save;
        }

        private Coroutine _delayCoroutine;

        private void OnDisable()
        {
            // Unregister from the Save Manager
            SaveManager.Unregister(this);

            // Unregister from Save & Load events
            _saveableEntity.OnLoad -= Load;
            _saveableEntity.OnSave -= Save;
        }

        /// <summary>
        /// Override this to add a custom Load behaviour
        /// </summary>
        /// <param name="state">Loaded State</param>
        protected abstract void Load(T state);

        /// <summary>
        /// Override this to add a custom Save behaviour
        /// </summary>
        /// <returns>Saved State</returns>
        protected abstract T Save();
    }
}