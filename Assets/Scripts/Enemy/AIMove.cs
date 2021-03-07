/** Wrapper that handles moving AI among different states. AI states:
 - attacking
 - patroling (i.e moving towards destination points)
 - chasing (i.e moving towards player)

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
    public NavMeshAgent _agent; 
    public Transform _player; 

    // allow enemies to patrol on different objects 
    [SerializeField]
    private LayerMask _whatIsGround, _whatIsPlayer; 

    // States AI can be in
    [SerializeField]
    private float _sightRange, _attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 
 
    [SerializeField] 
    private Transform _destination;

    public bool _isRangeAI, _isTurretAI, _isFighterAI; 
    
    // variable for floating eye enemy 
    private float _fly_height; 

    public AIPatrol patrolController;
    public AIAttack attackController;

    // Start is called before the first frame update
    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); 
        _player = GameObject.FindWithTag("Player").transform;

        // Check for sight and attack range 
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);


        if (_isRangeAI) { 
            _fly_height = GetComponent<NavMeshAgent>().baseOffset;
            _agent.SetDestination(Vector3.up*_fly_height);
        }
        if (!_playerInSightRange && !_playerInAttackRange) 
        {
            patrolController.Start();
        }
        if (_playerInSightRange && !_playerInAttackRange) 
        {
            //Debug.Log("AI should be chasing player"); 
            ChasePlayer();
        }
        if (_playerInAttackRange && _playerInSightRange) 
        {
            //Debug.Log("AI should be attacking");
            attackController.AttackPlayer();
        }
        else 
        {
            SetDestination(); 
        }
    }

    public void Update() 
    {
        // Check for sight and attack range 
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        // AI patrols  
        if (!_playerInSightRange && !_playerInAttackRange & !_isTurretAI) {
            patrolController.Update(); 

        }
        // AI detects player and chases. Exceptions: turret 
        if (_playerInSightRange && !_playerInAttackRange & !_isTurretAI) {
            //Debug.Log("Chasing player");
            ChasePlayer(); // should be refactored to its own class once this feature needs to be further develoepd 
        }
        // AI attacks player 
        if (_playerInAttackRange && _playerInSightRange && _player != null) { 
            //Debug.Log("TURRET SHOOT");
            attackController.AttackPlayer();
        }
    }

    void ChasePlayer()
    // When in sightrange, AI will move towards player
    {
        Vector3 targetPlayer = _player.transform.position; 
        _agent.SetDestination(targetPlayer); 
    }

    void SetDestination() 
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position; 
            _agent.SetDestination(targetVector); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange); 
    }
}
