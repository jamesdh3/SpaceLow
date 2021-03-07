using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField]
    private float _bulletDuration, _bulletSpeed;
    private float _lifeTimer;
    public GameObject collisionExplosion;

    private void Start()
    {
        _lifeTimer = _bulletDuration;
    }

    void Update() { 
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime; 

        // check if bullet should be destroyed 
        _lifeTimer -= Time.deltaTime; 
        if (_lifeTimer <= 0f) { 
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ground")
        {
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 2f);
        }
    }
}
