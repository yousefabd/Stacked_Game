using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveStrategyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI activeStrategyText;

    private void Start()
    {
        StrategyManager.Instance.OnConfirmStrategy += StrategyManager_OnConfirmStrategy;
    }

    private void StrategyManager_OnConfirmStrategy(string strategy)
    {
        activeStrategyText.text = strategy;
        if (strategy == string.Empty)
            activeStrategyText.text = "Player";
    }
}
