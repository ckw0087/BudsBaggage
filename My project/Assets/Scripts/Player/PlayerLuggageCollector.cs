using DG.Tweening;
using UnityEngine;

public class PlayerLuggageCollector : MonoBehaviour
{
    [SerializeField] private Transform _luggageBase;
    [SerializeField] private float _luggageCollectionRange;
    [SerializeField] private float _luggageOffset = 0.2f;
    [SerializeField] private LayerMask _luggageLayer;

    private int _amountOfLuggages = 0;

    private void Update()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, _luggageCollectionRange, _luggageLayer);
        foreach (var collision in collisions)
        {
            if (collision.TryGetComponent<Luggage>(out Luggage luggage))
            {
                if (luggage.Collected)
                    continue;

                _amountOfLuggages++;
                luggage.Collect();
                luggage.transform.SetParent(_luggageBase);
                //luggage.transform.localPosition = Vector3.up * (_amountOfLuggages - 1) * _luggageOffset;
                luggage.transform.DOLocalMove(Vector3.up * (_amountOfLuggages - 1) * _luggageOffset, 0.2f);
                luggage.transform.localRotation = Quaternion.identity;
            }
        }
    }

  
}
