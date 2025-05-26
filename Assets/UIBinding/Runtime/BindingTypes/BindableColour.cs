using System;
using UnityEngine;

namespace DataBinding
{
    [Serializable]
    public class BindableColour : BindableVariable<Color>
    {
        public BindableColour(Color startingValue) : base(startingValue)
        {
        }
    }
}