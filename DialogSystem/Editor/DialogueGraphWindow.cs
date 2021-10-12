using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphWindow : EditorWindow
{
    // Properties
    private List<DialogueBranchNode> DialogueNodes => _graphView?.nodes.ToList().Cast<DialogueBranchNode>().ToList();

    // Private fields
    private DialogueGraphView _graphView;
    private Toolbar _toolbar;
    private DialogueFile _dialogueFile;

    [MenuItem("Daniell/Dialogue Graph")]
    public static void Open()
    {
        var window = GetWindow<DialogueGraphWindow>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    #region Init / Deinit

    private void OnEnable()
    {
        // Create the toolbar
        CreateToolbar();
    }

    private void OnDisable()
    {
        // Remove the toolbar
        RemoveToolbar();

        // Remove the graph view
        RemoveGraphView();
    }

    #endregion

    #region Toolbar

    /// <summary>
    /// Create the toolbar
    /// </summary>
    private void CreateToolbar()
    {
        if (_toolbar == null)
        {
            _toolbar = new Toolbar();

            // Save button
            CreateToolbarButton("Save", () => Save());

            // Dialogue File loading field
            ObjectField scriptableDialogFile = new ObjectField();
            scriptableDialogFile.objectType = typeof(DialogueFile);
            scriptableDialogFile.RegisterValueChangedCallback(x => OnDialogueFileValueChanged((DialogueFile)x.newValue));
            _toolbar.Add(scriptableDialogFile);

            // Add the toolbar to the window
            rootVisualElement.Add(_toolbar);
        }

        void CreateToolbarButton(string name, Action action)
        {
            var button = new Button(action);
            button.text = name;
            _toolbar.Add(button);
        }
    }

    /// <summary>
    /// Remove the toolbar
    /// </summary>
    private void RemoveToolbar()
    {
        if (_toolbar != null)
        {
            rootVisualElement.Remove(_toolbar);
        }
    }

    #endregion

    #region GraphView

    /// <summary>
    /// Create the GraphView
    /// </summary>
    private void CreateGraphView()
    {
        if (_graphView == null)
        {
            // Create the graph view instance
            _graphView = new DialogueGraphView("Dialogue Graph", this, _dialogueFile);

            // Stretch view to container
            _graphView.StretchToParentSize();

            // Add to the window
            rootVisualElement.Add(_graphView);

            // Send the view to the back so that the toolbar stays on top
            _graphView.SendToBack();
        }
    }

    /// <summary>
    /// Remove the GraphView
    /// </summary>
    private void RemoveGraphView()
    {
        if (_graphView != null)
        {
            // Remove from the window
            rootVisualElement.Remove(_graphView);

            // Reset reference
            _graphView = null;
        }
    }

    #endregion

    /// <summary>
    /// Called when the dialogue file value has changed
    /// </summary>
    /// <param name="dialogueFile">New Dialogue file</param>
    private void OnDialogueFileValueChanged(DialogueFile dialogueFile)
    {
        // Cache value
        _dialogueFile = dialogueFile;

        if (dialogueFile != null)
        {
            // Create the graph
            CreateGraphView();

            // Load the graph
            Load();
        }
        else
        {
            // Remove the graph
            RemoveGraphView();
        }
    }

    #region Save / Load

    private void Save()
    {
        // Get Nodes
        List<BaseNode> nodes = _graphView.nodes.ToList().Cast<BaseNode>().ToList();

        // Clear dialogue file refs
        _dialogueFile.Clear();

        // Get Node Data from nodes and add to the dialogue file
        for (int i = 0; i < nodes.Count; i++)
        {
            BaseNode node = nodes[i];

            switch (node)
            {
                // If the node is a regular dialogue node
                case GraphNode graphNode:

                    // Get node data
                    GraphNodeData nodeData = graphNode.ToNodeData();

                    // Create the new scriptable object and add to the dialogue file
                    if (nodeData != null)
                    {
                        // Set sciptable node data name
                        nodeData.name = nodeData.GetType().Name;
                        _dialogueFile.AddNodeData(nodeData);
                    }

                    // Break
                    break;

                // If the node is the Start Node
                case StartNode startNode:

                    Dictionary<string, string> connectedGUIDs = startNode.GetConnectedGUIDs();

                    // Set default value
                    _dialogueFile.StartNodeConnectedGUID = null;

                    if (connectedGUIDs.Count > 0)
                    {
                        KeyValuePair<string, string> connectedGUID = connectedGUIDs.First();
                        if (connectedGUID.Value != null)
                        {
                            _dialogueFile.StartNodeConnectedGUID = connectedGUID.Value;
                        }
                    }

                    // Break
                    break;

                default:
                    // Do nothing
                    break;
            }
        }
    }

    private void Load()
    {
        // If there is no dialogue file
        if (_dialogueFile == null || _graphView == null)
        {
            return;
        }

        // Create nodes using sub assets
        for (int i = 0; i < _dialogueFile.NodeDataCount; i++)
        {
            // Create nodes
            GraphNodeData nodeData = _dialogueFile[i];
         
            // Instantiate node using reflection
            var method = typeof(DialogueGraphView).GetMethod(nameof(DialogueGraphView.CreateNode));
            var action = method.MakeGenericMethod(Type.GetType(nodeData.NodeTypeName));
            var node = action.Invoke(_graphView, null);

            // Load node data
            ((GraphNode)node).FromNodeData(nodeData);
        }

        // List nodes by GUID
        Dictionary<string, GraphNode> nodesByGUID = new Dictionary<string, GraphNode>();
        List<BaseNode> nodes = _graphView.nodes.ToList().Cast<BaseNode>().ToList();

        // Find all nodes and add to GUID list
        for (int i = 0; i < nodes.Count; i++)
        {
            BaseNode node = nodes[i];

            // If this node is saveable
            if (node is GraphNode graphNode)
            {
                nodesByGUID.Add(graphNode.GUID, graphNode);
            }
        }

        // Create connections
        for (int i = 0; i < _dialogueFile.NodeDataCount; i++)
        {
            GraphNodeData nodeData = _dialogueFile[i];

            for (int j = 0; j < nodeData.ConnectedGUIDs.Count; j++)
            {
                var connectedGUID = nodeData.ConnectedGUIDs[j];
                // Skip null or empty GUIDs
                if (connectedGUID.Value == null || connectedGUID.Value == "")
                    continue;

                GraphNode inputNode = nodesByGUID[nodeData.GUID];
                GraphNode outputNode = nodesByGUID[connectedGUID.Value];

                // Find input port on the output node
                var inputPort = outputNode.GetDefaultInputPort();

                // Find output port on the input node
                var outputPort = inputNode.GetPorts(Direction.Output).First(x => x.portName == connectedGUID.Key);

                // Connect nodes
                ConnectPorts(inputPort, outputPort);
            }
        }

        // If there is connection to the start node
        if (_dialogueFile.StartNodeConnectedGUID != null && _dialogueFile.StartNodeConnectedGUID != "")
        {
            BaseNode startNode = nodes.Where(x => x is StartNode).First();
            GraphNode nextToStartNode = nodesByGUID[_dialogueFile.StartNodeConnectedGUID];
            // Connect start node port and its connected node
            ConnectPorts(nextToStartNode.GetDefaultInputPort(), startNode.GetPorts(Direction.Output).First());
        }

        void ConnectPorts(Port inputPort, Port outputPort)
        {
            // Create a new connection
            Edge edge = new Edge
            {
                input = inputPort,
                output = outputPort
            };

            outputPort.Connect(edge);
            inputPort.Connect(edge);
            _graphView.Add(edge);
        }
    }

    #endregion
}