using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A Node that can be added to a graphview
/// </summary>
public abstract class GraphNode : Node
{
    /// <summary>
    /// X Position of the start node
    /// </summary>
    public const int DEFAULT_NODE_X_POSITION = 50;

    /// <summary>
    /// Y Position of the start node
    /// </summary>
    public const int DEFAULT_NODE_Y_POSITION = 50;

    /// <summary>
    /// Default width to give a node when created
    /// </summary>
    public const int DEFAULT_NODE_WIDTH = 350;

    /// <summary>
    /// Default height to give a node when created
    /// </summary>
    public const int DEFAULT_NODE_HEIGHT = 150;

    /// <summary>
    /// GUID of the node
    /// </summary>
    public string GUID { get; private set; }

    /// <summary>
    /// Is this node saveable?
    /// </summary>
    public virtual bool IsSaveable => true;

    /// <summary>
    /// Color applied to the title bar when the node is created
    /// </summary>
    protected abstract Color DefaultNodeColor { get; }

    /// <summary>
    /// Name given to the node when created
    /// </summary>
    protected abstract string DefaultNodeName { get; }

    /// <summary>
    /// Type of Node Data to be used. 
    /// </summary>
    protected virtual Type DataType => typeof(GraphNodeData);

    /// <summary>
    /// Is this node's output connected to other nodes?
    /// </summary>
    public bool IsOutputConnected => GetConnectedNodes().Length != 0;

    // Protected fields
    protected readonly Rect _defaultNodeRect = new Rect(DEFAULT_NODE_X_POSITION, DEFAULT_NODE_Y_POSITION, DEFAULT_NODE_WIDTH, DEFAULT_NODE_HEIGHT);

    // Private fields
    private Dictionary<string, VisualElement> _controls = new Dictionary<string, VisualElement>();

    public GraphNode()
    {
        // Create a new GUID
        GUID = Guid.NewGuid().ToString();

        // Set node default position
        SetPosition(_defaultNodeRect);

        // Set the width
        SetWidth(DEFAULT_NODE_WIDTH);

        // Set the default color of the node
        titleContainer.style.backgroundColor = DefaultNodeColor;

        // Set extension container color
        Color.RGBToHSV(DefaultNodeColor, out float h, out float s, out float v);
        extensionContainer.style.backgroundColor = Color.HSVToRGB(h, Mathf.Clamp01(s - 0.3f), Mathf.Clamp01(v - 0.3f));

        // Set padding
        extensionContainer.style.paddingTop = extensionContainer.style.paddingLeft = extensionContainer.style.paddingRight = 5;

        // Set node name
        title = DefaultNodeName;

    }

    #region Node Connection

    /// <summary>
    /// Get all the nodes connected to this node
    /// </summary>
    /// <returns>List of connected nodes</returns>
    public GraphNode[] GetConnectedNodes()
    {
        List<GraphNode> connectedNodes = new List<GraphNode>();

        var outputPorts = GetOutputPorts();

        // Find all connected output ports
        foreach (Port port in outputPorts)
        {
            if (port.connected)
            {
                // If the port has connections, add them to the list of connected nodes
                foreach (Edge connection in port.connections)
                {
                    // Get connected input port of the edge
                    var targetPort = connection.input;

                    // Add connected node
                    connectedNodes.Add((GraphNode)targetPort.node);
                }
            }
        }

        return connectedNodes.ToArray();
    }

    /// <summary>
    /// Get all the output ports of this node
    /// </summary>
    /// <returns>List of output ports</returns>
    public Port[] GetOutputPorts()
    {
        List<Port> ports = new List<Port>();

        // Search output container for ports
        foreach (VisualElement visualElement in outputContainer.Children())
        {
            // Add the port to the list if it is an output port
            if (visualElement is Port p && p.direction == Direction.Output)
            {
                ports.Add(p);
            }
        }

        return ports.ToArray();
    }

    #endregion

    #region Port

    /// <summary>
    /// Shortcut to add an input port
    /// </summary>
    /// <param name="name">Name of the port</param>
    public virtual void AddInputPort(string name)
    {
        AddInputPort(name, out Port _);
    }

    /// <summary>
    /// Shortcut to add an input port
    /// </summary>
    /// <param name="name">Name of the port</param>
    /// <param name="port">Port Created</param>
    public virtual void AddInputPort(string name, out Port port)
    {
        AddPort(name, Direction.Input, Port.Capacity.Multi, out port);
    }

    /// <summary>
    /// Shortcut to add an output port
    /// </summary>
    /// <param name="name">Name of the port</param>
    public virtual void AddOutputPort(string name)
    {
        AddOutputPort(name, out Port _);
    }

    /// <summary>
    /// Shortcut to add an output port
    /// </summary>
    /// <param name="name">Name of the port</param>
    /// <param name="port">Port Created</param>
    public virtual void AddOutputPort(string name, out Port port)
    {
        AddPort(name, Direction.Output, Port.Capacity.Single, out port);
    }

    /// <summary>
    /// Add a new port to the node
    /// </summary>
    /// <param name="name">Name of the port</param>
    /// <param name="direction">Direction of the port (input or output)</param>
    /// <param name="capacity">Capacity of the node (single or multiple)</param>
    public virtual void AddPort(string name, Direction direction, Port.Capacity capacity, out Port port)
    {
        port = InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(object));
        port.portName = name;
        port.portColor = DefaultNodeColor;

