using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

namespace DataBinding
{
    public class UIBinderPool : MonoBehaviour
    {
        [SerializeField] private UIBinding Prefab;
        [SerializeField] private RectTransform parent;
        [SerializeField] private float releaseDelay;
        
        private ObjectPool<UIBinding> _pool;
        private Dictionary<object, UIBinding> _activeBindings = new Dictionary<object, UIBinding>();

        private void Awake()
        {
            _pool = new ObjectPool<UIBinding>(CreateFunc, ActionOnGet, ActionOnRelease);
        }

        public void Bind(object obj)
        {
            _pool.Get(out var binder);
            binder.Bind(obj);
            _activeBindings.Add(obj, binder);
        }

        public void Unbind(object obj)
        {
            if (_activeBindings.ContainsKey(obj))
            {
                var binder = _activeBindings[obj];
                binder.Unbind();
                Tween.Delay(releaseDelay).OnComplete(() => _pool.Release(binder));
                _activeBindings.Remove(obj);
            }
        }

        private UIBinding CreateFunc()
        {
            return Instantiate(Prefab, parent);
        }
        
        private void ActionOnGet(UIBinding obj)
        {
            obj.gameObject.SetActive(true);
        }
        
        private void ActionOnRelease(UIBinding obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
