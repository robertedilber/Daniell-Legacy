using UnityEngine;

namespace Daniell.SaveSystem.Examples
{
    /// <summary>
    /// Example of a Simple MonoBehaviour implementation of the Save System
    /// </summary>
    public class SaveLoadSimpleExample : SaveableMonoBehaviour<SaveLoadSimpleExample.SaveData>
    {
        [SerializeField]
        [Tooltip("Character name")]
        private string _name;

        [SerializeField]
        [Tooltip("Current health amount")]
        private int _health;

        [SerializeField]
        [Tooltip("Current xp amount")]
        private int _xp;

        protected override void Load(SaveData state)
        {
            // Assing values from the loaded state
            _name = state.name;
            _health = state.health;
            _xp = state.xp;
        }

        protected override SaveData Save()
        {
            // Send values from the current state
            return new SaveData(_name, _health, _xp);
        }

        /// <summary>
        /// Data Structure holding all of the values we want to save. In this case, it needs to be public.
        /// </summary>
        public struct SaveData
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