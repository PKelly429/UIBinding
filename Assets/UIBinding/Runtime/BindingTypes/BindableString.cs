using System;

namespace DataBinding
{
    [Serializable]
    public class BindableString : BindableVariable<string>
    {
        public BindableString(string startingValue) : base(startingValue)
        {
        }
        
        public override string stringValue
        {
            get => GetValue();
            set => SetValue(value);
        }
    }
}