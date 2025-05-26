using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DataBinding
{
    [Binder("TMP Enum Dropdown", "Dropdown")]
    public class TMPEnumDropdownBinder : AbstractBinder
    {
        [SerializeField] private TMP_Dropdown target;
        protected AbstractBindableVariable bindableVariable;
        [BindingType(typeof(Enum))] public BindingField bindingField;

        public sealed override void Bind(object obj, Type bindingType)
        {
            if (!target)
            {
#if DEBUG
                Debug.LogError($"Binder: {GetType().Name} does not have a target");
#endif
                return;
            }
            
            Unbind();

            try
            {
                bindableVariable = bindingField.GetBindingVariable(obj, bindingType);
            }
            catch (Exception e)
            {
                Debug.LogError($"Encountered error while binding {obj.GetType()} to {bindingField.property}: {e.Message}");
            }
            
            #if DEBUG
            if (bindableVariable == null)
            {
                Debug.Log("Could not get bindable enum, extend from BindableEnum<T> to create a concrete bindable enum");
                Debug.Log("Also make sure it has been initialised or has [Serializable] attribute");
            }
            #endif
            
            if (bindableVariable == null) return;
        
            bindableVariable.onValueChanged += OnBindingValueChanged;
            OnBindingValueChanged();
            
            List<string> options = new List<string>();
            Debug.Log($"Bind {bindableVariable.Type.Name}");
            foreach (var enumValue in Enum.GetValues(bindableVariable.Type))
            {
                Debug.Log($"{enumValue}");
                options.Add(bindableVariable.GetLocalisedEnumText(enumValue));   
            }
            target.AddOptions(options);
            
            target.onValueChanged.AddListener(OnInputFieldValueChanged);
        }
        
        public sealed override void Unbind()
        {
            target.ClearOptions();
            target.onValueChanged.RemoveListener(OnInputFieldValueChanged);
            
            if (bindableVariable == null) return;
            bindableVariable.onValueChanged -= OnBindingValueChanged;
            bindableVariable = null;
        }


        protected void OnBindingValueChanged()
        {
            target.SetValueWithoutNotify(Convert.ToInt32(bindableVariable.value));
        }
        
        private void OnInputFieldValueChanged(int newValue)
        {
            bindableVariable.value = newValue;
        }
    }
}
