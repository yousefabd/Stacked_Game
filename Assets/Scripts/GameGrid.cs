using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GameGrid
{
    private int width;
    private int height;
    private PuzzleBlock[,] gameGrid;

    public event Action<PuzzleBlock,int,int> OnTestMoveObject;
    public event Action<PuzzleBlock> OnMerged; 

    public GameGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        gameGrid = new PuzzleBlock[width, height];
    }
    private bool IsBlocked(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
            return true;
        if (gameGrid[pos.x, pos.y] == null)
            return false;
        if (gameGrid[pos.x, pos.y].IsBlock())
            return true;
        return false;
    }
    private bool IsIdentical(PuzzleBlock targetPuzzleBlock,Vector2Int currentPosition)
    {
        if (IsBlocked(currentPosition))
            return false;
        if(gameGrid[currentPosition.x, currentPosition.y] == null)
            return false;
        return gameGrid[currentPosition.x, currentPosition.y].IsIdentical(targetPuzzleBlock);
    }
    private bool IsClear(Vector2Int pos)
    {
        return gameGrid[pos.x, pos.y] == null;
    }
    private bool IsColoredBlock(Vector2Int pos)
    {
        return !IsBlocked(pos) && !IsClear(pos);
    }
    private Vector2Int GetFuturePosition(Vector2Int currentPosition, Vector2Int moveDir)
    {
        Vector2Int offsetValue = new Vector2Int(-1 * moveDir.x, -1 * moveDir.y);
        Vector2Int currentOffset = Vector2Int.zero;
        Vector2Int futurePosition = currentPosition;
        bool canMerge = true;
        while (!IsBlocked(futurePosition + moveDir))
        {
            futurePosition += moveDir;
            if (!IsIdentical(GetPuzzleBlock(currentPosition), futurePosition) && !IsClear(futurePosition))
            {
                canMerge = false;
                currentOffset += offsetValue;
            }
            else if(IsIdentical(GetPuzzleBlock(currentPosition),futurePosition) && !canMerge)
                currentOffset += offsetValue;
        }
        return futurePosition + currentOffset;
    }
    public void Move(Vector2Int moveDir)
    {

        Vector2Int forceDir = new Vector2Int(-1 * moveDir.y, moveDir.x);
        int outerLimit = (moveDir.x == 0 ? height : width);
        int innerLimit = (width * height) / outerLimit;
        Dictionary<PuzzleBlock, Vector2Int> newPositions = new Dictionary<PuzzleBlock, Vector2Int>();
        for (int i = 0; i < outerLimit; i++)
        {
            Queue<Vector2Int> availablePositions = new Queue<Vector2Int>();

            for (int j = 0; j < innerLimit; j++)
            {
                int x = Math.Abs(forceDir.x * j + forceDir.y * i);
                int y = Math.Abs(forceDir.y * j + forceDir.x * i);
                int r = forceDir.x > 0 ? width  - 1 - x : x;
                int c = forceDir.y > 0 ? height - 1 - y : y;
                Vector2Int pos = new Vector2Int(r, c);
                if(IsColoredBlock(pos))
                {
                    if (availablePositions.Any())
                    {
                        Vector2Int newPos = availablePositions.Dequeue();
                        MovePuzzleBlock(pos, newPos);
                    }
                }
                else if (IsClear(pos))
                {
                    availablePositions.Enqueue(pos);
                }
                else
                {
                    availablePositions.Clear();
                }
            }
        }
    }

    private void MovePuzzleBlock(Vector2Int oldPosition, Vector2Int newPosition)
    {
        gameGrid[newPosition.x,newPosition.y] = gameGrid[oldPosition.x,oldPosition.y];
        gameGrid[oldPosition.x, oldPosition.y] = null;
        OnTestMoveObject?.Invoke(gameGrid[newPosition.x,newPosition.y],newPosition.x, newPosition.y);
    }

    private void ClearGameGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!IsBlocked(new Vector2Int(i, j)))
                    gameGrid[i, j] = null;
            }
        }
    }
    public void SetGridObject(int x,int y,PuzzleBlock puzzleBlock)
    {
        gameGrid[x, y] = puzzleBlock;   
    }

    public PuzzleBlock GetPuzzleBlock(Vector2Int position)
    {
        return gameGrid[position.x, position.y];
    }


}
