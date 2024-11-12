using System;
using UnityEngine;

public class StrategyManager : MonoBehaviour
{
    public static StrategyManager Instance { get; private set; }
    private string selectedStrategy = "";

    public event Action<string> OnConfirmStrategy;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectStrategy(string strategy)
    {
        selectedStrategy = strategy;
    }
    public void ClearStrategy()
    {
        selectedStrategy = "";
    }

    public void ConfirmStrategy()
    {
        OnConfirmStrategy?.Invoke(selectedStrategy);
    }
}
