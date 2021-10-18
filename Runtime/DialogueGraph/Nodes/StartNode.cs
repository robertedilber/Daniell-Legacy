using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Node used as a starting point of every dialogue graph
/// </summary>
public class StartNode : BaseNode
{
    protected override Color DefaultNodeColor => new Color(0.15f, 0.43f, 0.12f);
    protected override string DefaultNodeName => "Start";

    public StartNode()
    {
        // Add a single output port to the start node
        AddOutputPort("Next");

        SetWidth(75);
    }

    public override bool IsMovable() => false;

    public override bool IsSelectable() => false;
}
