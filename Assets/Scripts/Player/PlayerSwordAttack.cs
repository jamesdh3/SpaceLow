﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    public int damageDealt = 3;
    public GameObject collisionExplosion;

    private GameObject player;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("isSlashing", true);
        }
        else
        {
            anim.SetBool("isSlashing", false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        EnemyHealth health = col.gameObject.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.TakeDamage(damageDealt);
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(explosion, 3f);
            Debug.Log("Sword is dealing damage");
        }
    }

    // Explosion Animation

    private void OnCollisionEnter(Collision collision) // however, here this requires trigger to be off, so it collides for explosion to play.
    {
        GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
        Destroy(explosion, 3f);
        Destroy(gameObject);

        return;
    }
}

