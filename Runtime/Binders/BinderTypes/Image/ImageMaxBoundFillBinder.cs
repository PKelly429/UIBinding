using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Fill: Bound Max", "Image")]
    public class ImageMaxBoundFillBinder : AbstractBinder
    {
        [SerializeField] private Image target;

        private BindableVariable<float> current;
        private BindableVariable<float> max;

        [SerializeField, Header("Current"), BindingType(typeof(float))] private BindingField currentBindingField;
        [SerializeField, Header("Max"), BindingType(typeof(float))] private BindingField maxBindingField;

        [SerializeField] private bool smoothValue;
        [SerializeField] private float smoothTime;

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
            
            target.fillAmount = GetValue();
        }

        public sealed override void Unbind()
        {
            if(!target) return;
            
            if (current == null || max == null) return;
            
            current.onValueChanged -= OnBindingValueChanged;
            max.onValueChanged -= OnBindingValueChanged;
            current = null;
            max = null;
        }

        protected void OnBindingValueChanged()
        {
            if (smoothValue)
            {
                Tween.UIFillAmount(target, GetValue(), smoothTime);
            }
            else
            {
                target.fillAmount = GetValue();
            }
        }

        private float GetValue()
        {
            return Mathf.Clamp01(current.GetValue() / max.GetValue());
        }
    }
}
