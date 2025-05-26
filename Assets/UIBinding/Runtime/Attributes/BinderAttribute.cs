using System;
using UnityEngine;

namespace DataBinding
{
    public class BinderAttribute : Attribute
    {
        public string name;
        public string category;

        public BinderAttribute(string name, string category)
        {
            this.name = name;
            this.category = category;
        }
        
        public string Path => string.IsNullOrEmpty(category) ? $"Misc/{name}" : $"{category}/{name}";
    }
}
