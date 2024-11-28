using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Puzzle Block")]
public class PuzzleBlockSO : ScriptableObject
{
    public string stringName;
    public char charSymbol;
    public Transform prefab;
    public float movementDuration = 0.2f;
    public Sprite sprite;
    public bool IsBlock()
    {
        return charSymbol == '#';
    }
}
