using System;
using UnityEngine;

public class SetParameterNode : ParameterNode
{
    protected override Color DefaultNodeColor => new Color32(44, 130, 201, 255);
    protected override Type DataType => typeof(object);

    public SetParameterNode()
    {
        // Output Port
        AddOutputPort("Output");

        GetOutputPorts();
    }

    protected override string GetName(string type)
    {
        return $"Set {type} Parameter";
    }

    protected override void ClearContent()
    {
        base.ClearContent();

        ClearContentContainer(extensionContainer);
    }

    protected override void SetBool()
    {
        base.SetBool();
        AddToggle("Value", null);
    }

    protected override void SetString()
    {
        base.SetString();
        AddTextfield("Value", false, null);
    }

    protected override void SetFloat()
    {
        base.SetFloat();
        AddFloatField("Value", null);
    }

    protected override void SetInt()
    {
        base.SetInt();
        AddIntField("Value", null);
    }
}