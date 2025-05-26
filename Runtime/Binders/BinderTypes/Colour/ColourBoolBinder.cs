using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Colour: Boolean", "Colour")]
    public class ColourBoolBinder : AbstractBinder<bool>
    {
        [SerializeField] private Graphic target;
        [BindingType(typeof(bool))] public BindingField bindingField;

        [SerializeField] private Color trueColour;
        [SerializeField] private Color falseColour;
        
        [SerializeField] private bool smoothValue;
        [SerializeField] private float smoothTime;

        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBindingValueChanged()
        {
            Color targetColour = bindableVariable.GetValue() ? trueColour : falseColour;
            if (smoothValue)
            {
                Tween.Color(target, targetColour, smoothTime);
            }
            else
            {
                target.color = targetColour;
            }
        }
    }
}
