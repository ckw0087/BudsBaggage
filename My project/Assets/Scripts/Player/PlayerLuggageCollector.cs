using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerLuggageCollector : MonoBehaviour
{
    [SerializeField] private Transform _luggageBase;
    [SerializeField] private float _luggageCollectionRange;
    [SerializeField] private float _luggageOffset = 0.2f;
    [SerializeField] private float _instantDepositDuration = 5f;
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
        Debug.LogWarning("deposit");
        if (collision.gameObject.TryGetComponent<LuggageDeposit>(out LuggageDeposit deposit))
        {
            foreach (var luggage in _carriedLuggage)
            {
                luggage.transform.SetParent(null);
                luggage.transform.DOMove(collision.gameObject.transform.position, 0.25f).OnComplete(() => Destroy(luggage.gameObject));
                luggage.transform.rotation = Quaternion.identity;
            }
            _carriedLuggage.Clear();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void ActivateInstantDeposit()
    {
        StartCoroutine(InstantDeposit(_instantDepositDuration));
    }

    private IEnumerator InstantDeposit(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            DepositLuggages();
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void DepositLuggages()
    {
        if (GameObject.FindGameObjectWithTag("Deposit").TryGetComponent<LuggageDeposit>(out LuggageDeposit deposit))
        {
            foreach (var luggage in _carriedLuggage)
            {
                luggage.transform.SetParent(null);
                luggage.transform.DOMove(deposit.transform.position, 0.25f).OnComplete(() => Destroy(luggage.gameObject));
                luggage.transform.rotation = Quaternion.identity;
            }
            _carriedLuggage.Clear();
        }
    }
}
