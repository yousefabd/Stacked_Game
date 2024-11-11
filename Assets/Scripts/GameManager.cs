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
        EDITING,PLAYING
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
        GridEditorUI.Instance.gameObject.SetActive(true);
        GridEditorUI.Instance.OnConfirm += GridEditor_OnConfirm;
        playGame.onClick.AddListener(() =>
        {
            currentState = GameState.PLAYING;
            HideLists();
            InGameOptionsUI.Instance.gameObject.SetActive(true);
            OnGameStarted?.Invoke();
        });
    }

    private void GridEditor_OnConfirm()
    {
        HideLists();
        PuzzleBlockEditorUI.Instance.gameObject.SetActive(true);
    }

    private void HideLists()
    {
        PuzzleBlockEditorUI.Instance.gameObject.SetActive(false);
        InGameOptionsUI.Instance.gameObject.SetActive(false);
        GridEditorUI.Instance.gameObject.SetActive(false);

    }

    public bool IsPlaying()
    {
        return currentState == GameState.PLAYING;
    }

    public void Restart()
    {
        OnRestart?.Invoke();
        Time.timeScale = 1f;
    }

    public void Edit()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        OnEdit?.Invoke();
    }

}
