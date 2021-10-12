using System;
using UnityEngine;

public class GetParameterNode : ParameterNode
{
    protected override Color DefaultNodeColor => new Color32(80, 100, 201, 255);
    protected override Type DataType => typeof(GetParameterNodeData);

    protected override string GetName(string type)
    {
        return $"Get {type} Parameter";
    }

    protected override void ClearContent()
    {
        base.ClearContent();

        ClearContentContainer(extensionContainer);
        ClearContentContainer(outputContainer);
    }

    protected override void SetBool()
    {
        base.SetBool();
        AddOutputPort("True");
        AddOutputPort("False");
    }

    protected override void SetString()
    {
        base.SetString();
        AddTextfield("Value", false, null);
        AddOutputPort("==");
        AddOutputPort("!=");
    }

    protected override void SetFloat()
    {
        base.SetFloat();
        AddFloatField("Value", null);
        AddOutputPort("==");
        AddOutputPort(">");
        AddOutputPort("<");
    }

    protected override void SetInt()
    {
        base.SetInt();
        AddIntField("Value", null);
        AddOutputPort("==");
        AddOutputPort(">");
        AddOutputPort("<");
    }
}
