﻿// Agents will need Navmesh Agent to move

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAi : MonoBehaviour
{

    // AI MOVE ***********************

    public NavMeshAgent _agentIfNPCMoves;
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    private LayerMask _selectTargetLayer;
    [SerializeField]
    private float _sightRange, _attackRange;
    private bool _targetInSightRange, _targetInAttackRange;
    [SerializeField]
    private Transform _moveToThisDestination;
    public bool _isRangeAI, _isTurretAI, _isFighterAI;
    // for flying enemies 
    private float _fly_height;

    // AI PATROL ***********************

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

    // AI ATTACK ***********************

    [SerializeField]
    private float _attackDelay;
    private bool _alreadyAttacked;
    [SerializeField]
    private GameObject _projectile;
    private bool _isReloading = false;
    private Vector3 yOffset = new Vector3(0, 3, 0);
    [SerializeField]
    private int _turretReloadTime;
    [SerializeField]
    private int _turretMagMax;
    private int _turretMagCount;
    [SerializeField]
    private Transform turretBarrel;
    public Transform turretEmptyTip;

    // GENERAL SOUND ***********************
    public AudioSource audioSrc;
    public AudioClip tracerSound, turretReloadSound;

    public void Awake()
    {
        _turretMagCount = _turretMagMax;
    }

    public void Start()
    {
        _agentIfNPCMoves = GetComponent<NavMeshAgent>();
        _isReloading = false;
        audioSrc = GetComponent<AudioSource>();
        checkRangeStatus();
        checkForPatrolPoints();
    }

    public void Update()
    {
        SetTargetRanges();

        // AI patrols  
        if (!_targetInSightRange && !_targetInAttackRange & !_isTurretAI)
        {
            checkForDestinationDistance();
            checkforPatrolWaiting();

        }
        // AI detects player and chases. Exceptions: turret 
        if (_targetInSightRange && !_targetInAttackRange & !_isTurretAI)
        {
            ChasePlayer();
        }
        // AI attacks player 
        if (_targetInAttackRange && _targetInSightRange && _targetTransform != null)
        {
            AttackPlayer();
        }

        checkForDestinationDistance();
        checkforPatrolWaiting();
    }

    private void SetTargetRanges()
    {
        _targetInSightRange = Physics.CheckSphere(transform.position, _sightRange, _selectTargetLayer);
        _targetInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _selectTargetLayer);
    }

    private void checkRangeStatus()
    {
        if (_isRangeAI)
        {
            _fly_height = GetComponent<NavMeshAgent>().baseOffset;
            _agentIfNPCMoves.SetDestination(Vector3.up * _fly_height);
        }

        if (!_targetInSightRange && !_targetInAttackRange)
        {
            checkForPatrolPoints();
        }

        if (_targetInSightRange && !_targetInAttackRange)
        {
            ChasePlayer();
        }

        if (_targetInAttackRange && _targetInSightRange)
        {
            AttackPlayer();
        }
        else
        {
            SetMoveDestination();
        }
    }

    private void checkForPatrolPoints()
    {
        if (_agentIfNPCMoves == null)
        {
            Debug.Log("You need a Navmesh Agent to move.");
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                _currentPatrolIndex = 0;
                SetPatrolDestination();
            }
            else 
            {
                Debug.Log("You need at least two patrol points in scene.");
            }
        }
    }

    private void checkForDestinationDistance()
    {
        if (_traveling && _agentIfNPCMoves.remainingDistance <= 1.0f) // NOTE: adjustment here depending on AI behavior 
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
                SetPatrolDestination();
            }
        }
    }

    private void checkforPatrolWaiting()
    {
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;

                ChangePatrolPoint();
                SetPatrolDestination();
            }
        }
    }

    public void SetPatrolDestination()
    {
        if (_patrolPoints != null)
        {
            // get the coordinates of patrol point; set that as the destination; start traveling
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            _agentIfNPCMoves.SetDestination(targetVector);
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
            _currentPatrolIndex++;
            if (_currentPatrolIndex >= _patrolPoints.Count)
            {
                _currentPatrolIndex = 0;
            }
        }
        else
        {
            _currentPatrolIndex--;
            if (_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }

    public void AttackPlayer()
    {
        if (_isRangeAI && !_isTurretAI && !_isFighterAI)
        {
            AIShoot();
        }
        else if (_isTurretAI && !_isRangeAI && !_isFighterAI)
        {
            TurretShoot();
        }
        else if (_isFighterAI && !_isTurretAI && !_isRangeAI)
        {
            // Why is this empty?
        }
        else
        {
            Debug.Log("ERROR: unknown attack state");
        }
    }

    void TurretShoot()
    {
        turretBarrel.LookAt(_targetTransform.position + yOffset);
 
        if (!_alreadyAttacked && _turretMagCount > 0)
        {
            Instantiate(_projectile, turretEmptyTip.transform.position, turretEmptyTip.transform.rotation);
            audioSrc.PlayOneShot(tracerSound, 1);
            _turretMagCount--;
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }

        // reload 
        if (_turretMagCount <= 0 && !_isReloading)
        {
            _isReloading = true;
            Invoke(nameof(Reload), _turretReloadTime);
        }

        if (!_isReloading)
        {
            //audioSrc.PlayOneShot(turretReloadSound, 0);
        }
        else
        {
            while (_isReloading)
            {
                audioSrc.PlayOneShot(turretReloadSound, 1);
                break;
            }
        }
    }

    void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    void Reload()
    {
        _isReloading = false;
        _turretMagCount = _turretMagMax;
    }

    void AIShoot()
    {
        // stop movement 
        _agentIfNPCMoves.SetDestination(transform.position);
        transform.LookAt(_targetTransform);

        // enemy shooting
        if (!_alreadyAttacked)
        {
            Instantiate(_projectile, transform.position, transform.rotation);
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPlayer = _targetTransform.transform.position;
        _agentIfNPCMoves.SetDestination(targetPlayer);
    }

    void SetMoveDestination()
    {
        if (_moveToThisDestination != null)
        {
            Vector3 targetVector = _moveToThisDestination.transform.position;
            _agentIfNPCMoves.SetDestination(targetVector);
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
