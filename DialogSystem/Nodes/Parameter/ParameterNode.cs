using UnityEditor.Experimental.GraphView;

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