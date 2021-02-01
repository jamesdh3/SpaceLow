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
    [SerializeField]
    private LayerMask _whatIsGround, _whatIsPlayer; 

    // Attacking variables 
    [SerializeField]
    private float _attackDelay;
    private bool _alreadyAttacked;
    private float _timeBtwShots; 

    // need instance of projectiles 
    [SerializeField]
    private GameObject _projectile;

    // turret variables 
    private Transform _turret; 
    [SerializeField]
    private int _turretMagMax;
    private int _turretMagCount;

    // States AI can be in
    [SerializeField]
    private float _sightRange, _attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 
 
    [SerializeField] 
    private Transform _destination;

    [SerializeField] 
    private bool _isRangeAI, _isTurretAI, _isFighterAI;

    /* Assign by dragging the GameObject with the other script you want into the inspector before running the game.
        Don't need public AIPatrol = new AIPatrol(); I think you had? Something like that.  You still want the same script.
        The script is still on the enemy, it's just split into a separate script to keep the code clean, as you intended.
        This is also why you were getting a null reference exception error (which you will see a lot) pointed at the Start(); function in the error message.
        I think it was trying to call Start(); but it didn't know what AIP.Start(); was referring to, hence being null.
    */
    public AIPatrol patrolController;
    //public AIDetect detectController;


    // Start is called before the first frame update
    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); 
        _player = GameObject.FindWithTag("Player").transform;
        _timeBtwShots = _attackDelay; 
        _turret = GameObject.FindWithTag("Turret").transform; 

        if (!_playerInSightRange && !_playerInAttackRange) 
        {
            // You're now telling that gameobject to call Start(); function within that script.
            patrolController.Start();
        }
        if (_playerInSightRange && !_playerInAttackRange) 
        {
            Debug.Log("AI should be chasing player"); 
            ChasePlayer();
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
        if (!_playerInSightRange && !_playerInAttackRange & !_isTurretAI) {
            patrolController.Update(); 
            Debug.Log("patroling");

        }
        // AI detects player and chases. Exceptions: turret 
        if (_playerInSightRange && !_playerInAttackRange & !_isTurretAI) {
            Debug.Log("Chasing player");
            ChasePlayer(); 
        }
        // AI attacks player 
        if (_playerInAttackRange && _playerInSightRange) {
            AttackPlayer(); 
        }
    }

    void AttackPlayer() 
    /** Main Attack function that will handle which attack animation an AI should do 
        possible attacks: 
         - turret attack (i.e _isTurretAI)
         - AI shoots (i.e _isRangeAI)
         - AI close combat attack (i.e _isFighterAI)
    */
    {
        // enforce only 1 can be selected at a time 
        if (_isRangeAI && !_isTurretAI && !_isFighterAI) {
            Debug.Log("Bang Bang!");
            AIShoot();
        }
        else if (_isTurretAI && !_isRangeAI && !_isFighterAI) { 
            Debug.Log("Turret go BRRRRrrrRRRR");
            TurretShoot(); 
        }
        else if (_isFighterAI && !_isTurretAI && !_isRangeAI) { 
            Debug.Log("Slash attack goes here");
        }
        else {
            Debug.Log("ERROR: unknown attack state");
        }
    }



    void TurretShoot() 
    /** TODO: get reloading feature to work 
        Known issues:
         - turret doesn't stop shooting when player is in range 
         - turret changes color in Level0...? 
    */
    {
        // look at player 
        transform.LookAt(_player); 
        _turretMagCount = _turretMagMax;

        // turret shoots without delay <X> times and reloads 
        if (_turretMagCount > 0) {
            Instantiate(_projectile, transform.position, Quaternion.identity); 
            _turretMagCount -= 1;
        }
        else if (_turretMagCount <= 0) {
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBtwShots); 
            _turretMagCount = _turretMagMax; 

        }
    }


    void AIShoot() 
    /** Method for typicaly AI enemies that can shoot projectiles in a straight line 

        possible Feature requests:
         - lob shot
         - grenade launcher method + lob shot   
    */
    {
         // stop movement 
        _agent.SetDestination(transform.position);
        transform.LookAt(_player); 
        // enemy shooting
        if (!_alreadyAttacked)
        {
            Instantiate(_projectile, transform.position, Quaternion.identity);
            //_timeBtwShots = _attackDelay;
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }
    }


    void ResetAttack() 
    {
        _alreadyAttacked = false; 
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

}
