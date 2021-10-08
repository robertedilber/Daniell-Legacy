using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public abstract class TypeSelectorNode : GraphNode
{
    public enum E_NodeType
    {
        Bool,
        String,
        Float,
        Int
    }

    public E_NodeType SelectedType { get; private set; }

    protected override string DefaultNodeName => "";

    public TypeSelectorNode()
    {
        PopupField<string> typeSelector = new PopupField<string>(new List<string> { "Bool", "String", "Float", "Int" }, 0);
        typeSelector.RegisterValueChangedCallback((x) => SelectType(x.newValue));

        typeSelector.style.width = 100;
        typeSelector.style.height = 30;
        typeSelector.style.marginTop = 3;
        typeSelector.style.marginRight = 3;

        SetBool();

        titleContainer.Add(typeSelector);
    }

    public void SelectType(string type)
    {
        switch (type)
        {
            case "Bool":
                SetBool();
                break;
            case "String":
                SetString();
                break;
            case "Float":
                SetFloat();
                break;
            case "Int":
                SetInt();
                break;
        }
    }

    protected abstract string GetName(string type);

    protected virtual void ClearContent() { }

    protected virtual void DrawDefaultContent() { }

    protected virtual void SetBool()
    {
        ClearContent();
        DrawDefaultContent();
        SelectedType = E_NodeType.Bool;
        title = GetName(SelectedType.ToString());
    }

    protected virtual void SetString()
    {
        ClearContent();
        DrawDefaultContent();
        SelectedType = E_NodeType.String;
        title = GetName(SelectedType.ToString());
    }

    protected virtual void SetFloat()
    {
        ClearContent();
        DrawDefaultContent();
        SelectedType = E_NodeType.Float;
        title = GetName(SelectedType.ToString());
    }

    protected virtual void SetInt()
    {
        ClearContent();
        DrawDefaultContent();
        SelectedType = E_NodeType.Int;
        title = GetName(SelectedType.ToString());
    }

    protected void ClearContentContainer(VisualElement container)
    {
        List<VisualElement> toRemove = new List<VisualElement>();

        // Get objects to remove
        foreach (var v in container.Children())
        {
            toRemove.Add(v);
        }

        // Remove all from hierarchy
        for (int i = 0; i < toRemove.Count; i++)
        {
            toRemove[i].RemoveFromHierarchy();
        }
    }
}
