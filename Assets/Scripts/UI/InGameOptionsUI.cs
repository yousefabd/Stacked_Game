using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOptionsUI : MonoBehaviour
{
    public static InGameOptionsUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
