using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Alpha: Boolean", "Alpha")]
    public class AlphaBoolBinder : AbstractBinder<bool>
    {
        [SerializeField] private CanvasGroup target;

        [SerializeField, BindingType(typeof(bool))]
        private BindingField bindingField;

        [SerializeField] private bool invert;
        [SerializeField] private float fadeTime;
        
        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;
        
        protected override void OnBindingValueChanged()
        {
            bool value = bindableVariable.GetValue();
            if (invert) value = !value;
            Tween.Alpha(target, value ? 1 : 0, fadeTime);
        }
    }
}
