using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Daniell.Runtime.DialogueNodes
{
    /// <summary>
    /// A Node that can be added to a graphview
    /// </summary>
    public abstract class GraphNode : BaseNode
    {
        /// <summary>
        /// Default name of the input port
        /// </summary>
        public const string DEFAULT_INPUT_NAME = "Input";

        /// <summary>
        /// GUID of the node
        /// </summary>
        public string GUID { get; private set; }

        /// <summary>
        /// Type of Node Data to be used. 
        /// </summary>
        protected virtual Type DataType => typeof(GraphNodeData);

        public GraphNode()
        {
            // Create a new GUID
            GUID = Guid.NewGuid().ToString();

            // Add the default input port
            AddInputPort(DEFAULT_INPUT_NAME);
        }

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

            // Assign connected GUIDs
            nodeData.ConnectedGUIDs = GetConnectedGUIDs();

            return nodeData;
        }

        public virtual void FromNodeData(GraphNodeData nodeData)
        {
            // Set node position
            SetPosition(nodeData.X, nodeData.Y);

            // Update GUID
            GUID = nodeData.GUID;
        }

        #endregion
    }

    public class CommentNode : BaseNode
    {
        protected override Color DefaultNodeColor => new Color32(80, 120, 113, 150);
        protected override string DefaultNodeName => "Comment";

        public CommentNode()
        {
            AddField(new StringNodeField("Comment", true), "field");
        }
    }
}