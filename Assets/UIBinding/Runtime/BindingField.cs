using System;
using UnityEngine;

namespace DataBinding
{
    [Serializable]
    public class BindingField
    {
        public string property;

        public AbstractBindableVariable GetBindingVariable(object obj, Type bindingType)
        {
            if (!bindingType.IsInstanceOfType(obj))
            {
#if DEBUG
                Debug.LogError($"Trying to bind {obj.GetType()} to BindingField of type {bindingType}");
#endif
                return null;
            }

            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("BindingField does not have property");
            }

            try
            {
                return (AbstractBindableVariable)bindingType.GetField(property).GetValue(obj);
            }
            catch (Exception e)
            {
                throw new ArgumentException("BindingField property does not exist.");
            }
        }
        
#if UNITY_EDITOR
        public void PrintMessageIfInvalid(GameObject target , Type bindingType)
        {
            if (bindingType == null)
            {
                Debug.LogError($"BindingField '{target.name}' does not have a binding type", target);
            }
            if (string.IsNullOrEmpty(property))
            {
                Debug.LogError($"BindingField '{target.name}' does not have property", target);
            }
            else
            {
                try
                {
                    var fieldInfo = bindingType.GetField(property);
                    if (fieldInfo == null)
                    {
                        Debug.LogError($"BindingField '{target.name}' has missing property {property}", target);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"BindingField '{target.name}' has missing property {property}", target);
                }   
            }
        }
#endif
    }
}