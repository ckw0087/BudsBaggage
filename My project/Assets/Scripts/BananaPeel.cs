using UnityEngine;

public class BananaPeel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerLuggageCollector>().DropLuggages();
            Destroy(gameObject);
        }
    }
}
