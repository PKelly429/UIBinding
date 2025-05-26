using TMPro;
using UnityEngine;

namespace DataBinding
{
    [Binder("TMP Input Field", "Text")]
    public class InputFieldBinder : AbstractBinder<string>
    {
        [SerializeField] private TMP_InputField target;
        [BindingType(typeof(string))] public BindingField bindingField;
        
        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBind()
        {
            target.onValueChanged.AddListener(OnInputFieldValueChanged);
        }

        protected override void OnUnbind()
        {
            target.onValueChanged.RemoveListener(OnInputFieldValueChanged);
        }
    
        protected override void OnBindingValueChanged()
        {
            target.SetTextWithoutNotify(bindableVariable.stringValue);
        }

        private void OnInputFieldValueChanged(string newValue)
        {
            bindableVariable.SetValue(newValue);
        }
    }
}
