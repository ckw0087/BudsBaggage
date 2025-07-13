using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _luggageCountText;
    [SerializeField] private Image _feverBar;
    [SerializeField] private Image _feverBarBorder;
    [SerializeField] private Color _feverDefaultColor;
    [SerializeField] private float _feverScale = 1.1f;
    [SerializeField] private PlayerLuggageCollector _luggageCollector;

    private float _feverHue;
    private bool _inFever;

    private void Update()
    {
        if (!_inFever)
            return;

        _feverHue += Time.deltaTime;
        _feverHue %= 1f;

        Color finalCol = Color.HSVToRGB(_feverHue, 0.8f, 0.8f);
        _feverBar.color = finalCol;
    }

    private void OnEnable()
    {
        _luggageCollector.OnLuggageAmountChanged += UpdateLuggageCount;
        _luggageCollector.OnFeverChanged += UpdateFeverMeter;
        _luggageCollector.OnFeverStarted += StartFever;
        _luggageCollector.OnFeverEnd += StopFever;
    }

    public void UpdateLuggageCount()
    {
        _luggageCountText.rectTransform.DOKill(true);
        float bonusScale = (float)(_luggageCollector.CarriedLuggage.Count) / 100f;
        bonusScale = Mathf.Clamp(bonusScale, 0, 0.5f);
        _luggageCountText.rectTransform.localScale = Vector3.one + Vector3.one * bonusScale;
        _luggageCountText.rectTransform.DOPunchScale(-Vector3.one * 0.15f, 0.15f);
        _luggageCountText.text = _luggageCollector.CarriedLuggage.Count.ToString();
    }

    public void UpdateFeverMeter()
    {
        _feverBar.fillAmount = _luggageCollector.Fever / _luggageCollector.MaxFever;
    }

    public void StartFever()
    {
        _feverBarBorder.rectTransform.DOScale(Vector3.one * _feverScale, 0.2f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
        _inFever = true;
    }

    public void StopFever()
    {
        _feverBarBorder.rectTransform.DOKill(true);
        _feverBarBorder.rectTransform.localScale = Vector3.one;
        _inFever = false;
    }
}
