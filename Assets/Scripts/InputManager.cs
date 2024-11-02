using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public event Action<Vector2Int> OnMove;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        Vector2Int moveDir = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDir.y += 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDir.y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDir.x += 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDir.x -= 1;
        }
        if (moveDir != Vector2Int.zero)
        {
            OnMove?.Invoke(moveDir);
        }
    }
}
