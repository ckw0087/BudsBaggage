using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupTimerSlot : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _timerText;

    private float _remaining;
    private Action _onExpire;
    private Coroutine _countdown;

    private void Awake()
    {
        if (_iconImage) _iconImage.gameObject.SetActive(false);
        if (_timerText) _timerText.gameObject.SetActive(false);
    }

    public void Init(Sprite icon, float duration, Action onExpire)
    {
        _iconImage.sprite = icon;
        _remaining = duration;
        _onExpire = onExpire;

        _iconImage.gameObject.SetActive(true);
        _timerText.gameObject.SetActive(true);

        if (_countdown != null) StopCoroutine(_countdown);
        _countdown = StartCoroutine(Countdown());
    }

    public void Restart(float duration)
    {
        _remaining = duration;
        if (_countdown != null) StopCoroutine(_countdown);
        _countdown = StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (_remaining > 0f)
        {
            _timerText.text = Mathf.CeilToInt(_remaining).ToString() + "s";
            _remaining -= Time.deltaTime;
            yield return null;
        }
        _onExpire?.Invoke();
    }
}
