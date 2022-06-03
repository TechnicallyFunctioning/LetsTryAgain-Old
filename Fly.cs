using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fly : MonoBehaviour
{
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderTimer;
    [SerializeField] private bool bobSwitch;
    [SerializeField] private float startYposition;
    [SerializeField] private float upFrequency;
    [SerializeField] private float upIntensity;
    [SerializeField] private float bobSwitchTime;

    private NavMeshAgent agent;
    private float timer;
    private bool bob = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        if (bobSwitch)
        {
            StartCoroutine(BobSwitch());
        }
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        if (bob)
        {
            agent.baseOffset = startYposition + Mathf.Cos(Time.time * upFrequency) * upIntensity;
        }

    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }


    private IEnumerator BobSwitch()
    {
        while (true)
        {
            bob = true;
            yield return new WaitForSeconds(bobSwitchTime);
            bob = false;
            yield return new WaitForSeconds(bobSwitchTime);
        }
    }
}
