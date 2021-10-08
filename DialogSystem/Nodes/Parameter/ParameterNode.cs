using UnityEditor.Experimental.GraphView;

public abstract class ParameterNode : TypeSelectorNode
{
    public ParameterNode()
    {
        // Input Port
        AddInputPort("Input");
    }

    protected override void DrawDefaultContent()
    {
        base.DrawDefaultContent();        
        
        // Parameter Name
        AddTextfield("Parameter Name", false);
    }
}
