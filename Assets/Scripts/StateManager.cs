using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private class GridState
    {
        public GridState previousState;
        public Vector2Int previousForceDir;
        public GameGrid currentGrid;
        public GridState(GridState previousState, Vector2Int previousForceDir, GameGrid previousGrid)
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

    private struct UCSNode
    {
        public GridState state;
        public int cost;
        public UCSNode(GridState state, int cost)
        {
            this.state = state;
            this.cost = cost;
        }
    }

    private struct HCNode
    {
        public GridState state;
        public int totalCost;
        public int moveCost;
        public HCNode(GridState state)
        {
            this.state=state;
            this.totalCost = 0;
            this.moveCost= this.state.currentGrid.GetCurrentBlocksCount();
        }
        public void SetCost(int cost)
        {
            this.totalCost = cost;
        }
    }
    public static StateManager Instance { get; private set; } 

    private char[,] startGrid;
    private Dictionary<string, bool> visitedGrids;
    private string currentStrategy = "";

    public event Action<List<Vector2Int>,bool> OnFindSolution;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        visitedGrids = new Dictionary<string, bool>();
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnRestart += GameManager_OnRestart;
        StrategyManager.Instance.OnConfirmStrategy += StrategyManager_OnConfirmStrategy;
    }

    private void StrategyManager_OnConfirmStrategy(string strategy)
    {
        currentStrategy = strategy;
    }

    private void GameManager_OnRestart()
    {
        if(startGrid != null)
        {
            GridManager.Instance.LoadGrid(startGrid);
        }
    }

    private void GameManager_OnGameStarted()
    {
        GameGrid grid = GridManager.Instance.GetGameGrid();
        startGrid = GridState.GetGridAsChar(grid);
        CalculateOptimalMoves(new GridState(null, default, grid));
        SolveAndMove(new GridState(null, default, grid));
    }
    private void CalculateOptimalMoves(GridState startState)
    {
        visitedGrids.Clear();
        List<Vector2Int> solution = SolveBFS(startState);
        OnFindSolution?.Invoke(solution,false);
    }
    private void SolveAndMove(GridState startState)
    {
        Type type = GetType();
        MethodInfo methodInfo = type.GetMethod("Solve" + currentStrategy, BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo == null)
        {
            return;
        }
        visitedGrids.Clear();
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        List<Vector2Int> solution = methodInfo.Invoke(this, new object[] {startState}) as List<Vector2Int>;
        stopwatch.Stop();
        Debug.Log("Solved using " + currentStrategy + " in: "+ stopwatch.ElapsedMilliseconds/1000f +"s");
        Debug.Log("Visited states: " + visitedGrids.Count);
        OnFindSolution?.Invoke(solution,true);

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

    private List<Vector2Int> SolveDFSR(GridState currentState)
    {
        List<GridState> adjacentStates = GetAdjacentStates(currentState);
        foreach (GridState adjacent in adjacentStates)
        {
            if (visitedGrids.ContainsKey(adjacent.GetKey()))
                continue;
            visitedGrids[adjacent.GetKey()] = true;
            if (adjacent.IsFinalState())
            {
                return RetractSolution(adjacent);
            }
            List<Vector2Int> directions = SolveDFSR(adjacent);
            if (directions.Count > 0)
                return directions;
;        }
        return new List<Vector2Int>();
    }
    private List<Vector2Int> SolveDFS(GridState startState)
    {
        Stack<GridState> stack = new Stack<GridState>();
        stack.Push(startState);

        while (stack.Count > 0)
        {
            GridState currentState = stack.Pop();
            if (visitedGrids.ContainsKey(currentState.GetKey()))
                continue;
            visitedGrids[currentState.GetKey()] = true;
            if (currentState.IsFinalState())
            {
                return RetractSolution(currentState);
            }
            List<GridState> adjacentStates = GetAdjacentStates(currentState);
            foreach(GridState adjacent in adjacentStates)
            {
                stack.Push(adjacent);
            }
        }
        return new List<Vector2Int>();
    } 
    private List<Vector2Int> SolveBFS(GridState startState)
    {
        visitedGrids[startState.GetKey()] = true;
        Queue<GridState> queue = new Queue<GridState>();
        queue.Enqueue(startState);
        while (queue.Count > 0)
        {
            GridState currentState = queue.Dequeue();
            if (currentState.IsFinalState())
            {
                return RetractSolution(currentState);
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
        return new List<Vector2Int>();
    }
    private List<Vector2Int> SolveUCS(GridState startState)
    {
        int[] movementCosts = new int [4]{ 1/*up*/, 1/*down*/, 2/*left*/, 3/*right*/ };
        Dictionary<GridState,int> distance = new Dictionary<GridState, int> ();
        PriorityQueue<UCSNode, int> p_queue = new PriorityQueue<UCSNode, int>(x => x.cost);

        distance[startState] = 0;
        visitedGrids[startState.GetKey()] = true;
        p_queue.Enqueue(new UCSNode(startState,0));
        while (!p_queue.Empty())
        {
            UCSNode currentNode = p_queue.Dequeue();
            if (distance[currentNode.state] < currentNode.cost)
                continue;
            if (currentNode.state.IsFinalState())
            {
                return RetractSolution(currentNode.state);
            }
            List<GridState> adjacencies = GetAdjacentStates(currentNode.state);
            for(int i=0;i<adjacencies.Count;i++)
            {
                int movementCost = movementCosts[i];
                int adjacentCost = 0;
                distance.TryGetValue(adjacencies[i], out adjacentCost);
                if (!visitedGrids.ContainsKey(adjacencies[i].GetKey())||
                    currentNode.cost+movementCost<adjacentCost) 
                {
                    int nextCost = currentNode.cost + movementCost;
                    distance[adjacencies[i]] = nextCost;
                    visitedGrids[adjacencies[i].GetKey()] = true;
                    p_queue.Enqueue(new UCSNode(adjacencies[i], nextCost));
                } 
            }
        }
        return new List<Vector2Int>();
    }
    private List<Vector2Int> SolveHC(GridState startState)
    {
        Dictionary<GridState, int> distance = new Dictionary<GridState, int>();
        PriorityQueue<HCNode, int> p_queue = new PriorityQueue<HCNode, int>(x => x.moveCost);

        distance[startState] = 0;
        visitedGrids[startState.GetKey()] = true;
        HCNode startNode = new HCNode(startState);
        startNode.moveCost = 0;
        p_queue.Enqueue(startNode);
        while (!p_queue.Empty())
        {
            HCNode currentNode = p_queue.Dequeue();
            if (distance[currentNode.state] < currentNode.moveCost)
                continue;
            if (currentNode.state.IsFinalState())
            {
                return RetractSolution(currentNode.state);
            }
            List<GridState> adjacencies = GetAdjacentStates(currentNode.state);
            foreach(GridState adjacent in adjacencies)
            {
                HCNode adjacentNode = new HCNode(adjacent);
                int adjacentCost = 0;
                distance.TryGetValue(adjacent, out adjacentCost);
                if (!visitedGrids.ContainsKey(adjacent.GetKey()) ||
                    currentNode.totalCost + adjacentNode.moveCost < adjacentCost)
                {
                    int nextCost = currentNode.totalCost + adjacentNode.moveCost;
                    distance[adjacent] = nextCost;
                    visitedGrids[adjacent.GetKey()] = true;
                    adjacentNode.SetCost(nextCost);
                    p_queue.Enqueue(adjacentNode);
                } 
            }
        }
        return new List<Vector2Int>();
    }
    private List<Vector2Int> RetractSolution(GridState finalState)
    {
        List<Vector2Int> solutionDirs = new List<Vector2Int>();
        GridState currentState = finalState;
        while (currentState != null)
        {
            if (currentState.previousForceDir.Equals(Vector2Int.zero)) { }
            else
                solutionDirs.Add(currentState.previousForceDir);
            currentState = currentState.previousState;
        }
        solutionDirs.Reverse();
        return solutionDirs;
    }
}