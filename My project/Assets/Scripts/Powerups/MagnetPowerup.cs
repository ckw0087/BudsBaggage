using UnityEngine;

public class MagnetPowerup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float volume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMagnet>(out var magnet))
        {
            magnet.ActivateMagnet();

            Player player = magnet.GetComponent<Player>();
            if (player != null)
                player.PlayPickupSound(pickupSound, volume);

            Destroy(gameObject);
        }
    }
}
