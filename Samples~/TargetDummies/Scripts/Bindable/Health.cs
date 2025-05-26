using System;
using System.Collections.Generic;
using DataBinding;
using DataBinding.Sample;
using UnityEngine;

[Bindable]
public class Health : MonoBehaviour
{
    public BindableFloat currentHealth;
    public BindableFloat maxHealth;
    public BindableBool isAlive;
    public BindableTransform pivot;
    
    public Action OnHit;
    public Action OnDeath;

    public void Damage(int damage)
    {
        if(!isAlive) return;
        
        currentHealth.SetValue(Mathf.Max(currentHealth - damage, 0));
        OnHit?.Invoke();
        if (currentHealth <= 0)
        {
            isAlive.SetValue(false);
            OnDeath?.Invoke();
        }
    }

    private void OnEnable()
    {
        currentHealth.SetValue(maxHealth);
        isAlive.SetValue(true);
        UIReferences.Instance.healthBarPool.Bind(this);
    }

    private void OnDisable()
    {
        UIReferences.Instance.healthBarPool.Unbind(this);
    }
}
