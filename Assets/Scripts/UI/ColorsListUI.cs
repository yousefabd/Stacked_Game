using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorsListUI : MonoBehaviour
{
    [SerializeField] private List<PuzzleBlockSO> coloredBlocksList;
    private Dictionary<PuzzleBlockSO, Transform> puzzleBlockSOTransform;
    private void Start()
    {
        puzzleBlockSOTransform = new Dictionary<PuzzleBlockSO, Transform>();
        Transform buttonTemplate = transform.Find("ButtonTemplate");
        buttonTemplate.gameObject.SetActive(false);
        foreach (PuzzleBlockSO puzzleBlock in coloredBlocksList) 
        {
            Transform colorButtonTransform = Instantiate(buttonTemplate, transform);
            Button colorButton = colorButtonTransform.GetComponent<Button>();
            colorButton.onClick.AddListener(() =>
            {
                BlockEditorManager.Instance.SetPuzzleBlockSO(puzzleBlock);
                UpdateVisuals();
            });
            colorButton.transform.Find("sprite").GetComponent<Image>().sprite = puzzleBlock.sprite;
            colorButton.gameObject.SetActive(true);
            puzzleBlockSOTransform[puzzleBlock] = colorButtonTransform;
        }
        UpdateVisuals();
    }



    private void UpdateVisuals()
    {
        if (!BlockEditorManager.Instance.HasSelected())
        {
            Transform selected = puzzleBlockSOTransform[coloredBlocksList[0]].Find("selected");
            selected.gameObject.SetActive(true);
            BlockEditorManager.Instance.SetPuzzleBlockSO(coloredBlocksList[0]);
            return;
        }
        foreach(PuzzleBlockSO puzzleBlock in coloredBlocksList)
        {
            Transform selected = puzzleBlockSOTransform[puzzleBlock].Find("selected");
            selected.gameObject.SetActive(BlockEditorManager.Instance.IsSelected(puzzleBlock));
        }
    }
}
