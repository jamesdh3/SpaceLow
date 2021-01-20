using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // I copied the Enemy Movement script and just added shooting on top of this, so can use this script for a shooting enemy and it will also handle the basic moving of the enemy towards the player.


    private Transform player;
    
    // turret
    private Transform turret;
    private Vector3 distance;
    private float distanceFrom;
    private bool isAttacking = false;
    private float fireRate = 0.5f;
    private float nextFire = 0f;

    public GameObject projectile;

    void Start()
    {
  
        player = GameObject.FindWithTag("Player").transform;

        // turret
        turret = GameObject.FindWithTag("Turret").transform;
    }

    void Update()
    {

        // turret
        distance = (turret.position - player.position);
        distance.y = 0;
        distanceFrom = distance.magnitude;
        distance /= distanceFrom;

        Attacking();

        if (distanceFrom < 40f)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        void Attacking()
        {
            if (isAttacking)
            {

                // The enemy isn't blind so it should face the player
               
                

                //Shoot

                if (Time.time > nextFire)
                {

                    nextFire = Time.time + fireRate;
                    transform.position += Time.deltaTime * transform.forward;
                    Instantiate(projectile, transform.position, Quaternion.identity);
                    
                }
            }
        }


    }
}