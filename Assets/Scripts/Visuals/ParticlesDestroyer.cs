using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        float duration = main.duration;
        StartCoroutine(DestroyAfterPlay(duration));
    }
    private IEnumerator DestroyAfterPlay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
