using UnityEngine;

public class Luggage : MonoBehaviour
{
    public bool Collected { get; private set; } = false;

    private Collider2D _luggageCollider;

    private void Awake()
    {
        _luggageCollider = GetComponent<Collider2D>();
    }

    public void Collect()
    {
        Collected = true;
        _luggageCollider.enabled = false;
    }
}
