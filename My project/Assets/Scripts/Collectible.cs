using System;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public event Action<Collectible> OnCollected;

    protected void CollectEvent()
    {
        OnCollected?.Invoke(this);
    }
}
