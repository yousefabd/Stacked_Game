using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public event Action<Vector2Int> OnMove;
    public event Action<Vector3> OnMouseCLicked;

    private float maxInputCooldown = 0.4f;
    private float currentInputCooldown;
    private bool canMove = true;
    private Camera mainCamera;
    private void Awake()
    {
        currentInputCooldown = maxInputCooldown;
        Instance = this;
    }
    private void Start()
    {
        mainCamera = Camera.main;
        StrategyManager.Instance.OnConfirmStrategy += StrategyManager_OnConfirmStrategy;
    }

    private void StrategyManager_OnConfirmStrategy(string strategy)
    {
        if(strategy != string.Empty)
            canMove = false;
    }

    void Update()
    {
        currentInputCooldown -= Time.deltaTime;
        Vector2Int moveDir = Vector2Int.zero;
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseCLicked?.Invoke(GetMouseWorldPosition());
        }
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
        else if (moveDir != Vector2Int.zero && canMove)
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
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        return worldPosition;
    }
}
