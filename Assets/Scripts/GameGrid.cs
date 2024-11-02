using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameGrid
{
    private readonly int width;
    private readonly int height;
    private readonly PuzzleBlock[,] gameGrid;

    public event Action<PuzzleBlock,int,int> OnMoveBlock;
    public event Action<PuzzleBlock> OnFuseBlock;
    public event Action OnGameOver;

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
    private bool IsGameOver()
    {
        int blocksCount = 0;
        HashSet<char> colors = new HashSet<char>();
        for(int i = 0; i < width; i++)
        {
            for(int j=0;j<height; j++)
            {
                if (IsColoredBlock(new Vector2Int(i, j)))
                {
                    blocksCount++;
                    colors.Add(gameGrid[i, j].GetCharSymbol());
                }
            }
        }
        return (blocksCount == colors.Count());
    }
    public void Move(Vector2Int moveDir)
    {

        Vector2Int forceDir = new Vector2Int(-1 * moveDir.y, moveDir.x);
        int outerLimit = (moveDir.x == 0 ? height : width);
        int innerLimit = (width * height) / outerLimit;
        for (int i = 0; i < outerLimit; i++)
        {
            Queue<Vector2Int> availablePositions = new Queue<Vector2Int>();
            Vector2Int currentFacingBlockPos = new Vector2Int(-1, -1);
            for (int j = 0; j < innerLimit; j++)
            {
                int x = Math.Abs(forceDir.x * j + forceDir.y * i);
                int y = Math.Abs(forceDir.y * j + forceDir.x * i);
                int r = forceDir.x > 0 ? width  - 1 - x : x;
                int c = forceDir.y > 0 ? height - 1 - y : y;
                Vector2Int pos = new Vector2Int(r, c);
                if(IsColoredBlock(pos))
                {
                    if (IsIdentical(gameGrid[pos.x, pos.y], currentFacingBlockPos))
                    {
                        OnMoveBlock?.Invoke(gameGrid[pos.x, pos.y], currentFacingBlockPos.x, currentFacingBlockPos.y);
                        OnFuseBlock?.Invoke(gameGrid[pos.x, pos.y]);
                        gameGrid[pos.x, pos.y] = null;
                        availablePositions.Enqueue(pos);
                    }
                    else if (availablePositions.Any())
                    {
                        Vector2Int newPos = availablePositions.Dequeue();
                        currentFacingBlockPos = newPos;
                        MovePuzzleBlock(pos, newPos);
                        availablePositions.Enqueue(pos);
                    }
                    else
                    {
                        currentFacingBlockPos = pos;
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
        if (IsGameOver())
            OnGameOver?.Invoke();
    }

    private void MovePuzzleBlock(Vector2Int oldPosition, Vector2Int newPosition)
    {
        gameGrid[newPosition.x,newPosition.y] = gameGrid[oldPosition.x,oldPosition.y];
        gameGrid[oldPosition.x, oldPosition.y] = null;
        OnMoveBlock?.Invoke(gameGrid[newPosition.x,newPosition.y],newPosition.x, newPosition.y);
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
