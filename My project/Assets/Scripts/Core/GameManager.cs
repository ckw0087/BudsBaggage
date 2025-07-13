using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioClip _drumrollSfx;
    [SerializeField] private AudioClip _drumrollEndSfx;
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Image _timerImage;
    [SerializeField] private CanvasGroup _summaryUI;
    [SerializeField] private TMP_Text _finalLuggageCountText;
    [SerializeField] private TMP_Text _finalScoreText;
    [SerializeField] private CanvasGroup _buttonsCanvasGroup;
    [SerializeField] private NewPlayerMovement _playerMovement;

    [field: SerializeField] public float Timer { get; private set; } = 120f;
    [field: SerializeField] public float DefaultScorePerLuggage { get; private set; } = 20f;

    private float initialTimer;
    public int LuggagesDeposited { get; private set; } = 0;
    public int Score { get; private set; } = 0;
    private bool _countdown;
    private float _countdownTimer = 4f;
    private bool _gameEnded = false;
    private void Start()
    {
        _countdown = true;

        initialTimer = Timer;
        _timerText.text = Timer.ToString();

        _playerMovement.SetCanMove(false);
    }

    private void Update()
    {
        if (_countdown)
        {
            _countdownTimer -= Time.deltaTime;
            _countdownText.text = $"{(int)(_countdownTimer)}";
            if (_countdownTimer <= 1)
            {
                _playerMovement.SetCanMove(true);
                _countdownText.text = "GO!";
                _countdownText.rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
                _countdownText.DOFade(0f, 0.25f).SetDelay(0.5f);
                _countdown = false;
            }
        }
        else if (!_gameEnded)
        {
            Timer -= Time.deltaTime;
            _timerText.text = $"{(int)(Timer)}";
            _timerImage.fillAmount = Timer / initialTimer;
            if (Timer < 0)
                GameEnd();
        }
            
    }

    private void GameEnd()
    {
        _gameEnded = true;
        _playerMovement.SetCanMove(false);
        _countdownText.alpha = 1;
        _countdownText.text = "TIME UP!";
        _audioManager.StopMusic();
        _countdownText.rectTransform.localScale = Vector3.one;
        _countdownText.rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
        StartCoroutine(SummaryCO());
    }

    private IEnumerator SummaryCO()
    {
        yield return WaitHandler.GetWaitForSeconds(1f);
        _countdownText.alpha = 0;
        _summaryUI.gameObject.SetActive(true);
        _audioManager.PlayGlobalSFX(_drumrollSfx);

        int luggageCount = 0;
        while (luggageCount < LuggagesDeposited)
        {
            luggageCount++;
            _finalLuggageCountText.text = $"x {luggageCount}";
            yield return WaitHandler.GetWaitForSeconds(0.05f);
        }

        int finalScore = 0;
        while (finalScore <= Score)
        {
            finalScore += 10;
            _finalScoreText.text = $"FINAL SCORE: {Score}";
            yield return WaitHandler.GetWaitForSeconds(0.01f);
        }

        _audioManager.PlayGlobalSFX(_drumrollEndSfx);

        yield return WaitHandler.GetWaitForSeconds(1f);

        _buttonsCanvasGroup.gameObject.SetActive(true);
    }

    public void DepositLuggage(int combo)
    {
        LuggagesDeposited++;

        int multipler = 1;
        if (combo >= 60)
            multipler = 5;
        else if (combo >= 45)
            multipler = 4;
        else if (combo >= 30)
            multipler = 3;
        else if (combo >= 15)
            multipler = 2;

        Score += (int)(DefaultScorePerLuggage * multipler);
        _scoreText.text = $"Score: {Score}";
        _scoreText.rectTransform.DOKill(true);
        _scoreText.rectTransform.DOPunchScale(Vector3.one * 0.85f, 0.2f);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
