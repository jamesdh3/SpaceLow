// Agents will need Navmesh Agent

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour
{
    public NavMeshAgent _agent; 
    public Transform _player; 

    [SerializeField]
    private LayerMask _whatIsGround, _whatIsPlayer; 

    [SerializeField]
    private float _sightRange, _attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 
 
    [SerializeField] 
    private Transform _destination;
    public bool _isRangeAI, _isTurretAI, _isFighterAI; 
    
    // for flying enemies 
    private float _fly_height; 

    public AIPatrol patrolController;
    public AIAttack attackController;

    // Start is called before the first frame update
    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); 
        _player = GameObject.FindWithTag("Player").transform;
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        if (_isRangeAI)
        { 
            _fly_height = GetComponent<NavMeshAgent>().baseOffset;
            _agent.SetDestination(Vector3.up * _fly_height);
        }

        if (!_playerInSightRange && !_playerInAttackRange) 
        {
            patrolController.Start();
        }

        if (_playerInSightRange && !_playerInAttackRange) 
        {
            ChasePlayer();
        }

        if (_playerInAttackRange && _playerInSightRange) 
        {
            attackController.AttackPlayer();
        }
        else 
        {
            SetDestination(); 
        }
    }

    public void Update() 
    {
        // These vars are needed to be here in order for AI Shoot to work with flying enemy, figure out why, then DRY.
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        // AI patrols  
        if (!_playerInSightRange && !_playerInAttackRange & !_isTurretAI) {
            patrolController.Update(); 

        }
        // AI detects player and chases. Exceptions: turret 
        if (_playerInSightRange && !_playerInAttackRange & !_isTurretAI) {
            ChasePlayer(); // should be refactored to its own class
        }
        // AI attacks player 
        if (_playerInAttackRange && _playerInSightRange && _player != null) { 
            attackController.AttackPlayer();
        }
    }

    void ChasePlayer()
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
