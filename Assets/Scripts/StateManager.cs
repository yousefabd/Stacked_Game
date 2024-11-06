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
            Debug.Log("The grid "+ previousState?.GetKey() + "is now "+ GetKey() + " after applying "+previousForceDir);
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

    private void Start()
    {
        visitedGrids = new Dictionary<string, bool>();
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;

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
        startGrid = GridState.GetGridAsChar(GridManager.Instance.GetGameGrid());
        //char[,] cGrid = GridState.GetGridAsChar(startGrid);
        //Debug.Log(new GridState(null,default,startGrid).GetKey());
        //SolveDFS(new GridState(null,default,startGrid));
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

}