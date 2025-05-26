using System;
using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Serializable]
    [Binder("Complex Text: Unity", "Text")]
    public class ComplexTextBinder : AbstractComplexTextBinder
    {
        [SerializeField] private Text target;

        protected override void OnValueChanged()
        {
            target.text = GetBoundText();
        }
        
#if UNITY_EDITOR
        public override void Reset(Component parent)
        {
            target = parent.GetComponent<Text>();
        }
#endif
    }
}
