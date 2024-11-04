using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameGrid
{
    private readonly int width;
    private readonly int height;
    private readonly PuzzleBlock[,] gameGrid;

    public event Action OnGameOver;
    public event Action<Dictionary<PuzzleBlock,MoveAction>> OnSyncMoves;

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
    public void ApplyForce(Vector2Int forceDir ,PuzzleBlock[,] gameGrid,out Dictionary<PuzzleBlock,MoveAction> puzzleBlockMoves)
    {
        puzzleBlockMoves = new Dictionary<PuzzleBlock,MoveAction>();
        int width = gameGrid.GetLength(0);
        int height = gameGrid.GetLength(1);
        int outerLimit = (forceDir.y == 0 ? height : width);
        int innerLimit = (width * height) / outerLimit;
        for (int i = 0; i < outerLimit; i++)
        {
            Queue<Vector2Int> availablePositions = new Queue<Vector2Int>();
            Vector2Int currentFacingBlockPos = new Vector2Int(-1, -1);
            for (int j = 0; j < innerLimit; j++)
            {
                int x = Math.Abs(forceDir.x * j + forceDir.y * i);
                int y = Math.Abs(forceDir.y * j + forceDir.x * i);
                int r = forceDir.x > 0 ? width - 1 - x : x;
                int c = forceDir.y > 0 ? height - 1 - y : y;
                Vector2Int pos = new Vector2Int(r, c);
                if (IsColoredBlock(pos))
                {
                    if (IsIdentical(gameGrid[pos.x, pos.y], currentFacingBlockPos))
                    {
                        Debug.Log(gameGrid[pos.x, pos.y] + " at " + pos + " is identical to the one at " + currentFacingBlockPos);
                        puzzleBlockMoves[gameGrid[pos.x, pos.y]] = new MoveAction(currentFacingBlockPos, true);
                        MovePuzzleBlock(pos, currentFacingBlockPos, gameGrid, true);
                        availablePositions.Enqueue(pos);
                    }
                    else if (availablePositions.Any())
                    {
                        Vector2Int newPos = availablePositions.Dequeue();
                        currentFacingBlockPos = newPos;
                        puzzleBlockMoves[gameGrid[pos.x,pos.y]] = new MoveAction(newPos, false);
                        MovePuzzleBlock(pos, newPos, gameGrid);
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
                    currentFacingBlockPos = pos;
                    availablePositions.Clear();
                }
            }
        }
    }
    public List<PuzzleBlock[,]> GetAdjacentGrids()
    {
        List<PuzzleBlock[,]> adjacents = new List<PuzzleBlock[,]>();
        Vector2Int[] moves = { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1) };
        for (int i = 0; i < moves.Count(); i++)
        {
            PuzzleBlock[,] move = gameGrid;
            ApplyForce(moves[i], move,out _);
            adjacents.Add(move);
        }
        return adjacents;
    }
    public void Move(Vector2Int moveDir)
    {
        Vector2Int forceDir = new Vector2Int(-1 * moveDir.y, moveDir.x);
        ApplyForce(forceDir,gameGrid,out Dictionary<PuzzleBlock,MoveAction> puzzleBlockMoves);
        OnSyncMoves?.Invoke(puzzleBlockMoves);
        if (IsGameOver())
        {
            OnGameOver?.Invoke();
        }
    }
    public bool SetGridObject(int x, int y, PuzzleBlock puzzleBlock)
    {
        if (!IsClear(new Vector2Int(x, y)))
            return false;
        gameGrid[x, y] = puzzleBlock;
        return true;
    }

    private void MovePuzzleBlock(Vector2Int oldPosition, Vector2Int newPosition, PuzzleBlock[,] gameGrid, bool destroy = false)
    {
        if (destroy)
        {
            gameGrid[oldPosition.x, oldPosition.y] = null;
            return;
        }
        gameGrid[newPosition.x,newPosition.y] = gameGrid[oldPosition.x,oldPosition.y];
        gameGrid[oldPosition.x, oldPosition.y] = null;
    }
}
