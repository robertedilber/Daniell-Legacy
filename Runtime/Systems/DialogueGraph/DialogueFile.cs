using Daniell.Runtime.Helpers.DataStructures;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Daniell.Runtime.Systems.DialogueNodes
{
    [CreateAssetMenu]
    public class DialogueFile : ScriptableObject
    {
        /// <summary>
        /// The GUID of the node connected to the start node
        /// </summary>
        public string StartNodeConnectedGUID { get => _startNodeConnectedGUID; set => _startNodeConnectedGUID = value; }
        [SerializeField] private string _startNodeConnectedGUID;

        [SerializeField]
        private SerializableDictionary<string, GraphNodeData> _nodeDatas = new SerializableDictionary<string, GraphNodeData>();

        public int NodeDataCount => _nodeDatas.Count;

        public GraphNodeData this[int i] => _nodeDatas[i].Value;
        public GraphNodeData this[string guid] => _nodeDatas[guid];

#if UNITY_EDITOR

        /// <summary>
        /// Get valid nodes for this file
        /// </summary>
        /// <returns>List of valid types for this file</returns>
        public virtual Dictionary<Type, string> GetValidNodeTypes()
        {
            return new Dictionary<Type, string> {
            {typeof(DialogueLineNode) , "Dialogue"},
            {typeof(DialogueBranchNode), "Dialogue"},
            {typeof(GetParameterNode), "Parameters"},
            {typeof(SetParameterNode), "Parameters"},
            {typeof(CallEventNode), "Event"},
            {typeof(CommentNode), "Utilities"}
        };
        }

        #region Node Data Management

        /// <summary>
        /// Add node data asset to dialogue file
        /// </summary>
        /// <param name="graphNodeData">Node Data to add</param>
        public void AddNodeData(GraphNodeData graphNodeData)
        {
            // Add node data to the list
            _nodeDatas.Add(new SerializableKeyValuePair<string, GraphNodeData>(graphNodeData.GUID, graphNodeData));

            // Add the node data to the dialogue file
            AssetDatabase.AddObjectToAsset(graphNodeData, AssetDatabase.GetAssetPath(this));

            // Save the assets
            AssetDatabase.SaveAssets();

            // Import the new asset
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(graphNodeData));
        }

        /// <summary>
        /// Remove node data asset from dialogue file
        /// </summary>
        /// <param name="graphNodeData">Node Data to remove</param>
        public void RemoveNodeData(GraphNodeData graphNodeData)
        {
            //_nodeDatas.Remove();

            // Remnove the node data from the dialogue file
            AssetDatabase.RemoveObjectFromAsset(graphNodeData);

            // Save the assets
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Clear the dialogue file data
        /// </summary>
        public void Clear()
        {
            // Load assets
            var nodeDatas = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this));

            // Remove all sub assets
            for (int i = 0; i < nodeDatas.Length; i++)
            {
                UnityEngine.Object nodeData = nodeDatas[i];
                if (nodeData is GraphNodeData data)
                {
                    AssetDatabase.RemoveObjectFromAsset(data);
                    AssetDatabase.SaveAssets();
                }
            }

            // Clear the list
            _nodeDatas.Clear();
        }

        #endregion

#endif

        #region Read Nodes

        public bool TryGetNextNodeData(GraphNodeData currentNodeData, out GraphNodeData nextNodeData)
        {
            var isNextNodeValid = currentNodeData.TryGetNextGUID(out string nextGUID);
            nextNodeData = null;
            if (isNextNodeValid)
            {
                nextNodeData = _nodeDatas[nextGUID];
            }

            return isNextNodeValid;
        }

        public GraphNodeData GetStartNode()
        {
            if (StartNodeConnectedGUID != null && StartNodeConnectedGUID != "")
            {
                return _nodeDatas[StartNodeConnectedGUID];
            }

            return null;
        }

        #endregion
    }
}