using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu]
public class DialogueFile : ScriptableObject, IEnumerable<GraphNodeData>
{
    private List<GraphNodeData> _nodeData = new List<GraphNodeData>();

    public void AddNodeData(GraphNodeData graphNodeData)
    {
        _nodeData.Add(graphNodeData);
    }

    public void RemoveNodeData(GraphNodeData graphNodeData)
    {
        _nodeData.Remove(graphNodeData);
    }

    public void Clear()
    {
        _nodeData.Clear();
    }

    public IEnumerator<GraphNodeData> GetEnumerator()
    {
        return ((IEnumerable<GraphNodeData>)_nodeData).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_nodeData).GetEnumerator();
    }
}
