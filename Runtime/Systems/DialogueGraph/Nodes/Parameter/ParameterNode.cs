using UnityEditor.Experimental.GraphView;

namespace Daniell.Runtime.Systems.DialogueNodes
{
    public abstract class ParameterNode : TypeSelectorNode
    {
        public const string PARAMETER_FIELD_NAME = "Parameter Name";

        public ParameterNode()
        {
            // Parameter Name
            AddField(new StringNodeField("Parameter Name", false), PARAMETER_FIELD_NAME);

            SelectType("Bool");
        }
    }
}