using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletforTurret : MonoBehaviour
{
    public float speed = 10f;
    public GameObject collisionExplosion;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed);
        Destroy(gameObject, 0.8f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 3f);
        }
    }


}
