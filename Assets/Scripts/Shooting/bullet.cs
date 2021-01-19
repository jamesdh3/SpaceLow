using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float DespawnTimer = 3f;
    private void Start()
    {
        Destroy(gameObject, DespawnTimer);
    }
}
