using UnityEngine;

namespace Daniell.Runtime.DialogueNodes
{
    /// <summary>
    /// Node Data for Dialogue Line Node
    /// </summary>
    public class DialogueLineNodeData : GraphNodeData
    {
        /// <summary>
        /// Character speaking during this line
        /// </summary>
        public Character Speaker { get => speaker; set => speaker = value; }
        [SerializeField] private Character speaker;

        /// <summary>
        /// Dialogue Line
        /// </summary>
        public string Line { get => line; set => line = value; }
        [SerializeField] private string line;
    }
}