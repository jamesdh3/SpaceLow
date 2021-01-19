using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform rocketBarrel;
    public float reloadTime = 0.5f;

    private float lastFireTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && Time.time > lastFireTime + reloadTime)
        {
            GameObject go = (GameObject)Instantiate(rocketPrefab, rocketBarrel.position, Quaternion.LookRotation(rocketBarrel.forward));
            Physics.IgnoreCollision(GetComponent<Collider>(), go.GetComponent<Collider>());
            lastFireTime = Time.time;
        }
    }
}
