using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize = 1;
    [SerializeField] private List<PuzzleBlock> testBlocks;
    [SerializeField] private PuzzleBlock obstacleBlock;

    private Vector2 originPosition;

    private GameGrid gameGrid;

    private void Start()
    {
        originPosition = new Vector2(-1f * ((height - 2f) / 2f), -1f * ((width - 2f) / 2f));
        gameGrid = new GameGrid(width, height);
        int randomBlockIndex = Random.Range(0, testBlocks.Count);

        CreateBlock(testBlocks[randomBlockIndex], new Vector2Int(0, 0));

        randomBlockIndex = Random.Range(0, testBlocks.Count);
        CreateBlock(testBlocks[randomBlockIndex], new Vector2Int(1, 1));

        randomBlockIndex = Random.Range(0, testBlocks.Count);
        CreateBlock(testBlocks[randomBlockIndex], new Vector2Int(2, 2));

        CreateBlock(obstacleBlock, new Vector2Int(2, 4));

        randomBlockIndex = Random.Range(0, testBlocks.Count);
        CreateBlock(testBlocks[randomBlockIndex], new Vector2Int(3, 3));

        gameGrid.OnTestMoveObject += GameGrid_OnTestMoveObject;
        gameGrid.OnMerged += GameGrid_OnMerged;

        InputManager.Instance.OnMove += InputManager_OnMove;
    }

    private void InputManager_OnMove(Vector2Int moveDir)
    {
        gameGrid.Move(moveDir);
    }

    private void GameGrid_OnTestMoveObject(PuzzleBlock puzzleBlock, int newX, int newY)
    {
        puzzleBlock.SetPosition(GridToWorldPosition(new Vector2Int(newX,newY)));
    }
    private void GameGrid_OnMerged(PuzzleBlock puzzleBlock)
    {
        puzzleBlock.Destruct();
    }
    public void CreateBlock(PuzzleBlock block, Vector2Int gridPosition)
    {
        PuzzleBlock puzzleBlock = Instantiate(block, GridToWorldPosition(gridPosition), Quaternion.identity);

        gameGrid.SetGridObject(gridPosition.x, gridPosition.y, puzzleBlock);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
        }
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.y * cellSize + originPosition.x, width - 1 - gridPosition.x + originPosition.y);
    }

    public void TestLogGameGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (gameGrid.GetPuzzleBlock(new Vector2Int(i,j)) == null)
                    Debug.Log("at: (" + i + " , " + j + "): .");
                else
                    Debug.Log("at: (" + i + " , " + j + "): " + gameGrid.GetPuzzleBlock(new Vector2Int(i,j)));
            }
        }
    }
}
