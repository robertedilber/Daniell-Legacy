using Daniell.EventSystem.Scriptable;
using System;
using UnityEngine;

public class CallEventNode : TypeSelectorNode
{
    protected override Color DefaultNodeColor => new Color32(80, 150, 100, 255);

    public CallEventNode()
    {
        AddOutputPort("Output");
    }

    protected override void ClearContent()
    {
        base.ClearContent();
        ClearContentContainer(extensionContainer);
    }

    protected override void SetBool()
    {
        base.SetBool();
        AddObjectField<BoolEvent>("Event");
        AddToggle("Event Value");
    }

    protected override void SetString()
    {
        base.SetString();
        AddObjectField<StringEvent>("Event");
        AddTextfield("Event Value", true);
    }

    protected override void SetFloat()
    {
        base.SetFloat();
        AddObjectField<FloatEvent>("Event");
        AddFloatField("Event Value");
    }

    protected override void SetInt()
    {
        base.SetInt();
        AddObjectField<IntEvent>("Event");
        AddIntField("Event Value");
    }

    protected override string GetName(string type)
    {
        return $"Call {type} Event";
    }
}