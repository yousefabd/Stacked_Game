using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenUI : MonoBehaviour
{
    private enum GameState { PLAYING, OVER};
    private GameState currentGameState;
    private float gameOverCountDown;

    [SerializeField] private Button restartButton;
    private void Start()
    {
        GridManager.Instance.OnGameOver += GridManager_OnGameOver;
        currentGameState = GameState.PLAYING;
        gameOverCountDown = 1f;
        restartButton.onClick.AddListener(() =>
        {
            Debug.Log("restart");
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        });
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
            Time.timeScale = 0f;
        }
    }
    private void GridManager_OnGameOver()
    {
        currentGameState = GameState.OVER;
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
