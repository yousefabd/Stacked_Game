using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlockEditorUI : MonoBehaviour
{
    public static PuzzleBlockEditorUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
