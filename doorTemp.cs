using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTemp : MonoBehaviour
{
    [SerializeField] private GameObject doorMain;
    [SerializeField] private GameObject doorRight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorMain.transform.Translate(Vector3.left * Time.deltaTime);
            doorRight.transform.Translate(Vector3.right * 2 * Time.deltaTime);
        }
    }
}
