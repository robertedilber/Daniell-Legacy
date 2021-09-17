using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Daniell.SaveSystem
{
    /// <summary>
    /// Handles global loading and saving of the game state
    /// </summary>
    public static class SaveManager
    {
        private static List<ISaveable> _saveables = new List<ISaveable>();

        /// <summary>
        /// Add a saveable object to the list of saveable objects
        /// </summary>
        /// <param name="saveable">Object to add</param>
        public static void Register(ISaveable saveable)
        {
            _saveables.Add(saveable);
        }

        /// <summary>
        /// Remove a saveable object from the list of saveable objects
        /// </summary>
        /// <param name="saveable">Object to remove</param>
        public static void Unregister(ISaveable saveable)
        {
            _saveables.Remove(saveable);
        }

        /// <summary>
        /// Save all data
        /// </summary>
        [MenuItem("Daniell/Save Manager/Save")]
        public static void Save()
        {
            for (int i = 0; i < _saveables.Count; i++)
            {
                _saveables[i].SaveableEntity.Save();
            }
        }

        /// <summary>
        /// Load all data
        /// </summary>
        [MenuItem("Daniell/Save Manager/Load")]
        public static void Load()
        {
            for (int i = 0; i < _saveables.Count; i++)
            {
                _saveables[i].SaveableEntity.Load();
            }
        }

        /// <summary>
        /// Clear all data
        /// </summary>
        [MenuItem("Daniell/Save Manager/Clear")]
        public static void Clear()
        {
            for (int i = 0; i < _saveables.Count; i++)
            {
                _saveables[i].SaveableEntity.Clear();
            }
        }
    }
}