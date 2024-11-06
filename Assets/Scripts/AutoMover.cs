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
    private float maxTimeCooldown = 1f;
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
    public void SetForcesList(List<Vector2Int > forcesList)
    {
        currentForcesList = forcesList;
        currentMoveIndex = 0;
        currentMoveState = AutoMoveState.MOVING;
    }
}
