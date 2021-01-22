// AIPatrol.cs

/**
Class: AIPatrol 

Methods: 
 - Start()
 - Update() 
 - SetDestination()
 - ChangePatrolPoint()

How it works; 
Set empty GameObjects that will serve as waypoints. agent will move from point A, 
And to whichever point is set as next. List of possible points shoulds already be set 
to desired order. AI will then move from point A to point B in a line.

Unity Requirements: 
 - Agents will need NavMeshAgent 
 - also this script if feature is desired 
 
Additional Features TBD: 
 - walk to nearest point 
 - walk to any neighboring points. neglecting distance among possible points  
 - walk in a non-linear or even better, random, path to next destination
 - agent turns randomly when idle (very low priority)
 - ability to move to previous patrol point 
*/

using UnityEngine;
using UnityEngine.AI; 
using System.Collections.Generic;

public class AIPatrol : MonoBehaviour
{
    // waiting variables
    [SerializeField] // NOTE: private variables to each object refering to this class 
    private bool _patrolWaiting;

    [SerializeField] // private variables to show in Unity editor
    private float _totalWaitTime;

    [SerializeField] // private variables to show in Unity editor
    private List<Waypoint> _patrolPoints; // reference to Waypoint class

    // base behaviors. no other scripts should need reference to 
    private NavMeshAgent _agent;
    private int _currentPatrolIndex; // index of PatrolPoints  
    private bool _traveling;
    private bool _waiting; 
    private bool _patrolForward;
    private float _waitTimer;


    // Start is called before the first frame update
    public void Start()
    /** 
        call to all objects needing partol features 
        start Patroling by looking for next destination point. must have > 1 points in scene 
    */
    {
        _agent = GetComponent<NavMeshAgent>(); // Requirements: Enemies will need this component 

        if (_agent == null) 
        { 
            // nav agent wouldn't be attached to game object
        }
        
        else
        {
            // basic requirement. 
            if (_patrolPoints != null && _patrolPoints.Count >= 2) 
            {
                _currentPatrolIndex=0; 
                SetDestination(); 
            }
            else // scene isn't set up for this patrol feature
            {
                // Not enough Gameobjects set in scene for Patroliing feature 
            }
        }  
    }

    // reminder Update is called once per frame. (NOTE: some patrol features may not be wanted here...?)
    void Update()
    /**
        Next possible actions: 
         - travel; wait; or look for next point 
    */
    {
        
        // agent must be moving. so check if agent is close to static destionation point 
        if (_traveling && _agent.remainingDistance <= 1.0f) // NOTE: adjustment here depending on AI behavior 
        {
            _traveling = false;

            // feature for waiting waiting before moving to the next point. idle animation would be needed 
            if (_patrolWaiting) 
            {
                _waiting=true; // NOTE : OR set idle 
                _waitTimer = 0f; 
            }
            // not waiting. look for next point to move to, then move to it 
            else 
            { 
                ChangePatrolPoint();
                SetDestination();
            }
        }

        // waiting. adjust wait timer 
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            // stop waiting and look for next destination point. then move to it 
            if (_waitTimer >= _totalWaitTime)
            { 
                _waiting = false; 

                ChangePatrolPoint(); 
                SetDestination(); 
            }
        }
    }

    private void SetDestination() 
    /**  base case: check for the numer of patrol/destination points 
    */
    {
        if (_patrolPoints != null) 
        {
            // get the coordinates of patrol point; set that as the destination; start traveling
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position; 
            _agent.SetDestination(targetVector); 
            _traveling = true; 
        }
    }

    private void ChangePatrolPoint() 
    /** Select new patrol point from list of possible points 
    */
    {
        // set next patrol point. need a check for end of list 
        if (_patrolForward) // NOTE: boolean here in case you want to move to mix it up and move to previous patrol point 
        {
            _currentPatrolIndex ++ ; 
            if (_currentPatrolIndex >= _patrolPoints.Count) 
            { 
                _currentPatrolIndex = 0; 
            }
        }

        else { 
            _currentPatrolIndex--;
            if (_currentPatrolIndex < 0)
            { 
                _currentPatrolIndex = _patrolPoints.Count -1;
            }
        }
    }
}
