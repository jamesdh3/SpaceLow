using UnityEngine;

public class follow_player_jdh : MonoBehaviour {

    public Transform player; 
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset; // first person view 
        Debug.Log(player.position); //print player position
    }
}
