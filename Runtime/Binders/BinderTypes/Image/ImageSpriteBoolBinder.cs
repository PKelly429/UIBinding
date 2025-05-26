using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Sprite based on Boolean", "Image")]
    public class ImageSpriteBoolBinder : AbstractBinder<bool>
    {
        [SerializeField] private Image target;

        [SerializeField, BindingType(typeof(bool))]
        private BindingField bindingField;
        
        [SerializeField] private Sprite trueSprite;
        [SerializeField] private Sprite falseSprite;
        
        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;
        
        protected override void OnBindingValueChanged()
        {
            target.sprite = bindableVariable.GetValue() ? trueSprite : falseSprite;
        }
    }
}
