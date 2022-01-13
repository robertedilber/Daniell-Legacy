using Daniell.Runtime.Systems.Events;
using System;
using UnityEngine;

namespace Daniell.Runtime.Systems.DialogueNodes
{
    public class CallEventNode : TypeSelectorNode
    {
        public const string EVENT_FIELD_NAME = "Event";
        public const string EVENT_VALUE_FIELD_NAME = "Event Value";

        protected override Color DefaultNodeColor => new Color32(80, 150, 100, 255);

        public CallEventNode()
        {
            AddOutputPort("Output");
            SelectType("Bool");
        }

        protected override void ClearContent()
        {
            base.ClearContent();
            RemoveField(EVENT_FIELD_NAME);
            RemoveField(EVENT_VALUE_FIELD_NAME);
        }

        protected override void SetBool()
        {
            base.SetBool();
            AddField(new ObjectNodeField<BoolEvent>("Event"), EVENT_FIELD_NAME);
            AddField(new BoolNodeField("Event Value"), EVENT_VALUE_FIELD_NAME);
        }

        protected override void SetString()
        {
            base.SetString();
            AddField(new ObjectNodeField<StringEvent>("Event"), EVENT_FIELD_NAME);
            AddField(new StringNodeField("Event Value", false), EVENT_VALUE_FIELD_NAME);
        }

        protected override void SetFloat()
        {
            base.SetFloat();
            AddField(new ObjectNodeField<FloatEvent>("Event"), EVENT_FIELD_NAME);
            AddField(new FloatNodeField("Event Value"), EVENT_VALUE_FIELD_NAME);
        }

        protected override void SetInt()
        {
            base.SetInt();
            AddField(new ObjectNodeField<IntEvent>("Event"), EVENT_FIELD_NAME);
            AddField(new IntNodeField("Event Value"), EVENT_VALUE_FIELD_NAME);
        }

        protected override string GetName(string type)
        {
            return $"Call {type} Event";
        }
    }
}