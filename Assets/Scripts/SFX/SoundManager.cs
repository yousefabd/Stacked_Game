using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] movementSounds;
    [SerializeField] private AudioClip youWin;

    private int currentMovementSoundIndex = 0;


    private void Start()
    {
        GridManager.Instance.OnMakeMove += GridManager_OnMakeMove;
        GameManager.Instance.OnRestart += GameManager_OnRestart;
        GridManager.Instance.OnGameOver += GridManager_OnGameOver;
    }

    private void GridManager_OnGameOver()
    {
        StartCoroutine(PlaySound(youWin, transform.position,0.7f, 1f));
    }

    private void GameManager_OnRestart()
    {
        currentMovementSoundIndex = 0;
    }

    private void GridManager_OnMakeMove(Vector3 position)
    {
        AudioClip audioClip = movementSounds[currentMovementSoundIndex % movementSounds.Length];
        currentMovementSoundIndex++;
        PlaySound(audioClip, position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    private IEnumerator PlaySound(AudioClip audioClip, Vector3 position, float delay, float volume = 1f)
    {
        yield return new WaitForSecondsRealtime(delay);
        PlaySound(audioClip, position, volume);
    }
}
