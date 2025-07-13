using UnityEngine;

public class BananaPeel : MonoBehaviour
{
    [SerializeField] AudioSource bananaSoundSource;
    [SerializeField] private AudioClip _bananaSfx;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bananaSoundSource.PlayOneShot(_bananaSfx);
            collision.gameObject.GetComponent<PlayerLuggageCollector>().DropLuggages();
            Destroy(gameObject);
        }
    }
}
