using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridEditorUI : MonoBehaviour
{

    public static GridEditorUI Instance { get; private set; }
    [SerializeField] private List<PropertyElementUI> properties;
    [SerializeField] private Button confirm;

    public event Action OnConfirm;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
            ColorsListUI.SetColorsCount(colors);
            OnConfirm?.Invoke();
        });
    }

    private void UpdateGameProperties()
    {
        int height = int.Parse(properties[0].value.text);
        int width = int.Parse(properties[1].value.text);
        int colors = int.Parse(properties[2].value.text);
        GridManager.Instance.SetWorldGridSizes(width, height);
    }
}
