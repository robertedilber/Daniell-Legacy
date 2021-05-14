using System;
using System.Collections.Generic;

namespace Daniell.Helpers.DataStructures
{
    /// <summary>
    /// A Serializable version of KeyValuePair
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return new SerializableKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
        }

        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> keyValuePair)
        {
            return new KeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
