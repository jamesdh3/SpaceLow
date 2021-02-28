// AIAttack.cs
/** Handles attack behaviors of AI 

Methods: 
 - AttackPlayer()
 - TurretShoot() 
 - Reload()
 - AIShoot()
 - AIStab()
 - ResetAttack() 
*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIAttack : AIMove  { 
    
    // Attacking variables 
    [SerializeField]
    private float _attackDelay;
    private bool _alreadyAttacked;

    // need instance of projectiles 
    [SerializeField]
    private GameObject _projectile;
    
    // turret variables 
    //private Transform _turret; 
    private bool _isReloading = false;
    private Transform _barrelTip;
    private Vector3 yOffset = new Vector3(0, 3, 0);
    [SerializeField]
    private float _turretReloadTime;

    [SerializeField]
    private int _turretMagMax;
    private int _turretMagCount;

    // Cannon's Barrel Position
    [SerializeField]
    private Transform cannonBarrel;
    
    
    public void Start() { 
        _turretMagCount = _turretMagMax;
        _barrelTip = GameObject.FindWithTag("barrelTip").transform;
    }


    public void Update() { 
    }


    void Reload()
    /** 
    */
    {
        Debug.Log("Should be reloading");
        _isReloading = false; 
       _turretMagCount = _turretMagMax; 
       Debug.Log(_turretMagCount);
    }

    public void AttackPlayer() 
    /** Main Attack function that will handle which attack animation an AI should do 
        possible attacks: 
         - turret attack (i.e _isTurretAI)
         - AI shoots (i.e _isRangeAI)
         - AI close combat attack (i.e _isFighterAI)
    */
    {
        // enforce only 1 can be selected at a time 
        if (_isRangeAI && !_isTurretAI && !_isFighterAI) {
            //Debug.Log("Bang Bang!");
            AIShoot();
        }
        else if (_isTurretAI && !_isRangeAI && !_isFighterAI) { 
            //Debug.Log("Turret go BRRRRrrrRRRR");
            TurretShoot(); 
        }
        else if (_isFighterAI && !_isTurretAI && !_isRangeAI) { 
            //Debug.Log("Slash attack goes here");
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
        cannonBarrel.LookAt(_player.position + yOffset);

        // turret shoots similar to AI behavior   
        if (!_alreadyAttacked && _turretMagCount > 0) {
            Instantiate(_projectile, _barrelTip.position, _barrelTip.rotation);
            _turretMagCount --;
            _alreadyAttacked = true; 
            Invoke(nameof(ResetAttack), _attackDelay); 
        }

        // reload 
        if (_turretMagCount <= 0 && !_isReloading) { 
            Debug.Log("RELOADING");
            _isReloading = true;
            Invoke(nameof(Reload), _turretReloadTime);
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
            Instantiate(_projectile, transform.position, transform.rotation);
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _attackDelay);
        }
    }


    void AIStab()
    /** tag enemy weapon, and check collider with player
    */
    {

    }
    void ResetAttack() 
    {
        _alreadyAttacked = false; 
    }

}