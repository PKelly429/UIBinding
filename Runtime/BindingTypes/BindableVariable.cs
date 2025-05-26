using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataBinding
{
    public abstract class AbstractBindableVariable
    {
        public Action onValueChanged;
        public abstract object value { get; set; }
        public abstract Type Type { get; }

        /// <summary>
        /// Override to change what is displayed when bound to a text field
        /// </summary>
        public virtual string stringValue
        {
            get => value.ToString();
            set { }
        }
        
        /// <summary>
        /// Can be overriden to provide localised text to enums in a dropdown field
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public virtual string GetLocalisedEnumText(object enumValue)
        {
            return enumValue.ToString();
        }
    }

    [Serializable]
    public abstract class BindableVariable<T> : AbstractBindableVariable
    {
        [SerializeField]
        private T _value;
        
        public BindableVariable(T startingValue)
        {
            SetValue(startingValue);
        }

        protected BindableVariable()
        {
        }

        public override object value
        {
            get => _value;
            set
            {
                T newValue = (T)value;
                if (EqualityComparer<T>.Default.Equals(_value, newValue)) return;
            
                _value = newValue;
                onValueChanged?.Invoke();
            }
        }

        public override Type Type => typeof(T);
    
        public T GetValue() => _value;
        public void SetValue(T newValue)
        {
            value = newValue;
        }

        public void SetWithoutNotify(T newValue)
        {
            _value = newValue;
        }
    
        public static implicit operator T(BindableVariable<T> var) => var.GetValue();
    }
}