using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrategySelectorUI : MonoBehaviour
{
    public static StrategySelectorUI Instance { get; private set; }
    private List<string> strategies = new List<string> { "BFS","DFS" };
    Dictionary<Toggle, string> toggleNames;
    [SerializeField] private Button confirmButton;
    private void Awake()
    {
        Instance = this;
        toggleNames = new Dictionary<Toggle, string>();
    }
    private void Start()
    {
        Transform strategyTemplate = transform.Find("StrategyTemplate");
        strategyTemplate.gameObject.SetActive(false);
        toggleNames[CreateStrategy("NONE", strategyTemplate, true)] = "NONE";
        foreach(var strategy in strategies)
        {
            toggleNames[CreateStrategy(strategy,strategyTemplate,false)] = strategy;
        }
        confirmButton.onClick.AddListener(() =>
        {
            StrategyManager.Instance.ConfirmStrategy();
        });
    }

    private Toggle CreateStrategy(string strategy,Transform strategyTemplate, bool isOn)
    {
        Transform strategyTransform = Instantiate(strategyTemplate, transform);
        strategyTransform.gameObject.SetActive(true);
        Toggle strategyToggle = strategyTransform.GetComponent<Toggle>();
        strategyToggle.isOn = isOn;
        strategyToggle.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = strategy;
        strategyToggle.onValueChanged.AddListener(isOn =>
        {
            if (!isOn)
            {
                StrategyManager.Instance.ClearStrategy();
                return;
            }
            string strategyName = toggleNames[strategyToggle];
            Toggle(strategyToggle);
            if (strategyName != "NONE")
                StrategyManager.Instance.SelectStrategy(toggleNames[strategyToggle]);
            else
                StrategyManager.Instance.ClearStrategy();
        });
        return strategyToggle;
    }
    private void Toggle(Toggle target)
    {
        foreach(Toggle toggle in toggleNames.Keys)
        {
            if (toggle.Equals(target))
                continue;
            toggle.isOn = false;
        }
    }
}
