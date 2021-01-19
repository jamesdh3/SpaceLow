using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed = 180.0f;
    public float gravity = 30.0f;
    private float yVelocity = 0f;

    private CharacterController controller;
    private Transform player;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        yVelocity -= gravity * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        controller.Move(transform.forward * speed * Time.deltaTime + yVelocity * Vector3.up * Time.deltaTime);
    }
}
