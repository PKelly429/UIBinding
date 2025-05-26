using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DataBinding
{
    public class UIBinding : MonoBehaviour
    {
        public BindingType bindingType;
        [SerializeReference] public List<AbstractBinder> binders;

        public void Bind(object obj)
        {
            foreach (var binder in binders)
            {
                binder.Bind(obj, bindingType.Type);
            }
        }

        public void Unbind()
        {
            foreach (var binder in binders)
            {
                binder.Unbind();
            }
        }

        public void OnDisable()
        {
            Unbind();
        }

        public void OnValidate()
        {
            if(binders == null) return;
            foreach (var binder in binders)
            {
                if(binder == null) continue;
                binder.OnValidate();
            }
        }
    }
}
