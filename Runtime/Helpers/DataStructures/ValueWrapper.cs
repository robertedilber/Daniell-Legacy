namespace Daniell.Runtime.DataStructures
{
    /// <summary>
    /// Wrapper for a single value to enable JSON conversion
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    [System.Serializable]
    public struct ValueWrapper<T>
    {
        public T value;

        public ValueWrapper(T value)
        {
            this.value = value;
        }
    }
}