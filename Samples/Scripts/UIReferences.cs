using System;
using UnityEngine;

namespace DataBinding.Sample
{
    public class UIReferences : MonoBehaviour
    {
        public UIBinding playerBinding;
        
        public static UIReferences Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}
