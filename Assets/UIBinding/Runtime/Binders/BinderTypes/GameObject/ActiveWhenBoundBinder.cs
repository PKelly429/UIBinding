using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Active when bound", "Game Object")]
    public class ActiveWhenBoundBinder : GeneralBinder
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float disableDelay;
        protected override bool TargetIsNull => !target;
        
        private Tween _currentTween;

        protected override void OnBind()
        {
            _currentTween.Stop();
            target.SetActive(true);
        }

        protected override void OnUnbind()
        {
            _currentTween.Stop();
            if (disableDelay > 0)
            {
                _currentTween = Tween.Delay(target, disableDelay).OnComplete(() => target.SetActive(false));
            }
            else
            {
                target.SetActive(false);
            }
        }
        
        protected override void OnBindingValueChanged()
        {
        }
    }
}
