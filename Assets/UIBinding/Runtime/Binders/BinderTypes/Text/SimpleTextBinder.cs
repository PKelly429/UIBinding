using UnityEngine;
using UnityEngine.UI;

namespace DataBinding
{
    [Binder("Simple Text: Unity", "Text")]
    public class SimpleTextBinder : AbstractSimpleTextBinder
    {
        [SerializeField] private Text target;

        protected override void OnValueChanged()
        {
            target.text = GetBoundText();
        }
    }
}
