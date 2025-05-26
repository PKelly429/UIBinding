using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Show For Time", "Alpha")]
    public class ShowForTimeBinder : GeneralBinder
    {
        [SerializeField] private CanvasGroup target;
        [SerializeField] private float duration;
        [SerializeField] private float fadeTime;

        private Sequence _currentTween;
        
        protected override bool TargetIsNull => !target;

        protected override void OnBind()
        {
            target.alpha = 0;
        }
        
        protected override void OnBindingValueChanged()
        {
            _currentTween.Stop();
            _currentTween = Tween.Alpha(target, 1, fadeTime).Chain(Tween.Alpha(target, 0, fadeTime, startDelay:duration));
        }
    }
}
