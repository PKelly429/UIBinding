using UnityEngine;

namespace DataBinding
{
    [Binder("Follow Transform", "Position")]
    public class TransformPositionBinder : AbstractBinder<Transform>, IUpdateBinder
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private RectTransform canvas;
        
        [SerializeField, BindingType(typeof(Transform))]
        private BindingField bindingField;
        
        private Camera _mainCamera;
        private bool _isRegistered;

        protected override BindingField BindingField => bindingField;
        protected override bool TargetIsNull => !target;

        protected override void OnBind()
        {
            _mainCamera = Camera.main;

            if (!canvas)
            {
#if DEBUG
                Debug.LogError("Canvas not set on Follow Transform binder");
#endif
                return;
            }
            if (!_mainCamera)
            {
#if DEBUG
                Debug.LogError("Could not find camera on Follow Transform binder");
#endif
                return;
            }
            
            if(_isRegistered) return;
            BinderUpdater.Register(this);
            _isRegistered = true;
        }

        protected override void OnUnbind()
        {
            if(!_isRegistered) return;
            BinderUpdater.Deregister(this);
            _isRegistered = false;
        }

        protected override void OnBindingValueChanged()
        {
            UpdatePosition();
        }

        public void UpdateBinder()
        {
            UpdatePosition();
        }
        
        private void UpdatePosition()
        {
            Vector2 screenPoint = _mainCamera.WorldToScreenPoint(bindableVariable.GetValue().position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, null, out var pos);
            target.anchoredPosition = pos;
        }
    }
}
