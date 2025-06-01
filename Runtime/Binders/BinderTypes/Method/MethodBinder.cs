using System;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Method Callback", "Button")]
    public class MethodBinder : AbstractBinder
    {
        [SerializeField, BindableMethod]
        private BindingField bindingField;

        [SerializeField] private Button button;
        
        private Delegate callback;
        
        public override void Bind(object obj, Type bindingType)
        {
            if (!button)
            {
                #if DEBUG
                Debug.LogError("MethodBinder: Button is not set");
                #endif
                return;
            }
            
            callback = bindingField.GetBindingMethod(obj, bindingType);
            button.onClick.AddListener(Callback);
        }

        public override void Unbind()
        {
            if(!button) return;
            
            button.onClick.RemoveListener(Callback);
        }

        private void Callback()
        {
            callback?.DynamicInvoke(null);
        }
    }
}
