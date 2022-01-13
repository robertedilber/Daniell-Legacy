using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Daniell.Runtime.Systems.DialogueNodes
{
    /// <summary>
    /// Base Node for a dialogue node system
    /// </summary>
    public abstract class BaseNode : Node
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
        /// Color applied to the title bar when the node is created
        /// </summary>
        protected abstract Color DefaultNodeColor { get; }

        /// <summary>
        /// Name given to the node when created
        /// </summary>
        protected abstract string DefaultNodeName { get; }

        // Protected fields
        protected readonly Rect _defaultNodeRect = new Rect(DEFAULT_NODE_X_POSITION, DEFAULT_NODE_Y_POSITION, DEFAULT_NODE_WIDTH, DEFAULT_NODE_HEIGHT);

        // Private fields
        private Dictionary<string, LabeledNodeField> _fields = new Dictionary<string, LabeledNodeField>();

        // Events
        public event Action OnNodeUpdated;

        public BaseNode()
        {
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

        public override bool IsCopiable()
        {
            return true;
        }

        #region Ports

        /// <summary>
        /// Shortcut to add an input port
        /// </summary>
        /// <param name="name">Name of the port</param>
        /// <param name="port">Port Created</param>
        /// <returns>Created port</returns>
        public Port AddInputPort(string name)
        {
            AddPort(name, Direction.Input, Port.Capacity.Multi, out Port port);
            return port;
        }

        /// <summary>
        /// Shortcut to add an output port
        /// </summary>
        /// <param name="name">Name of the port</param>
        /// <param name="port">Port Created</param>
        /// <returns>Created port</returns>
        public Port AddOutputPort(string name)
        {
            AddPort(name, Direction.Output, Port.Capacity.Single, out Port port);
            return port;
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

        public virtual void RemovePort(string name, Direction direction)
        {
            var container = direction == Direction.Input ? inputContainer : outputContainer;

            Port targetPort = null;

            foreach (var element in container.Children())
            {
                Port p = element.Q<Port>();
                if (p.portName == name)
                {
                    targetPort = p;
                    break;
                }
            }

            if (targetPort != null)
            {
                targetPort.RemoveFromHierarchy();
            }
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

        #region Node Connection

        /// <summary>
        /// Get all the connected nodes GUIDs
        /// </summary>
        /// <returns>Connected nodes GUIDs by port</returns>
        public Dictionary<string, string> GetConnectedGUIDs()
        {
            Dictionary<string, string> connectedGUIDs = new Dictionary<string, string>();

            Port[] outputPorts = GetPorts(Direction.Output);

            // Find all connected output ports
            foreach (Port port in outputPorts)
            {
                string connectedGUID = null;

                // If the port is connected
                if (port.connected)
                {
                    // Find all the connected GUIDs for this port
                    foreach (Edge connection in port.connections)
                    {
                        // Get connected input port of the edge
                        var targetPort = connection.input;
                        var targetNode = (GraphNode)targetPort.node;

                        // There should only be one connection from the output port
                        connectedGUID = targetNode.GUID;

                        // Add the port to the list
                        connectedGUIDs.Add(port.portName, connectedGUID);
                        break;
                    }
                }
            }

            // Return connected nodes as GUIDs
            return connectedGUIDs;
        }

        /// <summary>
        /// Get all the output ports of this node
        /// </summary>
        /// <returns>List of output ports</returns>
        public Port[] GetPorts(Direction direction)
        {
            List<Port> ports = new List<Port>();

            VisualElement targetContainer = direction == Direction.Output ? outputContainer : inputContainer;

            // Search output container for ports
            foreach (VisualElement visualElement in targetContainer.Children())
            {
                // Add the port to the list if it is an output port
                if (visualElement is Port p && p.direction == direction)
                {
                    ports.Add(p);
                }
            }

            return ports.ToArray();
        }

        /// <summary>
        /// Return the default input port
        /// </summary>
        /// <returns>Default input port</returns>
        public Port GetDefaultInputPort()
        {
            return GetPorts(Direction.Input).First(x => x.portName == GraphNode.DEFAULT_INPUT_NAME);
        }

        #endregion

        #region Field Handling

        protected bool TryGetField(string fieldName, out LabeledNodeField field)
        {
            field = default;

            if (_fields.ContainsKey(fieldName))
            {
                field = _fields[fieldName];
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
            if (TryGetField(fieldName, out LabeledNodeField field) && field is LabeledNodeField<T> genericField)
            {
                genericField.SetValue(value);
            }
        }

        protected T GetFieldValue<T>(string fieldName)
        {
            // Get value if the control exists
            if (TryGetField(fieldName, out LabeledNodeField field) && field is LabeledNodeField<T> genericField)
            {
                return genericField.GetValue();
            }
            else
            {
                return default;
            }
        }

        protected void AddField(LabeledNodeField labeledNodeField, string fieldName, VisualElement container = null)
        {
            if (container == null)
            {
                container = extensionContainer;
            }

            labeledNodeField.Create();

            container.Add(labeledNodeField.FieldContainer);
            _fields.Add(fieldName, labeledNodeField);
            RefreshExpandedState();
        }

        protected void RemoveField(string fieldName)
        {
            if (_fields.ContainsKey(fieldName))
            {
                // Find target field
                var targetField = _fields[fieldName].FieldContainer;

                // Find container
                var targetContainer = targetField.parent;

                // Remove field from container
                targetContainer.Remove(targetField);

                // Remove from container list
                _fields.Remove(fieldName);
                RefreshExpandedState();
            }
        }

        #endregion
    }
}