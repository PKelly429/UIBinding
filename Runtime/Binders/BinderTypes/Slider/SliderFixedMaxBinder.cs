using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Slider: Fixed Max", "Slider")]
    public class SliderFixedMaxBinder : AbstractBinder<float>
    {
        [SerializeField] private Slider target;
        [BindingType(typeof(float))] public BindingField bindingField;
        [SerializeField] private float max;
        
        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBind()
        {
            target.maxValue = max;
            target.onValueChanged.AddListener(OnSliderFieldValueChanged);
        }

        protected override void OnUnbind()
        {
            target.onValueChanged.RemoveListener(OnSliderFieldValueChanged);
        }
    
        protected override void OnBindingValueChanged()
        {
            target.SetValueWithoutNotify(bindableVariable.GetValue());
        }

        private void OnSliderFieldValueChanged(float newValue)
        {
            bindableVariable.SetValue(newValue);
        }
    }
}