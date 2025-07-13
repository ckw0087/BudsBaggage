using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class LuggageDeposit : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _depositSfx;
    [SerializeField] private float _pitchIncrease = 0.05f;
    [SerializeField] private float _pitchResetTime = 3f;

    private float _pitchTimer = 3f;

    public void Deposit(Luggage luggage, int combo)
    {
        Destroy(luggage.gameObject);
        GameManager.Instance.DepositLuggage(combo);
        _audioSource.PlayOneShot(_depositSfx);
        _audioSource.pitch += _pitchIncrease;
        _audioSource.pitch = Mathf.Clamp(_audioSource.pitch, 1f, 2.5f);
        _pitchTimer = _pitchResetTime;

        transform.DOKill(true);
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    private void Update()
    {
        _pitchTimer -= Time.deltaTime;
        if (_pitchTimer < 0)
            _audioSource.pitch = 1;
    }
}
