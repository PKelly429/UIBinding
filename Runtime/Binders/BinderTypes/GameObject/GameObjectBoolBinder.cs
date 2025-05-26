using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Active based on Boolean", "Game Object")]
    public class GameObjectBoolBinder : AbstractBinder<bool>
    {
        [SerializeField] private GameObject target;

        [SerializeField, BindingType(typeof(bool))]
        private BindingField bindingField;

        [SerializeField] private bool invert;
        [SerializeField] private float disableDelay;
        
        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;
        
        private Tween _currentTween;

        protected override void OnBind()
        {
            bool value = bindableVariable.GetValue();
            if(invert) value = !value;
            target.SetActive(value);
        }
        
        protected override void OnBindingValueChanged()
        {
            bool value = bindableVariable.GetValue();
            if(invert) value = !value;
            
            if(target.activeSelf == value) return;
            
            _currentTween.Stop();

            if (value)
            {
                target.SetActive(true);
                return;
            }

            if (disableDelay > 0)
            {
                _currentTween = Tween.Delay(target, disableDelay).OnComplete(() => target.SetActive(false));
            }
            else
            {
                target.SetActive(false);
            }
        }
    }
}
