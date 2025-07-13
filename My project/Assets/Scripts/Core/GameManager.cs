using DG.Tweening;
using GameCells.Utilities;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timerText;

    [field: SerializeField] public float Timer { get; private set; } = 120f;
    [field: SerializeField] public float DefaultScorePerLuggage { get; private set; } = 20f;

    public int LuggagesDeposited { get; private set; } = 0;
    public int Score { get; private set; } = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        _timerText.text = $"{(int)(Timer)}";
        if (Timer < 0)
            GameEnd();
    }

    private void GameEnd()
    {

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
