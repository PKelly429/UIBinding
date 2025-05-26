using System;
using UnityEngine;

namespace DataBinding
{
    public class BindingTypeAttribute : PropertyAttribute
    {
        public Type bindingType;
        public BindingTypeAttribute(Type type)
        {
            bindingType = type;
        }
    }
}
