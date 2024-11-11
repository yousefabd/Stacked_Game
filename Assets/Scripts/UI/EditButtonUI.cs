using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditButtonUI : MonoBehaviour
{
    private void Start()
    {
        Button editButton = GetComponent<Button>();
        editButton.onClick.AddListener(GameManager.Instance.Edit);
    }
}
