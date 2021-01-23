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

    public LayerMask _whatIsGround, _whatIsPlayer; 

    // Attacking variables 
    [SerializeField]
    private float _attackDelay;
    private bool _alreadyAttacked;

    // States AI can be in
    [SerializeField]
    private float _sightRange, _attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 
 
    [SerializeField] 
    Transform _destination; 


    private void OnDrawGizmosSelected()
    /** Aide in visualizing attack range and sight range of AI
    */
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange); 


    }
    // Start is called before the first frame update
    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); 
        _player = GameObject.FindWithTag("Player").transform;
        if (!_playerInSightRange && !_playerInAttackRange) 
        {
            Debug.Log("AI should be patroling");
        }
        if (_playerInSightRange && !_playerInAttackRange) 
        {
            Debug.Log("AI should be moving towards player");
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

    void Update() 
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

        if (!_playerInSightRange && !_playerInAttackRange) {
            Debug.Log("AI should be patrooooling");
        }
        if (_playerInSightRange && !_playerInAttackRange) {
            Debug.Log("AI should be chasing"); 
        }
        if (_playerInAttackRange && _playerInSightRange) {
            Debug.Log("AI should be attacking");
        }
    }

    void AttackPlayer() 
    /**
    */
    {

    }

    void ChasePlayer()
    /** When in sightrange, AI will move towards player
    */
    {

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
