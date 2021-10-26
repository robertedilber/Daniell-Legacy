using Daniell.Runtime.Helpers.DataStructures;
using System.Linq;
using UnityEngine;

namespace Daniell.Runtime.DialogueNodes
{
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
        public string GUID { get => _guid; set => _guid = value; }
        [SerializeField] private string _guid;

        /// <summary>
        /// GUIDs of nodes connected to this one
        /// </summary>
        public SerializableDictionary<string, string> ConnectedGUIDs { get => _connectedGUIDs; set => _connectedGUIDs = value; }
        [SerializeField] private SerializableDictionary<string, string> _connectedGUIDs = new SerializableDictionary<string, string>();

        /// <summary>
        /// Try to get the next connected node
        /// </summary>
        /// <param name="nextGUID">Next found GUID</param>
        /// <returns>True if there is a valid connected node</returns>
        public virtual bool TryGetNextGUID(out string nextGUID)
        {
            nextGUID = "";

            if (_connectedGUIDs.Count > 0)
            {
                // Get the first connected port
                nextGUID = _connectedGUIDs.First(x => x.Value != null && x.Value != "").Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Set custom data to be used when this node is processed
        /// </summary>
        /// <param name="customData">Data to be used by the node processor</param>
        public virtual void SetCustomData(params object[] customData)
        {
            // Do nothing
        }
    }
}