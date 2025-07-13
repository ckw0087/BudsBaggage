using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerLuggageCollector : MonoBehaviour
{
    [SerializeField] private Transform _luggageBase;
    [SerializeField] private float _luggageCollectionRange;
    [SerializeField] private float _luggageOffset = 0.2f;
    [SerializeField] private LayerMask _luggageLayer;

    public List<Luggage> CarriedLuggage { get; private set; } = new List<Luggage>();
    public List<Luggage> DepositingLuggage { get; private set; } = new List<Luggage>();

    public event Action OnLuggageAmountChanged;

    private void Update()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, _luggageCollectionRange, _luggageLayer);
        foreach (var collision in collisions)
        {
            if (collision.TryGetComponent<Luggage>(out Luggage luggage))
            {
                if (luggage.Collected)
                    continue;

                CarriedLuggage.Add(luggage);
                OnLuggageAmountChanged?.Invoke();

                luggage.Collect();
                luggage.transform.SetParent(_luggageBase);
                //luggage.transform.localPosition = Vector3.up * (_amountOfLuggages - 1) * _luggageOffset;
                luggage.transform.DOLocalMove(Vector3.up * (CarriedLuggage.Count - 1) * _luggageOffset, 0.2f);
                luggage.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<LuggageDeposit>(out LuggageDeposit deposit))
        {
            //Deposit luggages
            foreach (Luggage luggage in CarriedLuggage)
            {
                DepositingLuggage.Add(luggage);
                luggage.transform.SetParent(null);
            }
            CarriedLuggage.Clear();

            for (int i = DepositingLuggage.Count - 1; i >= 0; i--)
            {
                Luggage luggage = DepositingLuggage[i];
                luggage.SetOutline(true);
                luggage.transform.SetParent(null);
                luggage.transform.localScale = Vector3.one;
                luggage.transform.DOMove(transform.position + Vector3.up * 1f + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.2f);
                luggage.transform.DOMove(collision.gameObject.transform.position, 0.25f).SetDelay(0.2f + 0.1f * (DepositingLuggage.Count - i)).OnComplete(() =>
                {
                    deposit.Deposit(luggage);
                    DepositingLuggage.Remove(luggage);
                    OnLuggageAmountChanged?.Invoke();
                });
                luggage.transform.rotation = Quaternion.identity;
            }

            //CarriedLuggage.Clear();
            OnLuggageAmountChanged?.Invoke();

            /*foreach (var luggage in _carriedLuggage)
            {
                luggage.transform.SetParent(null);
                luggage.transform.DOMove(collision.gameObject.transform.position, 0.25f).OnComplete(() => Destroy(luggage.gameObject));
                luggage.transform.rotation = Quaternion.identity;
            }
            _carriedLuggage.Clear();*/
        }
    }

    public void DropLuggages()
    {
        foreach (var luggage in CarriedLuggage)
        {
            float dropTime = Random.Range(0.5f, 1f);
            luggage.transform.DOMoveX(transform.position.x + Random.Range(-2f, 2f), dropTime).SetEase(Ease.Linear);
            luggage.transform.DOMoveY(transform.position.y + Random.Range(-2f, 0f), dropTime).SetEase(Ease.InOutBounce);
            luggage.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-90f, 90f));
            luggage.transform.SetParent(null);
        }
        CarriedLuggage.Clear();
        OnLuggageAmountChanged?.Invoke();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
