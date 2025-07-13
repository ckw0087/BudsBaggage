using System;
using UnityEngine;

public class Luggage : Collectible
{
    [SerializeField] private Sprite _luggageSprite;

    public bool Collected { get; private set; } = false;

    private Collider2D _luggageCollider;
    public SpriteRenderer SpriteRenderer { get; private set; }
    private Material _material;

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
        CollectEvent();

    }

    public void SetOutline(bool outlined)
    {
        _material.SetInt("_DisplayOutline", outlined ? 1 : 0);
    }
}
