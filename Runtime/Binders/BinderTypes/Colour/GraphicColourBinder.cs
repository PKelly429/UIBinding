using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Colour", "Colour")]
    public class GraphicColourBinder : AbstractBinder<Color>
    {
        [SerializeField] private Graphic target;
        [BindingType(typeof(Color))] public BindingField bindingField;

        [SerializeField] private bool smoothValue;
        [SerializeField] private float smoothTime;

        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBindingValueChanged()
        {
            if (smoothValue)
            {
                Tween.Color(target, bindableVariable.GetValue(), smoothTime);
            }
            else
            {
                target.color = bindableVariable.GetValue();
            }
        }
    }
}
