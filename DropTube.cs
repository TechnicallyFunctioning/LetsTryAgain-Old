using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTube : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private float startTime = 2;
    [SerializeField] private float repeatTime = 2;

    void Start()
    {
        InvokeRepeating("SpawnRandomObject", startTime, repeatTime);
    }

    private void SpawnRandomObject()
    {
        Instantiate(items[Random.Range(0, items.Length)], transform.position, transform.rotation);
    }
}
