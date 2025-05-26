using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Fill", "Image")]
    public class ImageFillBinder : AbstractBinder<float>
    {
        [SerializeField] private Image target;

        [SerializeField, BindingType(typeof(float))] private BindingField bindingField;

        [SerializeField] private bool smoothValue;
        [SerializeField] private float smoothTime;

        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBind()
        {
            target.fillAmount = bindableVariable.GetValue();
        }

        protected override void OnBindingValueChanged()
        {
            if (smoothValue)
            {
                Tween.UIFillAmount(target, bindableVariable.GetValue(), smoothTime);
            }
            else
            {
                target.fillAmount = bindableVariable.GetValue();
            }
        }
    }
}
