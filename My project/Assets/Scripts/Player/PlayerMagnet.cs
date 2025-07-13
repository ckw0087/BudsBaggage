using System.Collections;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] private float radius = 3f;
    [SerializeField, Range(5f, 20f)] private float force = 5f;
    [Range(1f, 20f)] public float duration = 5f;
    [SerializeField] private LayerMask luggageLayer;

    public void ActivateMagnet()
    {
        StartCoroutine(MagnetRoutine(duration));
    }

    private IEnumerator MagnetRoutine(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            AttractLuggages();
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void AttractLuggages()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, luggageLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Luggage>(out var luggage) && !luggage.Collected)
            {
                luggage.transform.position = Vector3.Lerp(
                    luggage.transform.position,
                    transform.position,
                    force * Time.deltaTime
                );
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
