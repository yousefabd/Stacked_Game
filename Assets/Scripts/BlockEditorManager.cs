using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEditorManager : MonoBehaviour
{
    public static BlockEditorManager Instance { get; private set; }
    [SerializeField] private List<PuzzleBlockSO> puzzleBlocks;
    private PuzzleBlockSO selectedPuzzleBlock;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InputManager.Instance.OnMouseCLicked += InputManager_OnMouseCLicked;
    }

    private void InputManager_OnMouseCLicked(Vector3 worldPosition)
    {
        if (selectedPuzzleBlock != null)
        {
            Vector2Int gridPosition = GridManager.Instance.WorldToGridPosition(worldPosition);
            GridManager.Instance.CreateBlock(selectedPuzzleBlock, gridPosition);
        }
    }

    public void SetPuzzleBlockSO(PuzzleBlockSO puzzleBlock)
    {
        selectedPuzzleBlock = puzzleBlock;
    }

    public bool IsSelected(PuzzleBlockSO puzzleBlock)
    {
        return puzzleBlock == selectedPuzzleBlock;
    }

    public bool HasSelected()
    {
        Debug.Log(selectedPuzzleBlock);
        return selectedPuzzleBlock != null; 
    }
}
