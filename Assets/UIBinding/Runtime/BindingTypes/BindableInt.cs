using System;

namespace DataBinding
{
    [Serializable]
    public class BindableInt : BindableVariable<int>
    {
        public BindableInt(int startingValue) : base(startingValue)
        {
        }
    }
}