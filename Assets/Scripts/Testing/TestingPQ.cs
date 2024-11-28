using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPQ : MonoBehaviour
{
    PriorityQueue<int, int> pq;
    [SerializeField] private bool enable;
    private void Start()
    {
        enabled = enable;
        pq = new PriorityQueue<int, int>(x => x);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) {
            pq.Enqueue(Random.Range(0, 10));
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(!pq.Empty())
              Debug.Log(pq.Dequeue());
        }
    }
}
