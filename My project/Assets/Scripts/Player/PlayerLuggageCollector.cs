using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private TMP_Text _comboRatingText;
    [SerializeField] private float _comboResetTime = 2f;

    public float Fever = 0f;
    public bool InFever { get; private set; }

    private int _combo = 0;
    private float _comboTimer = 0f;
    private bool _inCombo = false;

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
        if (_inCombo)
        {
            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0f)
            {
                _inCombo = false;
                _comboRatingText.DOKill(true);
                _comboRatingText.alpha = 1f;
                string rating = "OKAY!";
                if (_combo > 100)
                    rating = "LEGENDARY!";
                else if (_combo > 50)
                    rating = "SPECTACULAR!";
                else if (_combo > 25)
                    rating = "BRAVO!";
                else if (_combo > 10)
                    rating = "GREAT!";

                _comboRatingText.text = rating;
                _comboRatingText.rectTransform.DOKill();
                _comboRatingText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45f));
                _comboRatingText.rectTransform.localScale = Vector3.one + Vector3.one * Mathf.Clamp(0.05f * _combo, 0f, 2f);
                _comboRatingText.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
                _comboRatingText.rectTransform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutBounce);

                _comboRatingText.DOFade(0f, 0.5f).SetDelay(0.5f);
                _comboText.DOFade(0f, 0.5f).SetDelay(0.5f);
                _combo = 0;
            }
        }

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
                _inCombo = true;
                _combo++;
                _comboText.text = $"X{_combo}";
                _comboText.rectTransform.DOKill();
                _comboText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45f));
                _comboText.rectTransform.localScale = Vector3.one + Vector3.one * Mathf.Clamp(0.05f * _combo, 0f, 2f);
                _comboText.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
                _comboText.rectTransform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutBounce);
                _comboText.DOKill(true);
                _comboText.alpha = 1f;

                _comboTimer = _comboResetTime;
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
