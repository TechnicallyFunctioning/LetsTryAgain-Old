using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Poker : MonoBehaviour
{
    [SerializeField] private InputActionReference activate;
    [SerializeField] private GameObject spike;
    [SerializeField] private float speed = 40;
    [SerializeField] private float fireCooldown = .5f;
    [SerializeField] private GameObject reticlePrefab;
    [SerializeField] LayerMask layerMaskIgnore;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject impactEffect2;
    [SerializeField] float damage = 1;

    private GameObject loadedSpike;
    private GameObject reticle;

    private Vector3 fwd;
    private RaycastHit hit;

    private EnemyHealth enemyHealth;
    private void Start()
    {
        loadedSpike = transform.Find("Spike").gameObject;
        loadedSpike.SetActive(true);

        activate.action.performed += Activate;

        reticle = Instantiate(reticlePrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, 50f, ~layerMaskIgnore))
        {
            reticle.transform.position = hit.point;
            reticle.transform.LookAt(transform.position, Vector3.up);
        }
    }

    private void Activate(InputAction.CallbackContext context)
    {
        if (loadedSpike.activeSelf)
        {
            if(hit.transform.gameObject.layer == 12)
            {

                enemyHealth = hit.transform.gameObject.GetComponent<EnemyHealth>();
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage();
                    Instantiate(impactEffect2, hit.point, Quaternion.identity);
                }
            }
            else
            {
                Instantiate(impactEffect, hit.point, Quaternion.identity);
            }
            reticle.SetActive(false);
            loadedSpike.SetActive(false);
            if (gameObject.activeSelf) { StartCoroutine(Cooldown()); }
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        loadedSpike.SetActive(true);
        reticle.SetActive(true);
    }
}
