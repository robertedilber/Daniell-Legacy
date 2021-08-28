namespace Daniell.Blackboard
{
    public interface IBlackboardListener
    {
        void OnReferenceSet(string id);
        void OnReferenceUnset(string id);
    }
}