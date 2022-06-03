using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private GameObject splat;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (rb.velocity.magnitude > 15)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, fwd, out hit, 20))
            {
                Instantiate(splat, hit.point, Quaternion.identity);
                Destroy(gameObject);

                if(hit.transform.gameObject.layer == 12)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    public void Launched()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
     {
         if(other.gameObject.layer != 10)
         {
             Instantiate(splat, transform.position, Quaternion.identity);
             Destroy(gameObject);
         }
     }
}
