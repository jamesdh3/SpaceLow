// AIMove.cs
/** Wrapper that handles moving AI among different states. AI states:
 - attacking
 - patorling (i.e moving towards destination points)
 - chasing (i.e moving towards player)

Methods: 
 - Start() 
 - SetDestination()
 - ChasePlayer()

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
    public NavMeshAgent _agent; 
    public Transform _player; 
    //public NavMeshAgent _turret; 

    // each enemy needs to be assigned what these variables are 
    // should allow enemies to patrol on different objects 
    [SerializeField]
    private LayerMask _whatIsGround, _whatIsPlayer; 

    // States AI can be in
    [SerializeField]
    private float _sightRange, _attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 
 
    [SerializeField] 
    private Transform _destination;

    [SerializeField] 
    public bool _isRangeAI, _isTurretAI, _isFighterAI;

    //[SerializeField]
    //private float health; 

    /* class features 
    */
    public AIPatrol patrolController;
    public AIAttack attackController;


    //public AIDetect detectController;


    // Start is called before the first frame update
    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); 
        _player = GameObject.FindWithTag("Player").transform;
        // Check for sight and attack range 
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);
        //_turret = GetComponent<NavMeshAgent>(); 

        if (!_playerInSightRange && !_playerInAttackRange) 
        {
            // You're now telling that gameobject to call Start(); function within that script.
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
    /** continually look for possible states AI. NOTE: could be refactored to its'
        own method. SHould just check what enemy, and wh
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
            attackController.AttackPlayer(); //AttackPlayer(); 
        }
    }

    void ChasePlayer()
    /** When in sightrange, AI will move towards player
    */
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
    /** Aide in visualizing attack range and sight range of AI
    */
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange); 


    }

    /*
    public void TakeDamage(int damage) 
    {
        health -= damage; 

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy() { 
        Destroy(gameObject); 
    }

    void OnCollisionEnter(Collision collision) { 
        if (collision.gameObject.tag == "PlayerSword") { 
            Debug.Log("YOU HURT THE ENEMY");
            TakeDamage(1); 
        }
    }
    */
}
