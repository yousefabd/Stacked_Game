using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FusionParticles : MonoBehaviour
{
    [SerializeField] private Transform fusionParticlesTemplate;
    [SerializeField] private Color fusionColor;
    private void Start()
    {
        PuzzleBlock puzzleBlock =transform.parent.GetComponent<PuzzleBlock>();
        puzzleBlock.OnFuse += PuzzleBlock_OnFuse;
    }
    private void PuzzleBlock_OnFuse()
    {
        for (int i = 0; i < 2; i++)
        {
            Transform fusionParticlesTransform = Instantiate(fusionParticlesTemplate, transform.position, Quaternion.identity);
            ParticleSystem ps = fusionParticlesTransform.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = fusionColor;
        }
    }
}
