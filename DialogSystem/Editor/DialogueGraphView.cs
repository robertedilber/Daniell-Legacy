using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    private DialogueGraphSearchWindow _dialogueGraphSearchWindow;

    public DialogueGraphView(string name, EditorWindow parent)
    {
        this.name = name;

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // Add zoom capabilities
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        // Add a grid background
        GridBackground gridBackground = new GridBackground();
        Insert(0, gridBackground);
        gridBackground.StretchToParentSize();

        styleSheets.Add(Resources.Load<StyleSheet>("DialogGraph"));
        // Create the start node
        CreateNode<StartNode>();

        _dialogueGraphSearchWindow = ScriptableObject.CreateInstance<DialogueGraphSearchWindow>();
        _dialogueGraphSearchWindow.Initialize(parent, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _dialogueGraphSearchWindow);
    }

    public GraphNode CreateNode<T>() where T : GraphNode, new()
    {
        GraphNode node = new T();
        AddElement(node);
        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }
}
