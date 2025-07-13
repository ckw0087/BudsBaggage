using UnityEngine;

public class InstantDepositPowerup : Collectible
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float volume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLuggageCollector>(out var collector))
        {
            collector.ActivateInstantDeposit();

            PowerupTimerManager.Instance?.ShowPowerupTimer(PowerupTimerManager.PowerupType.InstantDeposit, collector._instantDepositDuration);

            Player player = collector.GetComponent<Player>();
            if (player != null)
                player.PlayPickupSound(pickupSound, volume);

            CollectEvent();
            Destroy(gameObject);
        }
    }
}
