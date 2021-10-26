namespace Daniell.Runtime.Save
{
    /// <summary>
    /// Represents an object that can be saved
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Generic SaveableEntity used by the SaveManager
        /// </summary>
        SaveableEntity SaveableEntity { get; }
    }
}