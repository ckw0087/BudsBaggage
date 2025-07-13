using DG.Tweening;
using GameCells.Utilities;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private PlayerLuggageCollector _luggageCollector;
    [SerializeField] private AudioClip _defaultMusic;
    [SerializeField] private AudioClip _feverMusic;
    [SerializeField] private AudioSource _bgmSource1;
    [SerializeField] private AudioSource _bgmSource2;

    private bool _bgmSource1Playing = true;

    private void OnEnable()
    {
        _luggageCollector.OnFeverStarted += PlayFeverMusic;
        _luggageCollector.OnFeverEnd += PlayNormalMusic;
    }

    public void ChangeMusic(AudioClip clip)
    {
        if (_bgmSource1Playing)
        {
            _bgmSource1.DOKill(true);
            _bgmSource1.DOFade(0f, 2f);

            _bgmSource2.DOKill(true);
            _bgmSource2.clip = clip;
            _bgmSource2.Play();
            _bgmSource2.DOFade(1f, 2f);
        }
        else
        {
            _bgmSource2.DOKill(true);
            _bgmSource2.DOFade(0f, 2f);

            _bgmSource1.DOKill(true);
            _bgmSource1.clip = clip;
            _bgmSource1.Play();
            _bgmSource1.DOFade(1f, 2f);
        }
    }

    public void PlayNormalMusic()
    {
        ChangeMusic(_defaultMusic);
    }

    public void PlayFeverMusic()
    {
        ChangeMusic(_feverMusic);
    }
}
