using System;
using UnityEngine;

namespace DataBinding
{
    [Serializable]
    public class BindableSprite : BindableVariable<Sprite>
    {
        public BindableSprite(Sprite startingValue) : base(startingValue)
        {
        }
    }
}