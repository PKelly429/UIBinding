using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataBinding
{
    public class BinderUpdater : MonoBehaviour
    {
        private static List<IUpdateBinder> _binders;
        public static void Register(IUpdateBinder binder)
        {
            _binders.Add(binder);
        }
        
        public static void Deregister(IUpdateBinder binder)
        {
            _binders.Remove(binder);
        }

        public void Update()
        {
            foreach (var binder in _binders)
            {
                binder.UpdateBinder();
            }
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            GameObject go = new GameObject("BinderUpdater");
            go.AddComponent<BinderUpdater>();
            DontDestroyOnLoad(go);
            _binders = new List<IUpdateBinder>();
#if UNITY_EDITOR
            go.hideFlags = HideFlags.HideAndDontSave;
#endif
        }
    }
}
