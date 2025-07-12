using UnityEngine;

public class LuggageDeposit : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _depositSfx;
    [SerializeField] private float _pitchIncrease = 0.05f;
    [SerializeField] private float _pitchResetTime = 3f;

    private float _pitchTimer = 3f;

    public void Deposit(Luggage luggage)
    {
        Destroy(luggage.gameObject);
        _audioSource.PlayOneShot(_depositSfx);
        _audioSource.pitch += _pitchIncrease;
        _pitchTimer = _pitchResetTime;
    }

    private void Update()
    {
        _pitchTimer -= Time.deltaTime;
        if ( _pitchTimer < 0 )
            _audioSource.pitch = 1;
    }
}
