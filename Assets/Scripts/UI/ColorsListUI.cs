using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ColorsListUI : MonoBehaviour
{
    [SerializeField] private List<PuzzleBlockSO> coloredBlocksList;
    private Dictionary<PuzzleBlockSO, Transform> puzzleBlockSOTransform;
    private static event Action OnSelectPuzzleBlock;
    private int colorsCount = 1;
    private void Start()
    {
        OnSelectPuzzleBlock += UpdateVisuals;
        puzzleBlockSOTransform = new Dictionary<PuzzleBlockSO, Transform>();
        Transform buttonTemplate = transform.Find("ButtonTemplate");
        buttonTemplate.gameObject.SetActive(false);
        int index = 0;
        foreach (PuzzleBlockSO puzzleBlock in coloredBlocksList) 
        {
            if (index >= colorsCount)
                break;
            index++;
            Transform colorButtonTransform = Instantiate(buttonTemplate, transform);
            Button colorButton = colorButtonTransform.GetComponent<Button>();
            colorButton.onClick.AddListener(() =>
            {
                BlockEditorManager.Instance.SetPuzzleBlockSO(puzzleBlock);
                OnSelectPuzzleBlock?.Invoke();
            });
            colorButton.transform.Find("sprite").GetComponent<Image>().sprite = puzzleBlock.sprite;
            colorButton.gameObject.SetActive(true);
            puzzleBlockSOTransform[puzzleBlock] = colorButtonTransform;
        }
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        int index = 0;
        foreach(PuzzleBlockSO puzzleBlock in coloredBlocksList)
        {
            if (index >= colorsCount)
                break;
            index++;
            Transform selected = puzzleBlockSOTransform[puzzleBlock].Find("selected");
            selected.gameObject.SetActive(BlockEditorManager.Instance.IsSelected(puzzleBlock));
        }
    }

    public int GetMaxColorsAllowed()
    {
        return coloredBlocksList.Count;
    }
    public void SetColorsCount(int count)
    {
        colorsCount = count;
    }
}
