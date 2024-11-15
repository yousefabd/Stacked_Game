using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wonMessage;
    private enum GameState { PLAYING, OVER};
    private GameState currentGameState;
    private float gameOverCountDown;


    private void Start()
    {
        GridManager.Instance.OnGameOver += GridManager_OnGameOver;
        GameManager.Instance.OnRestart += GameManager_OnRestart;
        GameManager.Instance.OnEdit += GameManager_OnEdit;
        StrategyManager.Instance.OnConfirmStrategy += StrategyManager_OnConfirmStrategy;
        currentGameState = GameState.PLAYING;
        gameOverCountDown = 1f;

        Hide();
    }

    private void GameManager_OnEdit()
    {
        Time.timeScale = 1;
    }

    private void StrategyManager_OnConfirmStrategy(string strategy)
    {
        if (strategy.Equals(string.Empty))
            wonMessage.text = "YOU WON!";
        else
            wonMessage.text = "AI WON!";
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.PLAYING:
                break;
            case GameState.OVER:
                CountDown();
                break;
        }
    }
    private void CountDown()
    {
        gameOverCountDown -= Time.deltaTime;
        if (gameOverCountDown < 0f) {
            Show();
            currentGameState = GameState.PLAYING;
            gameOverCountDown = 1f;
            Time.timeScale = 0f;
        }
    }
    private void GridManager_OnGameOver()
    {
        currentGameState = GameState.OVER;
    }

    private void GameManager_OnRestart()
    {
        Time.timeScale = 1f;
        Hide();
    }
    private void Hide()
    {
        transform.localScale= Vector3.zero;
    }
    private void Show()
    {
        transform.localScale = Vector3.one;
    }
}
