using TMPro;
using UnityEngine;

namespace DataBinding
{
    [Binder("Simple Text: TMP", "Text")]
    public class TMPSimpleTextBinder : AbstractSimpleTextBinder
    {
        [SerializeField] private TMP_Text target;

        protected override void OnValueChanged()
        {
            target.text = GetBoundText();
        }
    }
}
