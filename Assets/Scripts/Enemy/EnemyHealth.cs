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

/* This health script can be put on an "enemy" object or a turret, etc. but it must also have a Character Controller.
 * I thought just having a box collider on the object if it was something static such as turret, would work, but it doesn't,
 * not without the character controller as well. */