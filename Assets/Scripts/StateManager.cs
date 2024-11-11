using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private class GridState
    {
        public GridState previousState;
        public Vector2Int previousForceDir;
        public GameGrid currentGrid;
        public GridState(GridState previousState,Vector2Int previousForceDir,GameGrid previousGrid)
        {
            this.previousState = previousState;
            this.previousForceDir = previousForceDir;
            currentGrid = previousGrid.GetCopy();
            currentGrid.ApplyForce(previousForceDir, out _);
        }
        public bool IsFinalState()
        {
            return currentGrid.IsGameOver();
        }

        public string GetKey()
        {
            string key = "";
            char[,] cGrid = GetGridAsChar(currentGrid);
            for (int i = 0; i < cGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cGrid.GetLength(1); j++)
                {
                    key += cGrid[i, j];
                }
            }
            return key;
        }
        public static char[,] GetGridAsChar(GameGrid grid)
        {
            Vector2Int size = grid.GetAxis();
            char[,] cGrid = new char[size.x, size.y];
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    cGrid[i, j] = grid.GetChar(i, j);
                }
            }
            return cGrid;
        }
    }
    private char[,] startGrid;
    private Dictionary<string, bool> visitedGrids;
    private int optimalMoves;

    private void Start()
    {
        visitedGrids = new Dictionary<string, bool>();
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnRestart += GameManager_OnRestart;
    }

    private void GameManager_OnRestart()
    {
        if(startGrid != null)
        {
            GridManager.Instance.LoadGrid(startGrid);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && startGrid != null)
        {
            GridManager.Instance.LoadGrid(startGrid);
        }
    }
    private void GameManager_OnGameStarted()
    {
        GameGrid grid = GridManager.Instance.GetGameGrid();
        startGrid = GridState.GetGridAsChar(grid);
        SolveBFS(new GridState(null,default,grid),true);
    }
    private List<GridState> GetAdjacentStates(GridState currentState)
    {
        List<Vector2Int> forceDirs = new List<Vector2Int> {
            new Vector2Int(-1, 0), // UP
            new Vector2Int( 1, 0), //DOWN
            new Vector2Int( 0,-1), //LEFT
            new Vector2Int( 0, 1)  //RIGHT
        };
        List<GridState> adjacentStates = new List<GridState>();
        foreach(Vector2Int forceDir  in forceDirs)
        {
            GridState dirState = new GridState(currentState, forceDir,currentState.currentGrid);
            adjacentStates.Add(dirState);
        }
        return adjacentStates;
    }

    private bool SolveDFS(GridState currentState, bool move = false)
    {
        List<GridState> adjacentStates = GetAdjacentStates(currentState);
        foreach (GridState adjacent in adjacentStates)
        {
            if (visitedGrids.ContainsKey(adjacent.GetKey()))
                continue;
            visitedGrids[adjacent.GetKey()] = true;
            if (adjacent.IsFinalState())
            {
                RetractSolution(adjacent,move);
                return true;
            }
            else if(SolveDFS(adjacent))
            {
                return true;
            }
        }
        return false;
    }
    private bool SolveBFS(GridState startState, bool move=false)
    {
        visitedGrids[startState.GetKey()] = true;
        Queue<GridState> queue = new Queue<GridState>();
        queue.Enqueue(startState);
        while (queue.Count > 0)
        {
            GridState currentState = queue.Dequeue();
            if (currentState.IsFinalState())
            {
                optimalMoves = RetractSolution(currentState,move);
                return true;
            }
            List<GridState> adjacencies = GetAdjacentStates(currentState);
            foreach(GridState adjacent in adjacencies)
            {
                if (!visitedGrids.ContainsKey(adjacent.GetKey()))
                {
                    queue.Enqueue(adjacent);
                    visitedGrids[adjacent.GetKey()] = true;
                }
            }
        }
        return false;
    }
    private int RetractSolution(GridState finalState, bool move=false)
    {
        Debug.Log("SOLVED");
        List<Vector2Int> solutionDirs = new List<Vector2Int>();
        GridState currentState = finalState;
        while (currentState != null)
        {
            solutionDirs.Add(currentState.previousForceDir);
            currentState = currentState.previousState;
        }
        solutionDirs.Reverse();
        if (move)
            AutoMover.Instance.SetForcesList(solutionDirs);
        return solutionDirs.Count - 1;
    }

    public int GetOptimalMoves()
    {
        GameGrid grid = GridManager.Instance.GetGameGrid();
        SolveBFS(new GridState(null, default, grid));
        return optimalMoves;
    }
}