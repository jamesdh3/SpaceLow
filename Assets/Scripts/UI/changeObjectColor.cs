using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeObjectColor : MonoBehaviour
{
    private Vector3 distance;
    private float distanceFrom;
 
    private Transform player;
    public Transform turret;
    private Renderer rend;

    private Color red = Color.red;
    private Color black = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        turret = GameObject.FindWithTag("TurretColor").transform;
        rend = turret.GetComponent<Renderer>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // turret
        distance = (turret.position - player.position);
        distance.y = 0;
        distanceFrom = distance.magnitude;
        distance /= distanceFrom;


        if (distanceFrom <= 42.23f)
        {
            rend.material.color = red;

        }
        else
        {
            rend.material.color = black;

        }
    }
}
