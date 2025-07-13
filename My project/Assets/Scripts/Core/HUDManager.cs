using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _luggageCountText;
    [SerializeField] private Image _feverBar;
    [SerializeField] private Image _feverBarBorder;
    [SerializeField] private Image _feverImage;
    [SerializeField] private Color _feverDefaultColor1;
    [SerializeField] private Color _feverDefaultColor2;
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
        if (_luggageCollector.CarriedLuggage.Count > 60)
            _luggageCountText.text += $"(x5)";
        else if (_luggageCollector.CarriedLuggage.Count > 45)
            _luggageCountText.text += $"(x4)";
        else if (_luggageCollector.CarriedLuggage.Count > 30)
            _luggageCountText.text += $"(x3)";
        else if (_luggageCollector.CarriedLuggage.Count > 15)
            _luggageCountText.text += $"(x2)";
    }

    public void UpdateFeverMeter()
    {
        _feverBar.fillAmount = _luggageCollector.Fever / _luggageCollector.MaxFever;
        if (!_inFever)
        {
            var fill = _feverBar.fillAmount;
            var adjustedFill = Mathf.Lerp(0.3f, 0.8f, fill);
            _feverBar.color = Color.HSVToRGB(Mathf.Lerp(0.5f, 1.2f, fill * 0.8f), adjustedFill, adjustedFill); 
        }
    }

    public void StartFever()
    {
        _feverBarBorder.rectTransform.DOScale(Vector3.one * _feverScale, 0.2f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
        _inFever = true;
        _feverImage.color = Color.white;
        _feverImage.rectTransform.anchoredPosition = new Vector2(-1050f, 0f);
        _feverImage.rectTransform.DOAnchorPosX(0f, 0.3f).SetEase(Ease.OutSine);
        _feverImage.rectTransform.DOScale(1.1f, 0.3f);
        _feverImage.rectTransform.DOAnchorPosX(1050f, 0.5f).SetEase(Ease.OutSine).SetDelay(1f);
        _feverImage.rectTransform.DOScale(0.9f, 0.5f).SetDelay(1f);
        _feverImage.DOFade(0f, 0.5f).SetDelay(1f);
    }

    public void StopFever()
    {
        _feverBarBorder.rectTransform.DOKill(true);
        _feverBarBorder.rectTransform.localScale = Vector3.one;
        _feverBar.color = _feverDefaultColor1;
        _inFever = false;
    }
}
