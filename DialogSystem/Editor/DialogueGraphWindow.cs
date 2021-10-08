using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphWindow : EditorWindow
{
    // Properties
    private List<DialogueBranchNode> DialogueNodes => _graphView?.nodes.ToList().Cast<DialogueBranchNode>().ToList();

    // Private fields
    private DialogueGraphView _graphView;
    private Toolbar _toolbar;
    private DialogueFile _dialogueFile;

    [MenuItem("Daniell/Dialogue Graph")]
    public static void Open()
    {
        var window = GetWindow<DialogueGraphWindow>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    #region Init / Deinit

    private void OnEnable()
    {
        // Create the toolbar
        CreateToolbar();
    }

    private void OnDisable()
    {
        // Remove the toolbar
        RemoveToolbar();

        // Remove the graph view
        RemoveGraphView();
    }

    #endregion

    #region Toolbar

    /// <summary>
    /// Create the toolbar
    /// </summary>
    private void CreateToolbar()
    {
        if (_toolbar == null)
        {
            _toolbar = new Toolbar();

            // Save button
            CreateToolbarButton("Save", () => Save());

            // Dialogue File loading field
            ObjectField scriptableDialogFile = new ObjectField();
            scriptableDialogFile.objectType = typeof(DialogueFile);
            scriptableDialogFile.RegisterValueChangedCallback(x => OnDialogueFileValueChanged((DialogueFile)x.newValue));
            _toolbar.Add(scriptableDialogFile);

            // Add the toolbar to the window
            rootVisualElement.Add(_toolbar);
        }

        void CreateToolbarButton(string name, Action action)
        {
            var button = new Button(action);
            button.text = name;
            _toolbar.Add(button);
        }
    }

    /// <summary>
    /// Remove the toolbar
    /// </summary>
    private void RemoveToolbar()
    {
        if (_toolbar != null)
        {
            rootVisualElement.Remove(_toolbar);
        }
    }

    #endregion

    #region GraphView

    /// <summary>
    /// Create the GraphView
    /// </summary>
    private void CreateGraphView()
    {
        if (_graphView == null)
        {
            // Create the graph view instance
            _graphView = new DialogueGraphView("Dialogue Graph", this);

            // Stretch view to container
            _graphView.StretchToParentSize();

            // Add to the window
            rootVisualElement.Add(_graphView);

            // Send the view to the back so that the toolbar stays on top
            _graphView.SendToBack();
        }
    }

    /// <summary>
    /// Remove the GraphView
    /// </summary>
    private void RemoveGraphView()
    {
        if (_graphView != null)
        {
            // Remove from the window
            rootVisualElement.Remove(_graphView);

            // Reset reference
            _graphView = null;
        }
    }

    #endregion

    /// <summary>
    /// Called when the dialogue file value has changed
    /// </summary>
    /// <param name="dialogueFile">New Dialogue file</param>
    private void OnDialogueFileValueChanged(DialogueFile dialogueFile)
    {
        // Cache value
        _dialogueFile = dialogueFile;

        if (dialogueFile != null)
        {
            // Create the graph
            CreateGraphView();

            // Load the graph
            Load();
        }
        else
        {
            // Remove the graph
            RemoveGraphView();
        }
    }

    #region Save / Load

    private void Save()
    {
        // Get Nodes
        List<GraphNode> nodes = _graphView.nodes.ToList().Cast<GraphNode>().ToList();

        // Cache dialogue file path
        string dialogueFilePath = AssetDatabase.GetAssetPath(_dialogueFile);

        // Load sub assets 
        UnityEngine.Object[] dialogueFileSubAssets = AssetDatabase.LoadAllAssetsAtPath(dialogueFilePath);

        // Remove all sub assets from the file
        for (int i = 0; i < dialogueFileSubAssets.Length; i++)
        {
            UnityEngine.Object dialogueFileSubAsset = dialogueFileSubAssets[i];
            if (dialogueFileSubAsset != _dialogueFile)
            {
                // Remove object from the asset
                AssetDatabase.RemoveObjectFromAsset(dialogueFileSubAsset);
                AssetDatabase.SaveAssets();
            }
        }

        // Clear dialogue file refs
        _dialogueFile.Clear();

        // Get Node Data from nodes and add to the dialogue file
        for (int i = 0; i < nodes.Count; i++)
        {
            GraphNode node = nodes[i];

            // If this node is saveable
            if (node.IsSaveable)
            {
                // Get node data
                GraphNodeData nodeData = node.ToNodeData();

                // Create the new scriptable object and add to the dialogue file
                if (nodeData != null)
                {
                    // Set sciptable node data name
                    nodeData.name = nodeData.GetType().Name;

                    // Add the node data to the dialogue file
                    AssetDatabase.AddObjectToAsset(nodeData, dialogueFilePath);

                    _dialogueFile.AddNodeData(nodeData);

                    // Save and refresh assets
                    AssetDatabase.SaveAssets();
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(nodeData));
                }
            }
        }
    }

    private void Load()
    {
        // If a file is loaded
        if (_dialogueFile != null && _graphView != null)
        {
            // Create nodes using sub assets
            foreach (GraphNodeData nodeData in _dialogueFile)
            {
                // Create nodes
                var method = typeof(DialogueGraphView).GetMethod(nameof(DialogueGraphView.CreateNode));
                var action = method.MakeGenericMethod(Type.GetType(nodeData.NodeTypeName));
                var node = action.Invoke(_graphView, null);
                ((GraphNode)node).FromNodeData(nodeData);
            }
        }
    }

    #endregion
}