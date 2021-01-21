/**
Set static points; have mesagent move between these 2 points in a linear line;
 feature to back back to a previous point
*/
using UnityEngine;
using UnityEngine.AI; 


public class EnemyPatrol : MonoBehaviour
{
    // waiting variables
    [SerializeField] // private variables to show in Unity editor
    private bool patrolWaiting;

    [SerializeField]
    private float totalWaitTime;

    [SerializeField] 
    private List<Waypoint> patrolPoints; 


    // base behaviors. no other scripts should need reference to 
    private List<Waypoint> patrolPoints; 
    private NavMeshAgent agent;
    private int currentPatrol;
    private bool traveling;
    private bool waiting; 
    private bool patrolForward;
    private float waitTimer;

    public Transform moveSpot;
    private int randomSpot; 


    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NashMeshAgent>();

        if (navMeshAgent == null) 
        { 
            // nav agent wouldn't be attached to game object
        }
        
        else
        {
            if ()

        }
        waitTime = startWaitTime;
        randomSpot = Random.Range(0,moveSpots.Length);    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[tandomSpot].position, speed*Time.deltaTime)
    }
}
