using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class GridState : MonoBehaviour
{
    public char[,] startGrid;
    int i = 0;
    private void Start()
    {
        GridManager.Instance.OnGameStarted += GridManager_OnGameStarted;
    }

    private void GridManager_OnGameStarted()
    {
        GameGrid grid = GridManager.Instance.GetGameGrid();
        Vector2Int size = grid.GetAxis();
        startGrid = new char[size.x,size.y];
        for (int i=0; i<size.x; i++)
        {
            for(int j=0;j<size.y; j++)
            {
                startGrid[i, j] = grid.GetChar(i,j);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) {
            GridManager.Instance.LoadGrid(startGrid);
        }
    }
    private List<GridState> GetAdjacencies()
    {

        return default;
    }
}
