using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerLuggageCollector : MonoBehaviour
{
    [SerializeField] private Transform _luggageBase;
    [SerializeField] private float _luggageCollectionRange;
    [SerializeField] private float _luggageOffset = 0.2f;
    [SerializeField] private LayerMask _luggageLayer;

    private List<Luggage> _carriedLuggage = new List<Luggage>();

    private void Update()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, _luggageCollectionRange, _luggageLayer);
        foreach (var collision in collisions)
        {
            if (collision.TryGetComponent<Luggage>(out Luggage luggage))
            {
                if (luggage.Collected)
                    continue;

                _carriedLuggage.Add(luggage);
                luggage.Collect();
                luggage.transform.SetParent(_luggageBase);
                //luggage.transform.localPosition = Vector3.up * (_amountOfLuggages - 1) * _luggageOffset;
                luggage.transform.DOLocalMove(Vector3.up * (_carriedLuggage.Count - 1) * _luggageOffset, 0.2f);
                luggage.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<LuggageDeposit>(out LuggageDeposit deposit))
        {
            for (int i = _carriedLuggage.Count - 1; i >= 0; i--)
            {
                Luggage luggage = _carriedLuggage[i];
                luggage.SetOutline(true);
                luggage.transform.SetParent(null);
                luggage.transform.localScale = Vector3.one;
                luggage.transform.DOMove(transform.position + Vector3.up * 1f + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.2f);
                luggage.transform.DOMove(collision.gameObject.transform.position, 0.25f).SetDelay(0.2f + 0.1f * (_carriedLuggage.Count - i)).OnComplete(() => deposit.Deposit(luggage));
                luggage.transform.rotation = Quaternion.identity;
            }

            _carriedLuggage.Clear();
            /*foreach (var luggage in _carriedLuggage)
            {
                luggage.transform.SetParent(null);
                luggage.transform.DOMove(collision.gameObject.transform.position, 0.25f).OnComplete(() => Destroy(luggage.gameObject));
                luggage.transform.rotation = Quaternion.identity;
            }
            _carriedLuggage.Clear();*/
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
