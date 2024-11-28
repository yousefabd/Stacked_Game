using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    private float bounceHeight = 0.3f;
    private float bounceDuration = 0.3f;
    private int BounceCount = 2;

    private enum BouncingState
    {
        BOUNCING, IDLE
    }
    private BouncingState currentBouncingState;
    private Vector2 currentBounceDir;
    private float currentBounceTime;
    private int currentBounceCount;
    private float currentBounceHeight;
    private float currentBounceDuration;

    private void Start()
    {
        PuzzleBlock puzzleBlock = targetObject.GetComponent<PuzzleBlock>();
        puzzleBlock.OnSetMoveDir += PuzzleBlock_OnSetMoveDir;
        puzzleBlock.OnReachedDestination += PuzzleBlock_OnReachedDestination;

        currentBounceTime = 0f;
        currentBounceCount = 0;
        currentBounceHeight = bounceHeight;
        currentBounceDuration = bounceDuration;
        currentBouncingState = BouncingState.IDLE;
    }

    private void Update()
    {
        switch(currentBouncingState)
        {
            case BouncingState.IDLE:
                break;
            case BouncingState.BOUNCING:
                if(currentBounceCount >= BounceCount)
                {
                    currentBounceTime = 0f;
                    currentBounceCount = 0;
                    currentBounceDuration = bounceDuration;
                    currentBounceHeight = bounceHeight;
                    currentBouncingState = BouncingState.IDLE;
                    transform.position = Vector2.zero;
                }
                float offsetValue = Mathf.Sin((currentBounceTime / bounceDuration) * Mathf.PI) * currentBounceHeight;
                transform.localPosition = new Vector2(offsetValue * currentBounceDir.x, offsetValue * currentBounceDir.y);
                currentBounceTime += Time.deltaTime;
                if(currentBounceTime >= bounceDuration)
                {
                    currentBounceCount++;
                    currentBounceTime = 0f;
                    currentBounceHeight /= 3f;
                    currentBounceDuration /= 3f;
                }
                break;
        }
    }

    private void PuzzleBlock_OnReachedDestination()
    {
        currentBouncingState = BouncingState.BOUNCING;
        currentBounceTime = 0f;
        currentBounceCount = 0;
        currentBounceHeight = bounceHeight;
        currentBounceDuration = bounceDuration;
    }

    private void PuzzleBlock_OnSetMoveDir(Vector2 moveDir)
    {
        currentBounceDir = -1 * moveDir;
    }
}
