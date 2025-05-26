using System;
using TMPro;
using UnityEngine;

namespace DataBinding
{
    [Serializable]
    [Binder("Complex Text: TMP", "Text")]
    public class TMPComplexTextBinder : AbstractComplexTextBinder
    {
        [SerializeField] private TMP_Text target;

        protected override void OnValueChanged()
        {
            target.text = GetBoundText();
        }
        
#if UNITY_EDITOR
        public override void Reset(Component parent)
        {
            target = parent.GetComponent<TMP_Text>();
        }
#endif
    }
}
