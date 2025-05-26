using System;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Slider: Bound Max", "Slider")]
    public class SliderBinder : AbstractBinder
    {
        public Slider target;
        
        private BindableVariable<float> current;
        private BindableVariable<float> max;

        [Header("Current"),BindingType(typeof(float))] public BindingField currentBindingField;
        [Header("Max"),BindingType(typeof(float))] public BindingField maxBindingField;
        
        public sealed override void Bind(object obj, Type bindingType)
        {
            if (!target)
            {
#if DEBUG
                Debug.LogError($"Binder: Slider does not have a target");
#endif
                return;
            }
            
            Unbind();
            if (obj == null) return;

            try
            {
                current = currentBindingField.GetBindingVariable(obj, bindingType) as BindableVariable<float>;
                max = maxBindingField.GetBindingVariable(obj, bindingType) as BindableVariable<float>;
            }
            catch (Exception e)
            {
                Debug.LogError($"Encountered error while binding {obj.GetType()} to Slider: {e.Message}");
            }

            if (current == null || max == null) return;
        
            current.onValueChanged += OnBindingValueChanged;
            max.onValueChanged += OnBindingValueChanged;
            OnBindingValueChanged();
            
            target.onValueChanged.AddListener(SliderValueChanged);
        }

        public sealed override void Unbind()
        {
            if(!target) return;
            
            target.onValueChanged.RemoveListener(SliderValueChanged);
            if (current == null || max == null) return;
            
            current.onValueChanged -= OnBindingValueChanged;
            max.onValueChanged -= OnBindingValueChanged;
            current = null;
            max = null;
        }

        protected void OnBindingValueChanged()
        {
            target.maxValue = max.GetValue();
            target.SetValueWithoutNotify(current.GetValue());
        }

        private void SliderValueChanged(float newValue)
        {
            current.SetValue(newValue);
        }
    }
}