using Daniell.DialogSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Object = UnityEngine.Object;

public class DialogueLineNode : GraphNode
{
    public const string SPEAKER_CONTROL_NAME = "Speaker";
    public const string LINE_CONTROL_NAME = "Line";

    protected override Color DefaultNodeColor => new Color32(150, 70, 80, 255);
    protected override string DefaultNodeName => "Dialogue Line";
    protected override Type DataType => typeof(DialogueLineNodeData);

    public DialogueLineNode()
    {
        // Add Input
        AddInputPort("Input");

        // Add output
        AddOutputPort("Next", out Port port);

        // Add parameter fields and link to variables
        AddObjectField<Character>(SPEAKER_CONTROL_NAME);
        AddTextfield("Line", multiline: true);
    }

    public override GraphNodeData ToNodeData()
    {
        // Cast base node data into concrete type
        DialogueLineNodeData nodeData = (DialogueLineNodeData)base.ToNodeData();

        // Assign speaker and line values from node fields
        nodeData.Speaker = (Character)GetFieldValue<Object>(SPEAKER_CONTROL_NAME);
        nodeData.Line = GetFieldValue<string>(LINE_CONTROL_NAME);

        return nodeData;
    }

    public override void FromNodeData(GraphNodeData nodeData)
    {
        base.FromNodeData(nodeData);
        var data = (DialogueLineNodeData)nodeData;

        // Set node data
        SetFieldValue<Object>(SPEAKER_CONTROL_NAME, data.Speaker);
        SetFieldValue(LINE_CONTROL_NAME, data.Line);
    }
}
