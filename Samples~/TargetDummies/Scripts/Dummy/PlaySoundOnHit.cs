using UnityEngine;

public class PlaySoundOnHit : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        health.OnHit += OnHit;
    }
    
    private void OnDisable()
    {
        health.OnHit -= OnHit;
    }

    private void OnHit()
    {
        audioSource.Play();
    }
}
