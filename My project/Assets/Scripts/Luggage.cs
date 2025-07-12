using System;
using UnityEngine;

public class Luggage : MonoBehaviour
{
    [SerializeField] private Sprite _luggageSprite;

    public bool Collected { get; private set; } = false;

    private Collider2D _luggageCollider;
    private SpriteRenderer _spriteRenderer;
    private Material _material;

    public event Action<Luggage> OnCollected;

    private void Awake()
    {
        _luggageCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _material = _spriteRenderer.material;

        if (_luggageSprite != null)
        {
            _spriteRenderer.sprite = _luggageSprite;
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
