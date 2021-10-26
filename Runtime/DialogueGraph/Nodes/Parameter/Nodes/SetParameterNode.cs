using System;
using UnityEngine;

namespace Daniell.Runtime.DialogueNodes
{
    public class SetParameterNode : ParameterNode
    {
        public const string PARAMETER_VALUE_FIELD_NAME = "Value";

        protected override Color DefaultNodeColor => new Color32(44, 130, 201, 255);

        public SetParameterNode()
        {
            // Output Port
            AddOutputPort("Output");
        }

        protected override string GetName(string type)
        {
            return $"Set {type} Parameter";
        }

        protected override void ClearContent()
        {
            base.ClearContent();
            RemoveField(PARAMETER_VALUE_FIELD_NAME);
        }

        protected override void SetBool()
        {
            base.SetBool();
            AddField(new BoolNodeField("Value"), PARAMETER_VALUE_FIELD_NAME);

        }

        protected override void SetString()
        {
            base.SetString();
            AddField(new StringNodeField("Value", false), PARAMETER_VALUE_FIELD_NAME);
        }

        protected override void SetFloat()
        {
            base.SetFloat();
            AddField(new FloatNodeField("Value"), PARAMETER_VALUE_FIELD_NAME);
        }

        protected override void SetInt()
        {
            base.SetInt();
            AddField(new IntNodeField("Value"), PARAMETER_VALUE_FIELD_NAME);
        }
    }
}