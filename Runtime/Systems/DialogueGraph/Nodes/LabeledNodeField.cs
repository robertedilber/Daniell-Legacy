using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Daniell.Runtime.Systems.DialogueNodes
{
    public abstract class LabeledNodeField
    {
        public VisualElement FieldContainer { get; private set; }
        public VisualElement Field { get; private set; }

        private string _labelText;

        public LabeledNodeField(string labelText)
        {
            _labelText = labelText;
        }

        public void Create()
        {
            FieldContainer = CreateFieldContainer(_labelText);
        }

        protected virtual VisualElement CreateFieldContainer(string labelText)
        {
            // Create a new label
            Label label = new Label();
            label.text = $"• {labelText}";
            label.style.marginLeft = 3;

            // Create a container for the label
            Box box = new Box();
            box.style.borderBottomLeftRadius = box.style.borderBottomRightRadius = box.style.borderTopLeftRadius = box.style.borderTopRightRadius = 5;
            box.style.marginBottom = 5;
            box.Add(label);

            // Create the field
            Field = CreateField();
            box.Add(Field);

            return box;
        }

        protected abstract VisualElement CreateField();
    }

    public abstract class LabeledNodeField<TFieldValue> : LabeledNodeField
    {
        /// <summary>
        /// Called when the value of the field has changed
        /// </summary>
        public event Action<TFieldValue> OnFieldValueChanged;

        public LabeledNodeField(string labelText) : base(labelText) { }

        #region Base Field Implementation

        /// <summary>
        /// Set the value of this field
        /// </summary>
        /// <param name="fieldValue">New field value</param>
        public abstract void SetValue(TFieldValue fieldValue);

        /// <summary>
        /// Get the value of this field
        /// </summary>
        /// <returns>Value of the field</returns>
        public abstract TFieldValue GetValue();

        #endregion

        #region Implicit Cast Operators

        public static implicit operator TFieldValue(LabeledNodeField<TFieldValue> labeledField)
        {
            return labeledField.GetValue();
        }

        public static implicit operator VisualElement(LabeledNodeField<TFieldValue> labeledField)
        {
            return labeledField.FieldContainer;
        }

        #endregion
    }

    public class SimpleNodeField<TFieldType, TFieldValue> : LabeledNodeField<TFieldValue> where TFieldType : BaseField<TFieldValue>, new()
    {
        public SimpleNodeField(string labelText) : base(labelText) { }

        private TFieldType _field;

        public override TFieldValue GetValue()
        {
            return _field.value;
        }

        public override void SetValue(TFieldValue fieldValue)
        {
            _field.value = fieldValue;
        }

        protected override VisualElement CreateField()
        {
            _field = new TFieldType();
            return _field;
        }
    }

    public class StringNodeField : SimpleNodeField<TextField, string>
    {
        private bool _isMultiline = false;

        public StringNodeField(string labelText, bool isMultiline) : base(labelText)
        {
            _isMultiline = isMultiline;
        }

        protected override VisualElement CreateField()
        {
            TextField field = (TextField)base.CreateField();
            field.multiline = _isMultiline;
            return field;
        }
    }

    public class IntNodeField : SimpleNodeField<IntegerField, int>
    {
        public IntNodeField(string labelText) : base(labelText) { }
    }

    public class FloatNodeField : SimpleNodeField<FloatField, float>
    {
        public FloatNodeField(string labelText) : base(labelText) { }
    }

    public class BoolNodeField : SimpleNodeField<Toggle, bool>
    {
        public BoolNodeField(string labelText) : base(labelText) { }
    }

    public class ObjectNodeField<T> : LabeledNodeField<T> where T : UnityEngine.Object
    {
        private ObjectField _objectField;

        public ObjectNodeField(string labelText) : base(labelText) { }

        public override T GetValue()
        {
            return (T)_objectField.value;
        }

        public override void SetValue(T fieldValue)
        {
            _objectField.value = fieldValue;
        }

        protected override VisualElement CreateField()
        {
            _objectField = new ObjectField();
            _objectField.objectType = typeof(T);
            return _objectField;
        }
    }

    public class DropdownNodeField<T> : LabeledNodeField<T>
    {
        private PopupField<T> _popupField;
        private List<T> _options;

        public DropdownNodeField(string labelText, params T[] options) : base(labelText)
        {
            _options = options.ToList();
        }

        public override T GetValue()
        {
            return _popupField.value;
        }

        public override void SetValue(T fieldValue)
        {
            _popupField.value = fieldValue;
        }

        protected override VisualElement CreateField()
        {
            if (_options.Count > 0)
            {
                _popupField = new PopupField<T>(_options, 0);
            }
            return _popupField;
        }
    }
}