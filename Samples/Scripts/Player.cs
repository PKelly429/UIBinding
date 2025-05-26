using System;
using UnityEngine;

namespace DataBinding.Sample
{
    [Bindable]
    public class Player : MonoBehaviour
    {
        public BindableFloat health;
        public BindableFloat maxHealth;

        private void Start()
        {
            UIReferences.Instance.playerBinding.Bind(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                health.SetValue(Mathf.Max(health - 20, 0));
            }
        }
    }
}
