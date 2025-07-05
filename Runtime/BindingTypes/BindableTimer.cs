using System;
using UnityEngine;

namespace DataBinding
{
    public class BindableTimer : BindableFloat
    {
        public BindableTimer(float startingValue) : base(startingValue)
        {
        }
        
        public override string stringValue
        {
            get
            {
                TimeSpan time = TimeSpan.FromSeconds(GetValue());
                if (time.Hours > 0)
                {
                    return time.ToString("hh':'mm':'ss");
                }
                else if  (time.Minutes > 0)
                {
                    return time.ToString("'mm':'ss");
                }
                return time.ToString("'ss");
            }
        }
    }
}
