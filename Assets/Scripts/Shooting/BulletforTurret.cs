using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletforTurret : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector3 target;

    public GameObject collisionExplosion;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // I added +500 to the Z so that the bullets will pass the target and not stop directly at it, and then will be destroyed after 3f seconds of being spawned.
        target = new Vector3(player.position.x, player.position.y + 3, player.position.z + 500);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            DestroyProjectile();
        }


        /* if (transform.position.x == target.x && transform.position.y == target.y && transform.position.z == target.z && target != null)
         {
             GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
             Destroy(explosion, 3f);
             DestroyProjectile();
         }
        */
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && target != null)
        {
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(explosion, 3f);
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject, 3f);
    }

}
