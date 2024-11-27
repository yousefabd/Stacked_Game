using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Button playGame;


    public event Action OnGameStarted;
    public event Action OnRestart;
    public event Action OnEdit;
    private enum GameState
    {
        EDITING,PLAYING,OVER
    }
    private GameState currentState;

    private void Awake()
    {
        currentState = GameState.EDITING;
        Instance = this;
    }
    private void Start()
    {
        HideLists();
        GridEditorUI.Instance.gameObject.transform.localScale = Vector3.one;
        GridEditorUI.Instance.OnConfirm += GridEditor_OnConfirm;
        GridManager.Instance.OnGameOver += GridManager_OnGameOver;
        playGame.onClick.AddListener(() =>
        {
            if (GridManager.Instance.IsValid())
            {
                currentState = GameState.PLAYING;
                HideLists();
                InGameOptionsUI.Instance.gameObject.transform.localScale = Vector3.one;
                OnGameStarted?.Invoke();
            }
        });
    }

    private void GridManager_OnGameOver()
    {
        currentState = GameState.OVER;
    }

    private void GridEditor_OnConfirm()
    {
        HideLists();
        PuzzleBlockEditorUI.Instance.gameObject.transform.localScale = Vector3.one;
    }

    private void HideLists()
    {
        PuzzleBlockEditorUI.Instance.transform.localScale = Vector3.zero;
        InGameOptionsUI.Instance.transform.localScale = Vector3.zero;
        GridEditorUI.Instance.transform.localScale = Vector3.zero;

    }

    public bool IsPlaying()
    {
        return currentState == GameState.PLAYING;
    }

    public void Restart()
    {
        OnRestart?.Invoke();
        currentState = GameState.PLAYING;
        Time.timeScale = 1f;
    }

    public void Edit()
    {
        OnEdit?.Invoke();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

}
