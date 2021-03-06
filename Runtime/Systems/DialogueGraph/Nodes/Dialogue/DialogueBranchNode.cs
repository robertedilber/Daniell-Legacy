using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Daniell.Runtime.Systems.DialogueNodes
{
    /// <summary>
    /// A node from the dialogue graph view.
    /// </summary>
    public class DialogueBranchNode : GraphNode
    {
        public const string SPEAKER_FIELD_NAME = "Speaker";
        public const string LINE_FIELD_NAME = "Line";

        protected const int DEFAULT_BRANCH_COUNT = 2;
        protected override Color DefaultNodeColor => new Color32(160, 100, 56, 255);
        protected override string DefaultNodeName => "Dialogue Branch";
        protected override Type DataType => typeof(DialogueBranchNodeData);

        public DialogueBranchNode()
        {
            // Add a branch button
            Button branchButton = new Button(() => AddBranchPort());
            branchButton.text = "Add Branch";
            branchButton.style.marginRight = 3;
            titleContainer.Add(branchButton);

            AddField(new ObjectNodeField<Character>("Speaker"), SPEAKER_FIELD_NAME);
            AddField(new StringNodeField("Line", true), LINE_FIELD_NAME);

            // Add default branches
            for (int i = 0; i < DEFAULT_BRANCH_COUNT; i++)
            {
                AddBranchPort();
            }
        }

        protected void AddBranchPort()
        {
            // Add the output port
            Port port = AddOutputPort(Guid.NewGuid().ToString());

            // Set Port label
            ResetBranchNumbers();

            // Add remove button
            Button button = new Button(() =>
            {
                outputContainer.Remove(port);
                ResetBranchNumbers();
            });
            button.text = "X";
            port.contentContainer.Add(button);

            // Add Text field
            TextField textField = new TextField();
            textField.style.width = 150;
            port.contentContainer.Add(textField);

            void ResetBranchNumbers()
            {
                int idx = 1;
                foreach (VisualElement e in outputContainer.Children())
                {
                    Label portLabel = e.contentContainer.Q<Label>("type");
                    portLabel.text = idx.ToString();
                    idx++;
                    portLabel.style.width = 20;
                }
            }
        }

        public override GraphNodeData ToNodeData()
        {
            DialogueBranchNodeData data = (DialogueBranchNodeData)base.ToNodeData();

            return data;
        }

        public override void FromNodeData(GraphNodeData nodeData)
        {
            base.FromNodeData(nodeData);
        }
    }
}