using UnityEngine;
using UnityEngine.AI; 
using System.Collections.Generic;

public class AIPatrol : AIMove
{
    [SerializeField]
    private bool _patrolWaiting;
    [SerializeField]
    private float _totalWaitTime;

    [SerializeField]
    private List<Waypoint> _patrolPoints; // reference to Waypoint class

    private int _currentPatrolIndex;
    private bool _traveling;
    private bool _waiting; 
    private bool _patrolForward = true;
    private float _waitTimer;

    // Start is called before the first frame update
    public void Start()
    {
        //_agent = GetComponent<NavMeshAgent>(); Requirements: Enemies will need this component; must have > 1 points in scene 

        if (_agent == null) 
        { 
            // nav agent wouldn't be attached to game object
        }
        
        else
        {
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

    public void Update()
    {
        // check if agent is close to static destionation point 
        if (_traveling && _agent.remainingDistance <= 1.0f) // NOTE: adjustment here depending on AI behavior 
        {
            _traveling = false;

            if (_patrolWaiting) 
            {
                _waiting = true; // OR set idle 
                _waitTimer = 0f; 
            }
            else 
            { 
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (_waiting)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _totalWaitTime)
            { 
                _waiting = false; 

                ChangePatrolPoint(); 
                SetDestination(); 
            }
        }
    }

    public void SetDestination() 
    {
        if (_patrolPoints != null) 
        {
            // get the coordinates of patrol point; set that as the destination; start traveling
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position; 
            _agent.SetDestination(targetVector); 
            _traveling = true; 
        }
    }

    public void ChangePatrolPoint() 
    {
        Debug.Log("patrol point is...");
        Debug.Log(_currentPatrolIndex);
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
