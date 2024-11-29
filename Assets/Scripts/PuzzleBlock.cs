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
    private float moveDistance;
    private bool destruct;
    private bool fuseInto;

    public event Action<Vector2> OnSetMoveDir;
    public event Action OnReachedDestination;
    public event Action OnFuse;
    public event Action OnFusedInto;

    private void Start()
    {
        destruct = false;
        fuseInto = false;
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
            if (destruct)
            {
                OnFuse?.Invoke();
                Destroy(gameObject);
            }
            else if (fuseInto)
            {
                OnFusedInto?.Invoke();
                fuseInto = false;
            }
            if(moveDistance >= 0.5f)
                OnReachedDestination?.Invoke();
        }
    }

    public char GetCharSymbol()
    {
        return puzzleBlockSO.charSymbol;
    }

    public void SetPosition(Vector3 newPosition)
    {
        currentTargetPosition = newPosition;
        moveDistance = Vector3.Distance(transform.position, currentTargetPosition);
        SetState(PuzzleBlockState.MOVING);
        blockSpeed = Vector2.Distance(transform.position, currentTargetPosition)/puzzleBlockSO.movementDuration;
        Vector2 moveDir = Vector3.Normalize(newPosition - transform.position);
        OnSetMoveDir?.Invoke(moveDir);
    }
    public void FuseInto()
    {
        fuseInto = true;
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
