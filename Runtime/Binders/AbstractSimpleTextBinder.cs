using System;
using UnityEngine;

namespace DataBinding
{
    public abstract class AbstractSimpleTextBinder : AbstractBinder
    {
        [SerializeField] private BindingField bindingField;

        protected AbstractBindableVariable bindableVariable;

        public sealed override void Bind(object obj, Type bindingType)
        {
            Unbind();

            bindableVariable = bindingField.GetBindingVariable(obj, bindingType);
            if (bindableVariable == null) return;

            bindableVariable.onValueChanged += OnValueChanged;
            OnValueChanged();
        }

        public sealed override void Unbind()
        {
            if (bindableVariable == null) return;
            bindableVariable.onValueChanged -= OnValueChanged;
            bindableVariable = null;
        }

        protected abstract void OnValueChanged();

        protected string GetBoundText()
        {
            return bindableVariable.stringValue;
        }
    }
}
