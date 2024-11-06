using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private int width = 4;
    private int height = 4;
    [SerializeField] private float cellSize = 1;
    private Vector2 originPosition;
    [SerializeField] private PuzzleBlock obstacleBlock;
    private List<PuzzleBlockSO> initialBlocks;

    public event Action OnGameOver;

    private GameGrid gameGrid;
    private void Awake()
    {
        Instance = this;
        initialBlocks = new List<PuzzleBlockSO>();
    }

    private void Start()
    {
        ConfirmWorldGridSizes(width, height);

        InputManager.Instance.OnMove += OnMove;
        AutoMover.Instance.OnAutoMove += OnMove;
    }
    private void GameGrid_OnGameOver()
    {
        OnGameOver?.Invoke();
    }

    private void OnMove(Vector2Int moveDir)
    {
        if(GameManager.Instance.IsPlaying())
            gameGrid.Move(moveDir);
    }
    public void CreateBlock(PuzzleBlockSO puzzleBlockSO, Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.x >= width ||
           gridPosition.y < 0 || gridPosition.y >= height ||
           EventSystem.current.IsPointerOverGameObject() ||
           puzzleBlockSO == null)
            return;

        Transform puzzleBlockTransform = Instantiate(puzzleBlockSO.prefab, GridToWorldPosition(gridPosition), Quaternion.identity);
        PuzzleBlock puzzleBlock = puzzleBlockTransform.GetComponent<PuzzleBlock>();

        if (!gameGrid.SetGridObject(gridPosition.x, gridPosition.y, puzzleBlock))
        {
            Destroy(puzzleBlock.gameObject);
            return;
        }
        if(!initialBlocks.Contains(puzzleBlockSO))
            initialBlocks.Add(puzzleBlockSO);
    }
    public PuzzleBlockSO FindBlock(char symbol)
    {
        if (symbol == '.')
            return null;
        foreach (PuzzleBlockSO puzzleBlock in initialBlocks)
        {
            if (symbol == puzzleBlock.charSymbol)
            {
                return puzzleBlock;
            }
        }
        return initialBlocks[0];
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.y * cellSize + originPosition.x, width - 1 - gridPosition.x + originPosition.y);
    }
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int((int)(originPosition.y + width - 1 - worldPosition.y + cellSize / 2f), (int)((worldPosition.x - originPosition.x) / cellSize + cellSize / 2f));
    }
    public GameGrid GetGameGrid()
    {
        return gameGrid.GetCopy();
    }
    public void ClearGameGrid()
    {
        PuzzleBlock[,] grid= gameGrid.GetGrid();
        foreach(PuzzleBlock block in grid)
        {
            Destroy(block?.gameObject);
        }
    }
    public void LoadGrid(char[,] cGrid)
    {
        ClearGameGrid();
        gameGrid = new GameGrid(cGrid.GetLength(0),cGrid.GetLength(1));
        gameGrid.OnGameOver += GameGrid_OnGameOver;
        gameGrid.OnSyncMoves += SyncPuzzleBlocks;
        for (int i = 0; i < gameGrid.GetGrid().GetLength(0); i++)
        {
            for(int j = 0; j < gameGrid.GetGrid().GetLength(1); j++)
            {
                CreateBlock(FindBlock(cGrid[i, j]), new Vector2Int(i, j));
            }
        }

    }
    public void SyncPuzzleBlocks(Dictionary<PuzzleBlock, MoveAction> puzzleBlockMoves)
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

    public void SetWorldGridSizes(int width, int height)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        SerializedObject serializedObject = new(spriteRenderer);
        SerializedProperty sizeProperty = serializedObject.FindProperty("m_Size");
        sizeProperty.vector2Value = new Vector2(height, width);
        serializedObject.ApplyModifiedProperties();
    }

    public void ConfirmWorldGridSizes(int width,int height)
    {
        this.width = width;
        this.height = height;
        SetWorldGridSizes(width, height);
        originPosition = new Vector2(-1f * ((height - 2f) / 2f), -1f * ((width - 2f) / 2f));
        gameGrid = new GameGrid(width, height);
        gameGrid.OnGameOver += GameGrid_OnGameOver;
        gameGrid.OnSyncMoves += SyncPuzzleBlocks;
    }
}