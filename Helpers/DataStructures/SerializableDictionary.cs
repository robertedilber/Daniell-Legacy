using System;
using System.Collections.Generic;

namespace Daniell.Helpers.DataStructures
{
    /// <summary>
    /// A Serializable version of a Dictionary. First get will take more time, so it's best to update manually before accessing.
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : List<SerializableKeyValuePair<TKey, TValue>>
    {
        public TValue this[TKey key]
        {
            get
            {
                if(_internalDictionary == null)
                {
                    UpdateInternalDictionary();
                }
                return _internalDictionary[key];
            }
        }

        // Private fields
        private Dictionary<TKey, TValue> _internalDictionary;

        /// <summary>
        /// Update the internal dictionary manually.
        /// </summary>
        public void UpdateInternalDictionary()
        {
            _internalDictionary = new Dictionary<TKey, TValue>();

            for (int i = 0; i < Count; i++)
            {
                SerializableKeyValuePair<TKey, TValue> keyValuePair = this[i];
                _internalDictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
