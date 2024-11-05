using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridEditorUI : MonoBehaviour
{
    [SerializeField] private List<PropertyElementUI> properties;
    [SerializeField] private ColorsListUI colorsListUI;
    [SerializeField] private Button confirm;

    private void Start()
    {
        PuzzleBlockEditorUI.Instance.gameObject.SetActive(false);
        foreach(PropertyElementUI property in properties)
        {
            property.up.onClick.AddListener(() =>
            {
                property.SetValue(int.Parse(property.value.text)+1);
                UpdateGameProperties();
            });
            property.down.onClick.AddListener(() =>
            {
                property.SetValue(int.Parse(property.value.text) - 1);
                UpdateGameProperties();
            });
        }
        confirm.onClick.AddListener(() =>
        {
            int height = int.Parse(properties[0].value.text);
            int width = int.Parse(properties[1].value.text);
            int colors = int.Parse(properties[2].value.text);
            GridManager.Instance.ConfirmWorldGridSizes(width, height);
            colorsListUI.SetColorsCount(colors);
            PuzzleBlockEditorUI.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    private void UpdateGameProperties()
    {
        int height = int.Parse(properties[0].value.text);
        int width = int.Parse(properties[1].value.text);
        int colors = int.Parse(properties[2].value.text);
        GridManager.Instance.SetWorldGridSizes(width, height);
        colorsListUI.SetColorsCount(colors);
    }
}
