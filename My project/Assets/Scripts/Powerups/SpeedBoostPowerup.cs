using UnityEngine;

public class SpeedBoostPowerup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float volume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<NewPlayerMovement>(out var movement))
        {
            movement.ActivateSpeedBoost();

            PowerupTimerManager.Instance?.ShowPowerupTimer(PowerupTimerManager.PowerupType.SpeedBoost, movement.speedBoostDuration);

            Player player = movement.GetComponent<Player>();
            if (player != null)
                player.PlayPickupSound(pickupSound, volume);

            Destroy(gameObject);
        }
    }
}
