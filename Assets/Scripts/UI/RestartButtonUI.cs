using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonUI : MonoBehaviour
{
    private void Start()
    {
        Button restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(GameManager.Instance.Restart);
    }
}
