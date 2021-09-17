using System;
using UnityEditor;
using UnityEngine;

namespace Daniell.SaveSystem
{
    /// <summary>
    /// Handles loading and saving data
    /// </summary>
    public abstract class SaveableEntity
    {
        public const string UNDEFINED_SAVE_ID = "undefined";

        /// <summary>
        /// Unique ID used to locate saved data
        /// </summary>
        public string SaveID => _saveID;

        [SerializeField]
        private string _saveID = UNDEFINED_SAVE_ID;

        /// <summary>
        /// Load the last saved object state
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Saves the current object state
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Clear saved data
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Generate a new save ID
        /// </summary>
        public void GenerateSaveID()
        {
            _saveID = GUID.Generate().ToString();
        }
    }

    /// <summary>
    /// Generic version of SaveableEntity
    /// </summary>
    /// <typeparam name="T">Type of Data to be saved</typeparam>
    [Serializable]
    public class SaveableEntity<T> : SaveableEntity
    {
        /// <summary>
        /// Called when loading the saved state
        /// </summary>
        public event Action<T> OnLoad;

        /// <summary>
        /// Called when saving the current state
        /// </summary>
        public event Func<T> OnSave;

        public override void Load()
        {
            // If the curent key doesn't exist
            if (!PlayerPrefs.HasKey(SaveID))
            {
                Debug.Log($"Object {SaveID} was not loaded. No saved data could be found.");
                return;
            }

            // Load json data from PlayerPrefs
            string json = PlayerPrefs.GetString(SaveID);

            // Convert from JSON to T
            T loadedState = JsonUtility.FromJson<T>(json);

            // Load on object
            OnLoad.Invoke(loadedState);

            Debug.Log($"Loaded Object with ID: {SaveID}");
        }

        public override void Save()
        {
            // Get current object state
            T currentState = OnSave.Invoke();

            // Save as JSON in PlayerPrefs
            string json = JsonUtility.ToJson(currentState, true);
            PlayerPrefs.SetString(SaveID, json);

            Debug.Log($"Saved Object with ID: {SaveID}");
        }

        public override void Clear()
        {
            // Clear saved data if it exists
            if (PlayerPrefs.HasKey(SaveID))
            {
                PlayerPrefs.DeleteKey(SaveID);
            }
        }
    }
}