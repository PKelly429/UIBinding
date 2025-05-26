using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Scale when bound", "Animation")]
    public class ScaleWhenBoundBinder : GeneralBinder
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private Vector3 unboundScale = Vector3.zero;
        [SerializeField] private Vector3 boundScale = Vector3.one;
        
        [SerializeField] private float scaleTime = 0.1f;
        
        protected override bool TargetIsNull => !target;
        
        protected override void OnBind()
        {
            Tween.Scale(target, boundScale, scaleTime);
        }

        protected override void OnUnbind()
        {
            Tween.Scale(target, unboundScale, scaleTime);
        }

        protected override void OnBindingValueChanged()
        {
        }
    }
}
