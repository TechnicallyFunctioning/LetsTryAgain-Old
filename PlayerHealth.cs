using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float playerhealth = 100;
    private float health;

    private float damage = 25;

    private void Start()
    {
        health = playerhealth;
    }
    private void Update()
    {
        if (health < .1f)
        {
            Die();
        }
    }
    private void TakeDamage()
    {
        health -= damage;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
