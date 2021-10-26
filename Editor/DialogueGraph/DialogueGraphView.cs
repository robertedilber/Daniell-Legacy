using Daniell.Runtime.DialogueNodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Daniell.Editor.DialogueNodes
{
    public class DialogueGraphView : GraphView
    {
        private DialogueGraphSearchWindow _dialogueGraphSearchWindow;

        public DialogueGraphView(string name, EditorWindow parent, DialogueFile dialogueFile)
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
            _dialogueGraphSearchWindow.Initialize(parent, this, dialogueFile.GetValidNodeTypes());
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _dialogueGraphSearchWindow);
        }

        public BaseNode CreateNode<T>() where T : BaseNode, new()
        {
            BaseNode node = new T();
            AddElement(node);
            node.OnNodeUpdated += Test;
            return node;
        }

        private void Test()
        {
            Debug.Log("Hello World");
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Duplicate", (e) =>
            {
                // Get middle point
                Vector2 center = Vector2.zero;
                int count = 0;
                foreach (ISelectable selectable in selection.Where(x => x is GraphNode))
                {
                    center += ((GraphNode)selectable).GetPosition().position;
                    count++;
                }
                center /= count;

                // Get Selection
                foreach (ISelectable selectable in selection.Where(x => x is GraphNode))
                {
                    var graphNode = (GraphNode)selectable;

                    var nodeData = graphNode.ToNodeData();

                    // Instantiate node using reflection
                    var method = typeof(DialogueGraphView).GetMethod(nameof(DialogueGraphView.CreateNode));
                    var action = method.MakeGenericMethod(Type.GetType(nodeData.NodeTypeName));
                    var node = action.Invoke(this, null);

                    // Load node data
                    var createdNode = (GraphNode)node;
                    createdNode.FromNodeData(nodeData);

                    // Get node position
                    var nodePosition = graphNode.GetPosition().position;
                    var mousePosition = e.eventInfo.mousePosition;

                    nodePosition += mousePosition - center;

                    createdNode.SetPosition((int)nodePosition.x, (int)nodePosition.y);
                }
            });
        }
    }
}