using System;
using PrimeTween;
using UnityEngine;

namespace DataBinding
{
    [Binder("Punch Scale", "Animation")]
    public class PunchScaleBinder : GeneralBinder
    {
        [SerializeField] private RectTransform target;
        
        [SerializeField] private float strength = 1f;
        [SerializeField] private Vector3 strengthVector = Vector3.one;
        [SerializeField] private float duration = 0.3f;
        
        protected override bool TargetIsNull => !target;

        protected override void OnBindingValueChanged()
        {
            Tween.PunchScale(target, strengthVector * strength, duration);
        }
    }
}
