using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 20.0f;
    public float timeBeforeDespawn = 5.0f;
    public int damageDealt = 3;
    public GameObject collisionExplosion;

    // Start is called before the first frame update
    void Start()
    {
        // Invoke calls the method named in its first param after the provided duration.
        Invoke("Kill", timeBeforeDespawn);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider col) // on Trigger is checked on the rocket, this is so it triggers the health damage
    {
        EnemyHealth health = col.gameObject.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.TakeDamage(damageDealt);
        }

        Kill();
    }

    void Kill()
    {
        Destroy(gameObject);
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
