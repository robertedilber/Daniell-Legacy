using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Daniell.Runtime.Systems.DialogueNodes
{
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

            titleContainer.Add(typeSelector);
        }

        public void SelectType(string type)
        {
            ClearContent();
            title = GetName(SelectedType.ToString());

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

        protected virtual void SetBool()
        {
            SelectedType = E_NodeType.Bool;
        }

        protected virtual void SetString()
        {
            SelectedType = E_NodeType.String;
        }

        protected virtual void SetFloat()
        {
            SelectedType = E_NodeType.Float;
        }

        protected virtual void SetInt()
        {
            SelectedType = E_NodeType.Int;
        }
    }
}