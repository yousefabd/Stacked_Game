using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Button playGame;
    [SerializeField] private PuzzleBlockEditorUI puzzleBlockEditor;

    public event Action OnGameStarted;
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
        playGame.onClick.AddListener(() =>
        {
            currentState = GameState.PLAYING;
            puzzleBlockEditor.gameObject.SetActive(false);
            OnGameStarted?.Invoke();
        });
    }

    public bool IsPlaying()
    {
        return currentState == GameState.PLAYING;
    }
}
