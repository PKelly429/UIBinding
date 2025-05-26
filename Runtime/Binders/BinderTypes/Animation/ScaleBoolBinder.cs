using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Scale: Boolean", "Animation")]
    public class ScaleBoolBinder : AbstractBinder<bool>
    {
        [SerializeField] private RectTransform target;

        [SerializeField, BindingType(typeof(bool))]
        private BindingField bindingField;

        [SerializeField] private Vector3 falseScale = Vector3.zero;
        [SerializeField] private Vector3 trueScale = Vector3.one;
        
        [SerializeField] private float scaleTime = 0.1f;
        
        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;
        

        protected override void OnBind()
        {
            bool value = bindableVariable.GetValue();
            target.localScale = value ? trueScale : falseScale;
        }
        
        protected override void OnBindingValueChanged()
        {
            bool value = bindableVariable.GetValue();
            
            Tween.Scale(target, value ? trueScale : falseScale, scaleTime);
        }
    }
}
