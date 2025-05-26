using System;
using UnityEngine;

public class TriggerDeathAnimation : MonoBehaviour
{
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Hit = Animator.StringToHash("Hit");
    
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        health.OnHit -= OnHit;
        health.OnDeath -= OnDeath;
    }

    private void OnHit()
    {
        animator.SetTrigger(Hit);
    }

    private void OnDeath()
    {
        animator.SetBool(Dead, true);
    }
}
