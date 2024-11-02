using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public event Action<Vector2Int> OnMove;

    private float maxInputCooldown = 0.2f;
    private float currentInputCooldown;
    private void Awake()
    {
        currentInputCooldown = maxInputCooldown;
        Instance = this;
    }
    void Update()
    {
        currentInputCooldown -= Time.deltaTime;
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
        if (currentInputCooldown > 0) { }
        else if (moveDir != Vector2Int.zero)
        {
            currentInputCooldown = maxInputCooldown;
            if (moveDir.y != 0 && moveDir.x != 0)
            {
                moveDir.x = 0;
                OnMove?.Invoke(moveDir);
            }
            else
            {
                OnMove?.Invoke(moveDir);
            }
        }
    }
}
