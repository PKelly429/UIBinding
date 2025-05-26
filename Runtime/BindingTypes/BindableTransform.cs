using System;
using UnityEngine;

namespace DataBinding
{
    [Serializable]
    public class BindableTransform : BindableVariable<Transform>
    {
        public BindableTransform(Transform startingValue) : base(startingValue)
        {
        }
    }
}