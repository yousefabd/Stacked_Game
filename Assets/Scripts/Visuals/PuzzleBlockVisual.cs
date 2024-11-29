using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleBlockVisual : MonoBehaviour
{
    [SerializeField] private Transform fusionParticlesTemplate;
    [SerializeField] private Color fusionColor;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PuzzleBlock puzzleBlock =transform.parent.GetComponent<PuzzleBlock>();
        puzzleBlock.OnFuse += PuzzleBlock_OnFuse;
        puzzleBlock.OnFusedInto += PuzzleBlock_OnFusedInto;
    }

    private void PuzzleBlock_OnFusedInto()
    {
        animator.Play("FuseBlock");
    }

    private void PuzzleBlock_OnFuse()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform fusionParticlesTransform = Instantiate(fusionParticlesTemplate, transform.position, Quaternion.identity);
            ParticleSystem ps = fusionParticlesTransform.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = fusionColor;
        }
    }
}
