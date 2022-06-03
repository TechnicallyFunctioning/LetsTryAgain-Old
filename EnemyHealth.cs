using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxhealth;

    private float health;
    private void Start()
    {
        health = maxhealth;
    }
    private void Update()
    {
        if(health < .1)
        {
            Die();
        }
    }
    public void TakeDamage()
    {
        health -= gameManager.playerDamage;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
