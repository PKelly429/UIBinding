using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Toggle", "Toggle")]
    public class ToggleBinder : AbstractBinder<bool>
    {
        [SerializeField] private Toggle target;
        [SerializeField, BindingType(typeof(bool))] private BindingField bindingField;
        [SerializeField] private bool twoWayBinding = true;

        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBind()
        {
            if(!twoWayBinding) return;
            target.onValueChanged.AddListener(OnToggleFieldValueChanged);
        }

        protected override void OnUnbind()
        {
            if(!twoWayBinding) return;
            target.onValueChanged.RemoveListener(OnToggleFieldValueChanged);
        }
    
        protected override void OnBindingValueChanged()
        {
            target.SetIsOnWithoutNotify(bindableVariable.GetValue());
        }

        private void OnToggleFieldValueChanged(bool newValue)
        {
            bindableVariable.SetValue(newValue);
        }
    }
}
