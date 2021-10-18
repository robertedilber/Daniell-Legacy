using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GetParameterNode : ParameterNode
{
    public const string COMPARATOR_FIELD_NAME = "Comparator Value";

    protected override Color DefaultNodeColor => new Color32(80, 100, 201, 255);
    protected override Type DataType => typeof(GetParameterNodeData);

    private List<Port> _outputPorts = new List<Port>();

    protected override string GetName(string type)
    {
        return $"Get {type} Parameter";
    }

    protected override void ClearContent()
    {
        base.ClearContent();
        foreach(var port in _outputPorts)
        {
            RemovePort(port.portName, port.direction);
        }

        _outputPorts.Clear();

        RemoveField(COMPARATOR_FIELD_NAME);
    }

    protected override void SetBool()
    {
        base.SetBool();
        _outputPorts.Add(AddOutputPort("True"));
        _outputPorts.Add(AddOutputPort("False"));
    }

    protected override void SetString()
    {
        base.SetString();
        AddField(new StringNodeField("Comparator", false), COMPARATOR_FIELD_NAME);
        _outputPorts.Add(AddOutputPort("=="));
        _outputPorts.Add(AddOutputPort("!="));
    }

    protected override void SetFloat()
    {
        base.SetFloat();
        AddField(new IntNodeField("Comparator"), COMPARATOR_FIELD_NAME);
        _outputPorts.Add(AddOutputPort("=="));
        _outputPorts.Add(AddOutputPort(">"));
        _outputPorts.Add(AddOutputPort("<"));
    }

    protected override void SetInt()
    {
        base.SetInt();
        AddField(new IntNodeField("Comparator"), COMPARATOR_FIELD_NAME);
        _outputPorts.Add(AddOutputPort("=="));
        _outputPorts.Add(AddOutputPort(">"));
        _outputPorts.Add(AddOutputPort("<"));
    }
}


