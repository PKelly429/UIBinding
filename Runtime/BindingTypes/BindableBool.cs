using System;

namespace DataBinding
{
    [Serializable]
    public class BindableBool : BindableVariable<bool>
    {
        public BindableBool(bool startingValue) : base(startingValue)
        {
        }
    }
}