using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataBinding
{
    public abstract class AbstractComplexTextBinder : AbstractBinder
    {
        [SerializeField] protected StringBinding formatString;

        private object _boundObj;
        private Type _bindingType;
        private List<AbstractBindableVariable> _bindingVariables = new List<AbstractBindableVariable>();
        public sealed override void Bind(object obj, Type bindingType)
        {
            Unbind();
            
            if(obj == null) return;

            _boundObj = obj;
            _bindingType = bindingType;
            
            foreach (var field in formatString.parameters)
            {
                _bindingVariables.Add(field.GetBindingVariable(obj, bindingType));
            }
            foreach (var bindableVariable in _bindingVariables)
            {
                bindableVariable.onValueChanged += OnValueChanged;
            }
            OnValueChanged();
        }

        public sealed override void Unbind()
        {
            foreach (var bindableVariable in _bindingVariables)
            {
                bindableVariable.onValueChanged -= OnValueChanged;
            }
            _bindingVariables.Clear();
        }

        protected abstract void OnValueChanged();

        protected virtual string GetBoundText()
        {
            return formatString.GetFormattedString(_boundObj, _bindingType);
        }

        public override void OnValidate()
        {
            formatString.OnValidate();
        }
    }
}
