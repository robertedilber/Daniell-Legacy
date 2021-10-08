using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private EditorWindow EditorWindow { get; set; }
    private DialogueGraphView GraphView { get; set; }

    public void Initialize(EditorWindow editorWindow, DialogueGraphView dialogueGraphView)
    {
        EditorWindow = editorWindow;
        GraphView = dialogueGraphView;
    }

    private struct NodeCreationContext
    {
        public Func<GraphNode> CreateNodeAction { get; private set; }

        public NodeCreationContext(Func<GraphNode> createNodeAction)
        {
            CreateNodeAction = createNodeAction;
        }

        public void CreateNode(out GraphNode node)
        {
            node = CreateNodeAction?.Invoke();
        }
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>()
        {
            // Header
            GetGroup("Create Nodes", 0),

            // Dialogue node group
            GetGroup("Dialogue Nodes", 1),

            // Dialogue node group content
            GetEntry<DialogueLineNode>(2),
            GetEntry<DialogueBranchNode>(2),

            // Parameter node group
            GetGroup("Parameter Nodes", 1),
            GetEntry<SetParameterNode>(2),
            GetEntry<GetParameterNode>(2),


            GetGroup("Event Nodes", 1),
            GetEntry<CallEventNode>(2)
        };

        SearchTreeEntry GetEntry<T>(int level) where T : GraphNode, new()
        {
            var nodeName = Regex.Replace(typeof(T).ToString(), @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
            var searchTreeEntry = new SearchTreeEntry(new GUIContent(nodeName));
            searchTreeEntry.userData = new NodeCreationContext(() =>
            {
                var node = GraphView.CreateNode<T>();
                return node;
            });

            searchTreeEntry.level = level;

            return searchTreeEntry;
        }

        SearchTreeGroupEntry GetGroup(string groupName, int level)
        {
            return new SearchTreeGroupEntry(new GUIContent(groupName), level);
        }

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        NodeCreationContext userData = (NodeCreationContext)searchTreeEntry.userData;
        userData.CreateNode(out GraphNode node);

        var worldMousePosition = EditorWindow.rootVisualElement.ChangeCoordinatesTo(EditorWindow.rootVisualElement.parent, context.screenMousePosition - EditorWindow.position.position);
        var localMousePosition = GraphView.contentViewContainer.WorldToLocal(worldMousePosition);

        node.SetPosition((int)localMousePosition.x, (int)localMousePosition.y);

        return true;
    }
}