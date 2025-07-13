using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerLuggageCollector : MonoBehaviour
{
    [Header("Fever")]
    [field: SerializeField] public float MaxFever { get; private set; } = 100f;
    [SerializeField] private float _feverGainPerLuggage = 10f;
    [SerializeField] private float _feverDepletionRate = 10f;

    [Header("Luggage Collection")]
    [SerializeField] private float _instantDepositDuration = 5f;
    [SerializeField] private Transform _luggageBase;
    [SerializeField] private float _luggageCollectionRange;
    [SerializeField] private float _luggageOffset = 0.2f;
    [SerializeField] private LayerMask _luggageLayer;

    public float Fever = 0f;
    public bool InFever { get; private set; }

    public List<Luggage> CarriedLuggage { get; private set; } = new List<Luggage>();
    public List<Luggage> DepositingLuggage { get; private set; } = new List<Luggage>();

    public event Action OnLuggageAmountChanged;
    public event Action OnFeverChanged;
    public event Action OnFeverStarted;
    public event Action OnFeverEnd;

    private bool _isInstantDeposit;
    private Coroutine _instantDepositRoutine;

    private void Update()
    {
        if (InFever)
        {
            Fever -= _feverDepletionRate * Time.deltaTime;
            OnFeverChanged?.Invoke();
            if (Fever <= 0f)
            {
                InFever = false;
                OnFeverEnd?.Invoke();
                Fever = 0f;
            }
        }

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
                if (!_isInstantDeposit)
                {
                    luggage.transform.SetParent(_luggageBase);
                    //luggage.transform.localPosition = Vector3.up * (_amountOfLuggages - 1) * _luggageOffset;
                    luggage.transform.DOLocalMove(Vector3.up * (CarriedLuggage.Count - 1) * _luggageOffset, 0.2f);
                    luggage.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    DepositLuggages(collision);
                }


                Fever += _feverGainPerLuggage;
                Fever = Mathf.Clamp(Fever, 0, MaxFever);
                OnFeverChanged?.Invoke();
                if (Fever >= MaxFever && !InFever)
                {
                    InFever = true;
                    OnFeverStarted?.Invoke();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<LuggageDeposit>(out LuggageDeposit deposit))
        {
            DepositLuggages(collision, deposit);
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
            luggage.SpriteRenderer.DOFade(0f, 1.5f).SetDelay(1f).OnComplete(() => Destroy(luggage.gameObject));
        }
        CarriedLuggage.Clear();
        OnLuggageAmountChanged?.Invoke();
    }

    public void ActivateInstantDeposit()
    {
        if (_instantDepositRoutine != null)
            StopCoroutine(_instantDepositRoutine);

        _instantDepositRoutine = StartCoroutine(InstantDeposit(_instantDepositDuration));

        if (GameObject.FindGameObjectWithTag("Deposit").TryGetComponent<LuggageDeposit>(out LuggageDeposit deposit))
        {
            DepositLuggages(deposit);
        }
    }

    private IEnumerator InstantDeposit(float duration)
    {
        _isInstantDeposit = true;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _isInstantDeposit = false;
    }

    private void DepositLuggages(Collider2D collision, LuggageDeposit deposit)
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
            luggage.transform.SetParent(null, true);
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

        OnLuggageAmountChanged?.Invoke();
    }

    private void DepositLuggages(Collider2D collision)
    {
        GameObject.FindGameObjectWithTag("Deposit").TryGetComponent<LuggageDeposit>(out LuggageDeposit deposit);
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
            luggage.transform.DOMove(collision.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.2f);
            luggage.transform.DOMove(deposit.transform.position, 0.25f).SetDelay(0.2f + 0.1f * (DepositingLuggage.Count - i)).OnComplete(() =>
            {
                deposit.Deposit(luggage);
                DepositingLuggage.Remove(luggage);
                OnLuggageAmountChanged?.Invoke();
            });
            luggage.transform.rotation = Quaternion.identity;
        }

        OnLuggageAmountChanged?.Invoke();
    }

    private void DepositLuggages(LuggageDeposit deposit)
    {
        //Deposit luggages
        foreach (Luggage luggage in CarriedLuggage)
        {
            DepositingLuggage.Add(luggage);
            luggage.transform.SetParent(null, true);
        }
        CarriedLuggage.Clear();

        for (int i = DepositingLuggage.Count - 1; i >= 0; i--)
        {
            Luggage luggage = DepositingLuggage[i];
            luggage.SetOutline(true);
            luggage.transform.SetParent(null);
            luggage.transform.localScale = Vector3.one;
            luggage.transform.DOMove(transform.position + Vector3.up * 1f + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.2f);
            luggage.transform.DOMove(deposit.transform.position, 0.25f).SetDelay(0.2f + 0.1f * (DepositingLuggage.Count - i)).OnComplete(() =>
            {
                deposit.Deposit(luggage);
                DepositingLuggage.Remove(luggage);
                OnLuggageAmountChanged?.Invoke();
            });
            luggage.transform.rotation = Quaternion.identity;
        }

        OnLuggageAmountChanged?.Invoke();
    }
}
