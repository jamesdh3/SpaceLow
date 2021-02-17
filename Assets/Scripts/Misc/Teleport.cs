using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Teleport : MonoBehaviour 
{ 
    public Transform teleportTarget; 
    public GameObject player; 
    public Vector3 offset; // offset to apply to teleport destination to prevent endless loop
    //public bool _teleported; 

    void OnTriggerEnter(Collider col)
    {
        player.transform.position = teleportTarget.transform.position + offset;
    }
}