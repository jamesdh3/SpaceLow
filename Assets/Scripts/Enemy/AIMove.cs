// AIMove.cs
/** Wrapper that handles moving AI among different states. AI states:
 - attacking
 - patorling (i.e moving towards destination points)
 - chasing (i.e moving towards player)

Methods: 
 - Start() 
 - SetDestination()

How it works: 

Requiments: 
 - Agents will need NavMeshAgent
 - 
*/ 




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour
{

    // variables
    private NavMeshAgent _agent; 

    private Transform _player; 

    // each enemy needs to be assigned what these variables are 
    // should allow enemies to patrol on different objects 
    public LayerMask _whatIsGround, _whatIsPlayer; 

    // instance of different components that make up an AI 
    AIPatrol AIP = new AIPatrol(); 
    
    [SerializeField]
    List<Waypoint> _patrolPoints; 

    AIDetect AID = new AIDetect();

    //private static AIDetect AID;

    // Attacking variables 
    [SerializeField]
    private float _attackDelay;
    private bool _alreadyAttacked;

    // States AI can be in
    [SerializeField]
    private float _sightRange, _attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 
 
    [SerializeField] 
    private Transform _destination;

    // Assign by dragging the GameObject with the other script you want into the inspector before running the game.
    public AIPatrol objectWithTheScript;

    // Start is called before the first frame update
    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); 
        _player = GameObject.FindWithTag("Player").transform;

        if (!_playerInSightRange && !_playerInAttackRange) 
        {
            // AIP.Start(); Delete this, but leaving it for you to understand why it didn't work. You need a reference.

            // Here you're assigning the gameobject that has the script - now there's a referenced object.
            objectWithTheScript = GameObject.FindObjectOfType(typeof(AIPatrol)) as AIPatrol;

            // You're now telling that gameobject to call Start(); function with that script.
            objectWithTheScript.Start();
        }
        if (_playerInSightRange && !_playerInAttackRange) 
        {
            Debug.Log("AI should be chasing player"); 
        }
        if (_playerInAttackRange && _playerInSightRange) 
        {
            Debug.Log("AI should be attacking");
        }
        if (_agent == null) 
        {
            // navmesh component needed 
        }
        else 
        {
            SetDestination(); 
        }
    }

    public void Update() 
    /** continually look for possible states AI. NOTE: could be refactored to its'
        own method. 
        possible states: 
         - patrol
         - chase
         - attack
    */

    {
        // Check for sight and attack range 
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);
        // AI patrols  
        if (!_playerInSightRange && !_playerInAttackRange) {
            AIP.Update();
            Debug.Log("patroling");

        }
        // AI detects player 
        if (_playerInSightRange && !_playerInAttackRange) {
            Debug.Log("Chasing player");
            //AID.ChasePlayer();  
        }
        // AI attacks player 
        if (_playerInAttackRange && _playerInSightRange) {
            AttackPlayer(); 
        }
    }

    void AttackPlayer() 
    /**
    */
    {
        // stop movement 
        _agent.SetDestination(transform.position);

        transform.LookAt(_player); 

        if (!_alreadyAttacked)
        {
            // shoot or slash enemy slash AI
            Debug.Log("BANG! BANG!");

            _alreadyAttacked = true; 
            Invoke(nameof(ResetAttack), _attackDelay); 
        }
    }


    private void ResetAttack() 
    {
        _alreadyAttacked = false; 
    }


    private void SetDestination() 
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position; 
            _agent.SetDestination(targetVector); 
        }
    }

}
