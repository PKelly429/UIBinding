using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Offset when bound", "Position")]
    public class OffsetWhenBoundBinder : GeneralBinder
    {
        [SerializeField] private RectTransform target;
        
        [SerializeField] private Vector3 boundOffset = Vector3.one;
        
        [SerializeField] private float moveTime = 0.3f;
        [SerializeField] private Ease _ease = Ease.Default;

        private bool _initalPositionSet;
        private Vector3 _initalPosition;
        
        protected override bool TargetIsNull => !target;
        
        protected override void OnBind()
        {
            if (!_initalPositionSet)
            {
                _initalPosition = target.localPosition;
                _initalPositionSet = true;
            }
            Tween.LocalPosition(target, _initalPosition+boundOffset, moveTime, _ease);
        }

        protected override void OnUnbind()
        {
            Tween.LocalPosition(target, _initalPosition-boundOffset, moveTime, _ease);
        }

        protected override void OnBindingValueChanged()
        {
        }
    }
}
