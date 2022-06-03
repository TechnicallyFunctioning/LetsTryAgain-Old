using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speedMin = .1f;
    public float speedMax = 1;

    public float changeTimeMin = 1f;
    public float changeTimeMax = 3f;

    private float speed;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin , speedMax);

        if(Random.Range(0 , 10) < 6) { direction = Vector3.forward; } else { direction = Vector3.back; }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction * speed);
        StartCoroutine(ChangeDirection());
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(changeTimeMin, changeTimeMax));

        speed = Random.Range(speedMin, speedMax);

        if (Random.Range(0, 10) < 6) { direction = Vector3.forward; } else { direction = Vector3.back; }
    }
}
