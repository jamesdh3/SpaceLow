using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttack : AIMove  { 
    
    [SerializeField]
    private float _attackDelay;
    private bool _alreadyAttacked;

    [SerializeField]
    private GameObject _projectile;
    
    // turret variables 
    [SerializeField]
    private bool _isReloading = false;
    private Vector3 yOffset = new Vector3(0, 3, 0);
    [SerializeField]
    private float _turretReloadTime;

    [SerializeField]
    private int _turretMagMax;
    private int _turretMagCount;

    // Cannon's Barrel Position
    [SerializeField]
    private Transform cannonBarrel;
    public Transform barrelTip;

    // Audio
    private AudioSource audioSrc;
    public ReloadTurretSound turretReloadSound;

    public void Start() {
        _isReloading = false;
        _turretMagCount = _turretMagMax;
        audioSrc = GetComponent<AudioSource>();
    }

    void Reload()
    {
        _isReloading = false;
        _turretMagCount = _turretMagMax; 
    }

    public void AttackPlayer() 
    {
        // enforce only 1 can be selected at a time 
        if (_isRangeAI && !_isTurretAI && !_isFighterAI) {
            AIShoot();
        }
        else if (_isTurretAI && !_isRangeAI && !_isFighterAI) { 
            TurretShoot(); 
        }
        else if (_isFighterAI && !_isTurretAI && !_isRangeAI) { 
        }
        else {
            Debug.Log("ERROR: unknown attack state");
        }
    }

    void TurretShoot() 
    {
        cannonBarrel.LookAt(_player.position + yOffset);

        // turret shoots similar to AI behavior   
        if (!_alreadyAttacked && _turretMagCount > 0)
        {
            Instantiate(_projectile, barrelTip.transform.position, barrelTip.transform.rotation);

            audioSrc.volume = Random.Range(0.8f, 1.0f);
            audioSrc.pitch = Random.Range(0.8f, 1.0f);
            audioSrc.Play();

            _turretMagCount--;
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }

        // reload 
        if (_turretMagCount <= 0 && !_isReloading) {
            Debug.Log("Testing if this is being started");
            _isReloading = true;
            turretReloadSound.TurretReloadSound();
            Invoke(nameof(Reload), _turretReloadTime);
        }
    }

    void AIShoot() 
    {
         // stop movement 
        _agent.SetDestination(transform.position);
        transform.LookAt(_player);

        // enemy shooting
        if (!_alreadyAttacked)
        {
            Instantiate(_projectile, transform.position, transform.rotation);
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }
    }

    void ResetAttack() 
    {
        _alreadyAttacked = false; 
    }
}