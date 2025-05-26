using System;

namespace DataBinding
{
    [Serializable]
    public class BindableFloat : BindableVariable<float>
    {
        public BindableFloat(float startingValue) : base(startingValue)
        {
        }
    }
}