using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollerBot : MonoBehaviour
{
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderTimer;
    [SerializeField] private float sphereRotationSpeed;
    [SerializeField] private float reverseSpeed;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifespan;
    [SerializeField] private float shootCooldown;

    private GameObject player;
    private GameObject playerHead;
    private GameObject barrel;
    private GameObject cannon;
    private GameObject sphere;
    private NavMeshAgent agent;
    private float timer;
    private bool wander = true;
    private float playerDistance;
    private bool canShoot;

    private void Start()
    {
        player = GameObject.Find("XR Rig");
        playerHead = Camera.main.gameObject;
        cannon = transform.Find("Cannon").gameObject;
        barrel = cannon.transform.Find("Barrel").gameObject;
        sphere = transform.Find("Sphere").gameObject;
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        canShoot = true;
    }
    void Update()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        print(playerDistance);
        timer += Time.deltaTime;

        if (wander) { Wander(); }

        if (playerDistance < 30) { Combat(); wander = false; } else { wander = true; }



        sphere.transform.Rotate(sphereRotationSpeed * agent.velocity.magnitude * Time.deltaTime,0, 0);
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void Wander()
    {
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    private void Shoot()
    {
        GameObject currentProjectile = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
        currentProjectile.GetComponent<Rigidbody>().AddForce(Vector3.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(currentProjectile, projectileLifespan);
        canShoot = false;
        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void Combat()
    {
        cannon.transform.LookAt(playerHead.transform);

        if (playerDistance < 25 && playerDistance > 10)
        {
            agent.destination = (player.transform.position);
            if (canShoot)
            {
                Shoot();
            }
        }
        if (playerDistance < 10)
        {
            agent.Move((transform.position - player.transform.position).normalized * reverseSpeed * Time.deltaTime);
            sphere.transform.Rotate(sphereRotationSpeed * -agent.velocity.magnitude * Time.deltaTime, 0, 0);
        }
    }
}
