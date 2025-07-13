using System;
using UnityEngine;

public class Luggage : MonoBehaviour
{
    [SerializeField] private Sprite _luggageSprite;

    public bool Collected { get; private set; } = false;

    private Collider2D _luggageCollider;
    public SpriteRenderer SpriteRenderer { get; private set; }
    private Material _material;

    public event Action<Luggage> OnCollected;

    private void Awake()
    {
        _luggageCollider = GetComponent<Collider2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _material = SpriteRenderer.material;

        if (_luggageSprite != null)
        {
            SpriteRenderer.sprite = _luggageSprite;
        }

    }

    public void Collect()
    {
        Collected = true;
        _luggageCollider.enabled = false;
        OnCollected?.Invoke(this);
    }

    public void SetOutline(bool outlined)
    {
        _material.SetInt("_DisplayOutline", outlined ? 1 : 0);
    }
}
