using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PropertyElementUI: MonoBehaviour
{
    private int minValue = 1;
    private int maxValue = 10;
    public TextMeshProUGUI value;
    public Button up;
    public Button down;
    
    public void SetValue(int newValue)
    {
        value.text = Mathf.Clamp(newValue,minValue, maxValue).ToString();
    }
}