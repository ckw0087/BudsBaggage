using UnityEngine;

public class InstantDepositPowerup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float volume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLuggageCollector>(out var collector))
        {
            collector.ActivateInstantDeposit();
            PlaySound();
            Destroy(gameObject);
        }
    }

    private void PlaySound()
    {
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
    }
}
