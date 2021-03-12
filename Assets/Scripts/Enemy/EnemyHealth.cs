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

// This needs a character controller to work.  Enemies will not take damage without one.
// Make the character controller circle BIGGER than the box collider.  The CC will by the one that connects the damage function
// the box collider will be for not running through the enemy.

// CC Collides against objects, ONLY in the direction it's currently moving. So if you have a moving enemy it needs a box collider too,
// but make sure box collider is smaller than the CC.