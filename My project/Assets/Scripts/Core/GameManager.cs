using DG.Tweening;
using GameCells.Utilities;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private NewPlayerMovement _playerMovement;

    [field: SerializeField] public float Timer { get; private set; } = 120f;
    [field: SerializeField] public float DefaultScorePerLuggage { get; private set; } = 20f;

    public int LuggagesDeposited { get; private set; } = 0;
    public int Score { get; private set; } = 0;
    private bool _countdown;
    private float _countdownTimer = 4f;

    private void Start()
    {
        _countdown = true;
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
            }
        }
        else
        {
            Timer -= Time.deltaTime;
            _timerText.text = $"{(int)(Timer)}";
            if (Timer < 0)
                GameEnd();
        }
            
    }

    private void GameEnd()
    {
        _playerMovement.SetCanMove(false);
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
}
