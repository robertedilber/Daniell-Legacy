using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Daniell.Runtime.Save.Examples
{
    /// <summary>
    /// Example of a full custom implementation of the Save System
    /// </summary>
    public class SaveLoadFullExample : MonoBehaviour, ISaveable
    {
        public SaveableEntity SaveableEntity => _saveableEntity;

        [SerializeField]
        [Tooltip("Saveable Entity Instance")]
        private SaveableEntity<SaveData> _saveableEntity;

        [SerializeField]
        [Tooltip("Character name")]
        private string _name;

        [SerializeField]
        [Tooltip("Current health amount")]
        private int _health;

        [SerializeField]
        [Tooltip("Current xp amount")]
        private int _xp;

        private void OnEnable()
        {
            // Register to the Save Manager
            SaveManager.Register(this);

            // Register to Save & Load events
            _saveableEntity.OnLoad += Load;
            _saveableEntity.OnSave += Save;
        }

        private void OnDisable()
        {
            // Unregister from the Save Manager
            SaveManager.Unregister(this);

            // Unregister from Save & Load events
            _saveableEntity.OnLoad -= Load;
            _saveableEntity.OnSave -= Save;
        }

        private void Load(SaveData state)
        {
            // Assing values from the loaded state
            _name = state.name;
            _health = state.health;
            _xp = state.xp;
        }

        private SaveData Save()
        {
            // Send values from the current state
            return new SaveData(_name, _health, _xp);
        }

        /// <summary>
        /// Data Structure holding all of the values we want to save. In this case, it can be private.
        /// It also needs to have public fields to serialize properly.
        /// </summary>
        private struct SaveData
        {
            public string name;
            public int health;
            public int xp;

            public SaveData(string name, int health, int xp)
            {
                this.name = name;
                this.health = health;
                this.xp = xp;
            }
        }
    }
}