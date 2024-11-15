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
    private int currentMoves = 0;
    private int optimalMoves;
    private int currentStarIndex;

    private void Start()
    {
        GridManager.Instance.OnMakeMove += Increase;
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnRestart += GameManager_OnRestart;
        StateManager.Instance.OnFindSolution += StateManager_OnFindSolution;
        currentStarIndex = stars.Count - 1;
    }

    private void StateManager_OnFindSolution(List<Vector2Int> solution, bool move)
    {
        if (move)
            return;
        optimalMoves = solution.Count;
        optimalMovesText.text = optimalMoves.ToString();
    }

    private void GameManager_OnRestart()
    {
        currentMoves = 0;
        currentMovesText.text = currentMoves.ToString();
        currentStarIndex = stars.Count-1;
        ResetStars();
    }

    private void GameManager_OnGameStarted()
    {
        currentMovesText.text = currentMoves.ToString();
    }

    private void Increase(Vector3 arg)
    {
        currentMoves++;
        if(currentMoves > optimalMoves && currentStarIndex >= 0)
        {
            Transform star = stars[currentStarIndex--];
            star.gameObject.SetActive(false);
        }
        currentMovesText.text = currentMoves.ToString();
    }

    private void ResetStars()
    {
        foreach(var item in stars)
        {
            item.gameObject.SetActive(true);
        }
    }
}
