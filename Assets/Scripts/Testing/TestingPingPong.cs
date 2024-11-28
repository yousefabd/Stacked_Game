using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPingPong : MonoBehaviour
{
    [Serializable]
    private enum BouncingState
    {
        BOUNCING, IDLE
    }
    [SerializeField] private Transform testObject;
    [SerializeField] private float BounceHeight;
    [SerializeField] private float BounceDuration;
    [SerializeField] private int BounceCount;
    [SerializeField] private BouncingState currentBouncingState;
    [SerializeField] private Vector3 MoveDir;

    private float currentBounceTime;
    private int currentBounceCount;
    private float currentBounceHeight;
    private float currentBounceDuration;
    private Vector3 originPosition;

    private void Start()
    {
        currentBounceTime = 0f;
        currentBounceCount = 0;
        currentBounceHeight = BounceHeight;
        currentBounceDuration = BounceDuration;
        currentBouncingState = BouncingState.BOUNCING;
        originPosition = testObject.transform.position;
    }

    private void Update()
    {
        switch (currentBouncingState)
        {
            case BouncingState.IDLE:
                break;
            case BouncingState.BOUNCING:
                if (currentBounceCount >= BounceCount)
                {
                    currentBounceTime = 0f;
                    currentBounceCount = 0;
                    currentBounceDuration = BounceDuration;
                    currentBounceHeight = BounceHeight;
                    currentBouncingState = BouncingState.IDLE;
                    testObject.transform.position = originPosition;
                }
                float offsetValue = Mathf.Sin((currentBounceTime / BounceDuration) * Mathf.PI) * currentBounceHeight;
                testObject.position = new Vector3(originPosition.x +(offsetValue*MoveDir.x), originPosition.y + (offsetValue*MoveDir.y));
                currentBounceTime += Time.deltaTime;
                if(currentBounceTime >= BounceDuration)
                {
                    currentBounceCount++;
                    currentBounceTime = 0f;
                    currentBounceHeight /= 3f;
                    currentBounceDuration /= 3f;
                }
                break;
        }

    }
}
