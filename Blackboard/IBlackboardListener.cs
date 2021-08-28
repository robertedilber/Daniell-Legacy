namespace Daniell.Blackboard
{
    /// <summary>
    /// Implements callbacks for when a reference is updated in the blackboard
    /// </summary>
    public interface IBlackboardListener
    {
        /// <summary>
        /// Called when a reference has been set in the blackboard
        /// </summary>
        /// <param name="id">ID of the reference</param>
        void OnReferenceSet(string id);

        /// <summary>
        /// Called when a reference has been unset in the blackboard
        /// </summary>
        /// <param name="id">ID of the reference</param>
        void OnReferenceUnset(string id);
    }
}