using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Sprite", "Image")]
    public class ImageSpriteBinder : AbstractBinder<Sprite>
    {
        [SerializeField] private Image target;

        [SerializeField, BindingType(typeof(Sprite))]
        private BindingField bindingField;
        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;
        
        protected override void OnBindingValueChanged()
        {
            target.sprite = bindableVariable.GetValue();
        }
    }
}
