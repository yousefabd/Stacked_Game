using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize = 1;
    private Vector2 originPosition;
    [SerializeField] private PuzzleBlock obstacleBlock;
    private List<PuzzleBlock> initialBlocks;

    public event Action OnGameOver;

    private GameGrid gameGrid;
    private void Awake()
    {
        Instance = this;
        initialBlocks = new List<PuzzleBlock>();
    }

    private void Start()
    {
        originPosition = new Vector2(-1f * ((height - 2f) / 2f), -1f * ((width - 2f) / 2f));
        gameGrid = new GameGrid(width, height);

        gameGrid.OnGameOver += GameGrid_OnGameOver;
        gameGrid.OnSyncMoves += SyncPuzzleBlocks;

        InputManager.Instance.OnMove += InputManager_OnMove;
    }
    private void GameGrid_OnGameOver()
    {
        OnGameOver?.Invoke();
    }

    private void InputManager_OnMove(Vector2Int moveDir)
    {
        gameGrid.Move(moveDir);
    }
    public void CreateBlock(PuzzleBlockSO puzzleBlockSO, Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.x >= width ||
           gridPosition.y < 0 || gridPosition.y >= height ||
           EventSystem.current.IsPointerOverGameObject())
            return;
        Transform puzzleBlockTransform = Instantiate(puzzleBlockSO.prefab, GridToWorldPosition(gridPosition), Quaternion.identity);
        PuzzleBlock puzzleBlock = puzzleBlockTransform.GetComponent<PuzzleBlock>();

       if(!gameGrid.SetGridObject(gridPosition.x, gridPosition.y, puzzleBlock))
       {
            Destroy(puzzleBlock.gameObject);
            return;
       }
       initialBlocks.Add(puzzleBlock);
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.y * cellSize + originPosition.x, width - 1 - gridPosition.x + originPosition.y);
    }
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int((int)(originPosition.y + width - 1 - worldPosition.y+cellSize/2f), (int)((worldPosition.x - originPosition.x) / cellSize + cellSize/2f));
    }
    public int GetColoredBlocksCount()
    {
        return initialBlocks.Count;
    }
    private void SyncPuzzleBlocks(Dictionary<PuzzleBlock,MoveAction> puzzleBlockMoves)
    {
        foreach (PuzzleBlock puzzleBlock in puzzleBlockMoves.Keys)
        {
            MoveAction moveAction = puzzleBlockMoves[puzzleBlock];
            MovePuzzleBlockVisual(puzzleBlock, moveAction.newPos, moveAction.destroy);
        }
    }
    private void MovePuzzleBlockVisual(PuzzleBlock puzzleBlock, Vector2Int newPosition, bool destroy = false)
    {
        puzzleBlock.SetPosition(GridToWorldPosition(newPosition));
        if (destroy)
        {
            puzzleBlock.Destruct();
        }
    }
}
