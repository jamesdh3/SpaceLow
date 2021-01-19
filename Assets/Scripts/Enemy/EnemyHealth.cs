using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int initialHealth = 8;
    private int currentHealth;

    void Start()
    {
        currentHealth = initialHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}