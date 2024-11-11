using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MovesCounterUI : MonoBehaviour
{
    [SerializeField] private List<Transform> stars;
    [SerializeField] private TextMeshProUGUI currentMovesText;
    [SerializeField] private TextMeshProUGUI optimalMovesText;
    [SerializeField] StateManager stateManager;
    private int currentMoves = 0;
    private int optimalMoves;

    private void Start()
    {
        GridManager.Instance.OnMakeMove += Increase;
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
    }

    private void GameManager_OnGameStarted()
    {
        optimalMoves = stateManager.GetOptimalMoves();
        currentMovesText.text = currentMoves.ToString();
        optimalMovesText.text = optimalMoves.ToString();
    }

    private void Increase()
    {
        currentMoves++;
        if(currentMoves > optimalMoves && stars.Any())
        {
            Transform star = stars[^1];
            stars.RemoveAt(stars.Count - 1);
            star.gameObject.SetActive(false);
        }
        currentMovesText.text = currentMoves.ToString();
    }
}
