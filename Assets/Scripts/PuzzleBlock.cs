using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : MonoBehaviour, IMovableObject
{
    private enum PuzzleBlockState
    {
        MOVING,IDLE,BLOCKED
    }
    [SerializeField] private PuzzleBlockSO puzzleBlockSO;

    private PuzzleBlockState currentPuzzleBlockState;
    private Vector3 currentTargetPosition;
    private float blockSpeed;
    private bool destruct;

    public event Action<Vector2> OnSetMoveDir;
    public event Action OnReachedDestination;

    private void Start()
    {
        destruct = false;
        SetState(PuzzleBlockState.IDLE);
        if (puzzleBlockSO.IsBlock())
            currentPuzzleBlockState = PuzzleBlockState.BLOCKED;
    }

    private void Update()
    {
        switch (currentPuzzleBlockState)
        {
            case PuzzleBlockState.MOVING:
                Move();
                break;
            case PuzzleBlockState.IDLE:
                break;
            case PuzzleBlockState.BLOCKED:
                break;
        }
    }

    private void SetState(PuzzleBlockState newState)
    {
        if (currentPuzzleBlockState != PuzzleBlockState.BLOCKED)
            currentPuzzleBlockState = newState;
    }
    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, blockSpeed*Time.deltaTime);
        if (Vector3.Distance(transform.position, currentTargetPosition) <= 0.01)
        {
            currentPuzzleBlockState = PuzzleBlockState.IDLE;
            OnReachedDestination?.Invoke();
            if(destruct)
                Destroy(gameObject);
        }
    }

    public char GetCharSymbol()
    {
        return puzzleBlockSO.charSymbol;
    }

    public void SetPosition(Vector3 newPosition)
    {
        currentTargetPosition = newPosition;
        SetState(PuzzleBlockState.MOVING);
        blockSpeed = Vector2.Distance(transform.position, currentTargetPosition)/puzzleBlockSO.movementDuration;
        Vector2 moveDir = Vector3.Normalize(newPosition - transform.position);
        OnSetMoveDir?.Invoke(moveDir);
    }
    public void Destruct()
    {
        destruct = true;
    }
    public bool IsIdentical(PuzzleBlock targetPuzzleBlock)
    {
        return targetPuzzleBlock.GetCharSymbol() == GetCharSymbol();
    }
    public bool IsBlock()
    {
        return puzzleBlockSO.IsBlock();
    }
}
