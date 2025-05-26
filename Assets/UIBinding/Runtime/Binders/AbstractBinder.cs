using System;
using UnityEngine;

namespace DataBinding
{
    [Serializable]
    public abstract class AbstractBinder
    {
        public abstract void Bind(object obj, Type bindingType);
        public abstract void Unbind();
        
        public virtual void Reset(Component component) { }
        public virtual void OnValidate() { }
    }

    public abstract class GeneralBinder : AbstractBinder
    {
        [SerializeField] private BindingField bindingField;
        
        protected AbstractBindableVariable bindableVariable;
        protected abstract bool TargetIsNull { get; }

        public override void Bind(object obj, Type bindingType)
        {
            if (TargetIsNull)
            {
#if DEBUG
                Debug.LogError($"Binder: {GetType().Name} does not have a target");
#endif
                return;
            }
            
            try
            {
                bindableVariable = bindingField.GetBindingVariable(obj, bindingType);
            }
            catch (Exception e)
            {
                Debug.LogError($"Encountered error while binding {obj.GetType()} to {bindingField.property}: {e.Message}");
            }
            
            bindableVariable.onValueChanged += OnBindingValueChanged;
            OnBind();
        }

        public override void Unbind()
        {
            if (bindableVariable == null) return;
            bindableVariable.onValueChanged -= OnBindingValueChanged;
            bindableVariable = null;
            
            if (TargetIsNull) return;
            OnUnbind();
        }
        
        protected virtual void OnBind() { }
        protected virtual void OnUnbind() { }

        protected abstract void OnBindingValueChanged();
    }

    public abstract class AbstractBinder<T> : AbstractBinder
    {
        protected BindableVariable<T> bindableVariable;

        protected abstract BindingField BindingField { get; }
        protected abstract bool TargetIsNull { get; }
        
        public sealed override void Bind(object obj, Type bindingType)
        {
            Unbind();
            if (TargetIsNull)
            {
                #if DEBUG
                Debug.LogError($"Binder: {GetType().Name} does not have a target");
                #endif
                
                return;
            }
            if (obj == null) return;

            try
            {
                bindableVariable = BindingField.GetBindingVariable(obj, bindingType) as BindableVariable<T>;
            }
            catch (Exception e)
            {
                Debug.LogError($"Encountered error while binding {obj.GetType()} to {BindingField.property}: {e.Message}");
            }

            if (bindableVariable == null) return;
        
            bindableVariable.onValueChanged += OnBindingValueChanged;
            OnBind();
            OnBindingValueChanged();
        }

        public sealed override void Unbind()
        {
            if (bindableVariable == null) return;
            bindableVariable.onValueChanged -= OnBindingValueChanged;
            bindableVariable = null;
            
            if (TargetIsNull) return;
            OnUnbind();
        }

        protected virtual void OnBind() { }
        protected virtual void OnUnbind() { }
        protected abstract void OnBindingValueChanged();
    }
}