        switch (direction)
        {
            case Direction.Input:
                // Add port to the input container
                inputContainer.Add(port);
                break;
            case Direction.Output:
                // Add port to the output container
                outputContainer.Add(port);
                break;
            default:
                // Do nothing
                break;
        }

        // Refresh node view
        RefreshExpandedState();
        RefreshPorts();
    }

    #endregion

    #region Size & Position

    public override void SetPosition(Rect newPos)
    {
        // Ensure that the position is locked to the grid
        newPos.x -= newPos.x % 25;
        newPos.y -= newPos.y % 25;

        base.SetPosition(newPos);
    }

    /// <summary>
    /// Set the position of the node using XY coordinates
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public void SetPosition(int x, int y)
    {
        SetPosition(new Rect(x, y, DEFAULT_NODE_WIDTH, DEFAULT_NODE_HEIGHT));
    }

    /// <summary>
    /// Set the width of the node
    /// </summary>
    /// <param name="width">Width</param>
    public void SetWidth(int width)
    {
        style.width = width;
    }

    /// <summary>
    /// Set the height of the node
    /// </summary>
    /// <param name="height">Height</param>
    public void SetHeight(int height)
    {
        style.height = height;
    }

    #endregion

    #region Content Helpers

    protected bool TryGetField<T>(string fieldName, out T control) where T : VisualElement
    {
        control = default;

        if (_controls.ContainsKey(fieldName))
        {
            control = (T)_controls[fieldName];
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void SetFieldValue<T>(string fieldName, T value)
    {
        // Set value if the control exists
        if (TryGetField(fieldName, out BaseField<T> control))
        {
            control.value = value;
        }
    }

    protected T GetFieldValue<T>(string fieldName)
    {
        // Get value if the control exists
        if (TryGetField(fieldName, out BaseField<T> control))
        {
            return control.value;
        }
        else
        {
            return default;
        }
    }

    protected void AddField(string fieldName, VisualElement control)
    {
        // Add to controls
        _controls.Add(fieldName, control);

        Label label = new Label();
        label.text = fieldName;

        Box box = new Box();
        box.style.borderBottomLeftRadius = box.style.borderBottomRightRadius = box.style.borderTopLeftRadius = box.style.borderTopRightRadius = 5;
        box.style.marginBottom = 5;
        box.Add(label);
        box.Add(control);

        extensionContainer.Add(box);

        RefreshExpandedState();
    }

    protected TextField AddTextfield(string name, bool multiline, Action<string> onValueChangedCallback = null)
    {
        TextField textField = new TextField();
        textField.multiline = multiline;
        textField.RegisterValueChangedCallback(x => onValueChangedCallback?.Invoke(x.newValue));

        AddField(name, textField);

        return textField;
    }

    protected Toggle AddToggle(string name, Action<bool> onValueChangedCallback = null)
    {
        Toggle toggle = new Toggle();
        toggle.RegisterValueChangedCallback(x => onValueChangedCallback?.Invoke(x.newValue));

        AddField(name, toggle);

        return toggle;
    }

    protected FloatField AddFloatField(string name, Action<float> onValueChangedCallback = null)
    {
        FloatField floatField = new FloatField();
        floatField.RegisterValueChangedCallback(x => onValueChangedCallback?.Invoke(x.newValue));

        AddField(name, floatField);

        return floatField;
    }

    protected IntegerField AddIntField(string name, Action<int> onValueChangedCallback = null)
    {
        IntegerField integerField = new IntegerField();
        integerField.RegisterValueChangedCallback(x => onValueChangedCallback?.Invoke(x.newValue));

        AddField(name, integerField);

        return integerField;
    }

    protected ObjectField AddObjectField<T>(string name, Action<T> onValueChangedCallback = null) where T : UnityEngine.Object
    {
        ObjectField objectField = new ObjectField();
        objectField.objectType = typeof(T);
        objectField.RegisterValueChangedCallback(x => onValueChangedCallback?.Invoke((T)x.newValue));

        AddField(name, objectField);

        return objectField;
    }

    #endregion

    #region Save & Load

    public virtual GraphNodeData ToNodeData()
    {
        if (!DataType.IsSubclassOf(typeof(GraphNodeData)))
        {
            Debug.LogWarning($"{DataType} is not a valid type for {GetType()} data. Node wasn't saved.");
            return null;
        }

        // Create a scriptable instance of NodeData
        GraphNodeData nodeData = (GraphNodeData)ScriptableObject.CreateInstance(DataType);

        Rect nodePosition = GetPosition();

        // Assign node position
        nodeData.X = (int)nodePosition.x;
        nodeData.Y = (int)nodePosition.y;

        // Set type
        nodeData.NodeTypeName = GetType().AssemblyQualifiedName;

        // Assign GUID
        nodeData.GUID = GUID;

        return nodeData;
    }

    public virtual void FromNodeData(GraphNodeData nodeData)
    {
        // Set node position
        SetPosition(nodeData.X, nodeData.Y);

        // Update GUID
        GUID = nodeData.GUID;

        // Set node connections
        // TODO: uh oh nodes are not instantiated when doing this
    }

    #endregion
}