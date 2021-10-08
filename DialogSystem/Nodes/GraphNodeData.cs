using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for node data
/// </summary>
public class GraphNodeData : ScriptableObject
{
    /// <summary>
    /// X Position of the node
    /// </summary>
    public int X { get => _x; set => _x = value; }
    [SerializeField] private int _x;

    /// <summary>
    /// Y Position of the node
    /// </summary>
    public int Y { get => _y; set => _y = value; }
    [SerializeField] private int _y;

    /// <summary>
    /// Type name of the node. Needs to be an assembly qualified type name.
    /// </summary>
    public string NodeTypeName { get => _nodeTypeName; set => _nodeTypeName = value; }
    [SerializeField] private string _nodeTypeName;

    /// <summary>
    /// Original GUID of the node.
    /// </summary>
    public string GUID { get => _guid; set => _guid= value; }
    [SerializeField] private string _guid;
}
