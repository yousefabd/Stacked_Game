using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategyScreenUI : MonoBehaviour
{
    [SerializeField] private Button strategySelectButton;
    private void Start()
    {
        strategySelectButton.onClick.AddListener(() =>
        {
            Show();
        });
        StrategyManager.Instance.OnConfirmStrategy += StrategyManager_OnConfirmStrategy;
        Hide();
    }

    private void StrategyManager_OnConfirmStrategy(string obj)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive (true);
    }

    private void Hide()
    {
        gameObject.SetActive (false);
    }
}
