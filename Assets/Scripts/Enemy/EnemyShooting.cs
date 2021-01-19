using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    // I copied the Enemy Movement script and just added shooting on top of this, so can use this script for a shooting enemy and it will also handle the basic moving of the enemy towards the player.

    // Standard enemy movement
    public float speed = 5.0f;
    public float turnSpeed = 180.0f;
    public float gravity = 30.0f;
    private float yVelocity = 0f;
    private CharacterController controller;
    private Transform player;

    // Shooting portion of script.
    public float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject projectile;

    void Start()
    {
        // enemy movement
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player").transform;

        // enemy shooting
        timeBtwShots = startTimeBtwShots;
    }

    void Update()
    {
        // enemy movement
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        yVelocity -= gravity * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        controller.Move(transform.forward * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime);

        // enemy shooting
        if (timeBtwShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}