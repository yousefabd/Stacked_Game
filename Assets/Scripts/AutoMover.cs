using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AutoMover : MonoBehaviour
{
    public static AutoMover Instance { get; private set; }
    private enum AutoMoveState
    {
        MOVING,IDLE
    }
    private AutoMoveState currentMoveState;
    private int currentMoveIndex;
    private float maxTimeCooldown = 0.5f;
    private float currentTimeCooldown = 1f;
    private List<Vector2Int> currentForcesList;

    public event Action<Vector2Int> OnAutoMove;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentMoveState = AutoMoveState.IDLE;
        StateManager.Instance.OnFindSolution += StateManager_OnFindSolution;
    }

    private void StateManager_OnFindSolution(List<Vector2Int> solution,bool move)
    {
        if (!move)
            return;
        SetForcesList(solution);
        string solutionString = "";
        foreach (Vector2Int forceDir in solution)
        {
            string forceString = "";
            if (forceDir.x == -1)
                forceString = "UP";
            else if (forceDir.y == -1)
                forceString = "LEFT";
            else if (forceDir.x == 1)
                forceString = "DOWN";
            else
                forceString = "RIGHT";
            solutionString += forceString + " =>";
        }
        Debug.Log(solutionString + "END!");
    }

    private void Update()
    {
        switch (currentMoveState)
        {
            case AutoMoveState.MOVING:
                Move();
                break;
            case AutoMoveState.IDLE:
                break;
        }
    }
    private void Move()
    {
        if(currentTimeCooldown > 0)
        {
            currentTimeCooldown -= Time.deltaTime;
            return;
        }
        currentTimeCooldown = maxTimeCooldown;
        if(currentMoveIndex >= currentForcesList.Count)
        {
            currentMoveState = AutoMoveState.IDLE;
            currentMoveIndex = 0;
            currentForcesList.Clear();
            return;
        }
        Vector2Int forceDir = currentForcesList[currentMoveIndex];
        OnAutoMove?.Invoke(new Vector2Int(forceDir.y, -1 * forceDir.x));
        currentMoveIndex++;
    }
    private void SetForcesList(List<Vector2Int > forcesList)
    {
        currentForcesList = forcesList;
        currentMoveIndex = 0;
        currentMoveState = AutoMoveState.MOVING;
    }
}
