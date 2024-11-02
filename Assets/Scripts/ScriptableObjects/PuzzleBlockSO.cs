using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Puzzle Block")]
public class PuzzleBlockSO : ScriptableObject
{
    public string stringName;
    public char charSymbol;
    public Transform prefab;
    public float blockSpeed;
    public bool IsBlock()
    {
        return charSymbol == '#';
    }
}
