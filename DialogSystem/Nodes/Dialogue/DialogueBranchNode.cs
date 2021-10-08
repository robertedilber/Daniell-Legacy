using Daniell.DialogSystem;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A node from the dialogue graph view.
/// </summary>
public class DialogueBranchNode : GraphNode
{
    protected const int DEFAULT_BRANCH_AMOUNT = 2;
    protected override Color DefaultNodeColor => new Color32(160, 100, 56, 255);
    protected override string DefaultNodeName => "Dialogue Branch";
    protected override Type DataType => typeof(object);

    public DialogueBranchNode()
    {
        // Add Input
        AddInputPort("Input");

        // Add a branch button
        Button branchButton = new Button(() => AddBranchPort());
        branchButton.text = "Add Branch";
        branchButton.style.marginRight = 3;
        titleContainer.Add(branchButton);

        AddObjectField<Character>("Speaker");

        AddTextfield("Line", multiline: true);

        // Add default branches
        for (int i = 0; i < DEFAULT_BRANCH_AMOUNT; i++)
        {
            AddBranchPort();
        }
    }

    protected void AddBranchPort()
    {
        // Add the output port
        AddOutputPort(Guid.NewGuid().ToString(), out Port port);

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
}