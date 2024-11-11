using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenUI : MonoBehaviour
{
    private enum GameState { PLAYING, OVER};
    private GameState currentGameState;
    private float gameOverCountDown;
    private void Start()
    {
        GridManager.Instance.OnGameOver += GridManager_OnGameOver;
        GameManager.Instance.OnRestart += GameManager_OnRestart;
        currentGameState = GameState.PLAYING;
        gameOverCountDown = 1f;

        Hide();
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
        Hide();
    }
    private void Hide()
    {
        transform.localScale= Vector3.zero;
    }
    private void Show()
    {
        Debug.Log("SHow");
        transform.localScale = Vector3.one;
    }
}
