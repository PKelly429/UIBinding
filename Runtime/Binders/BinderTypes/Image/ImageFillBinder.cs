using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Fill: Fixed Max", "Image")]
    public class ImageFillBinder : AbstractBinder<float>
    {
        [SerializeField] private Image target;

        [SerializeField, BindingType(typeof(float))] private BindingField bindingField;

        [SerializeField] private float maxValue = 1;
        [SerializeField] private bool smoothValue;
        [SerializeField] private float smoothTime;

        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBind()
        {
            target.fillAmount = GetValue();
        }

        protected override void OnBindingValueChanged()
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
            return Mathf.Clamp01(bindableVariable.GetValue() / maxValue);
        }
    }
}
