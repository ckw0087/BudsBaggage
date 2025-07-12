using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _luggageCountText;
    [SerializeField] private PlayerLuggageCollector _luggageCollector;
    private void OnEnable()
    {
        _luggageCollector.OnLuggageAmountChanged += UpdateLuggageCount;
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
}
