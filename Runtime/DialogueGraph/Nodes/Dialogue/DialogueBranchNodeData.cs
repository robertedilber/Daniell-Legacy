using System;
using System.Linq;
using UnityEngine;

namespace Daniell.Runtime.DialogueNodes
{
    /// <summary>
    /// Node Data for Dialogue Branch
    /// </summary>
    public class DialogueBranchNodeData : GraphNodeData
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

        /// <summary>
        /// Dialogue Branches
        /// </summary>
        public Branch[] Branches { get => _branches; set => _branches = value; }
        [SerializeField] private Branch[] _branches;

        // Private fields
        private int _chosenBranchIndex = -1;

        public override bool TryGetNextGUID(out string nextGUID)
        {
            // If no branch was chosen
            if (_chosenBranchIndex == -1)
            {
                // Return default value
                return base.TryGetNextGUID(out nextGUID);
            }
            // If a branch was chosen
            else
            {
                nextGUID = Branches.First(x => x.ID == _chosenBranchIndex).NextGUID;
                return nextGUID != null && nextGUID != "";
            }
        }

        public override void SetCustomData(params object[] customData)
        {
            base.SetCustomData(customData);

            // Expecting an ID
            if (customData.Length > 0)
            {
                var branchID = customData[0];
                _chosenBranchIndex = (int)branchID;
            }
        }

        /// <summary>
        /// A branch of a dialogue branch node. Set custom data using the ID.
        /// </summary>
        [Serializable]
        public struct Branch
        {
            /// <summary>
            /// Name of the branch
            /// </summary>
            public string Name { get => _name; set => _name = value; }
            [SerializeField] private string _name;

            /// <summary>
            /// ID of the branch
            /// </summary>
            public int ID { get => _id; set => _id = value; }
            [SerializeField] private int _id;

            /// <summary>
            /// Connected GUID for this branch
            /// </summary>
            public string NextGUID { get => _nextGUID; set => _nextGUID = value; }
            [SerializeField] private string _nextGUID;

            public Branch(string name, int id, string nextGUID)
            {
                _name = name;
                _id = id;
                _nextGUID = nextGUID;
            }
        }
    }
}