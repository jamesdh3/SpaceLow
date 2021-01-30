// AIDetect.cs 

/**
Class: AIDetect

Methods: 
 - TBD

How it works: A statis value for radius of detection is set for each AI. If a player 
    walks within the range, the AI will then chase the player 


NOTE: may be good to have both implement this area detection, and FOV in order 
    to have different AI behavior 

Unity Requirements: 
 - Agents will need NavMeshAgent component 
 - also this script if feature is desired 
 
Additional Features TBD: 
 - distance or time that enemy will stop chasing player 
 - Enemy hearing (player radius of noise should be implemented first)
 - implement "eye-sight" detection, instead of spherical area 
*/

using UnityEngine;
using UnityEngine.AI; 


public class AIDetect : MonoBehaviour
{
    [SerializeField]
    private float _sightRange, _attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 
    

    // reference to wrapper movement script that'll handle cases outside of scope of this script 
    //private static AIMove AIM;
    // player and AI 
    private Transform _player; 
    private NavMeshAgent _agent; 

    public void Start() 
    {
        _player = GameObject.FindWithTag("Player").transform;
        _agent  = GetComponent<NavMeshAgent>(); 
        if (_playerInSightRange && !_playerInAttackRange)
        {
            ChasePlayer(); 
        }
    }


    public void Update() 
    {
        if (_playerInSightRange && !_playerInAttackRange)
        {
            ChasePlayer(); 
        }
    }

    void ChasePlayer()
    /** When in sightrange, AI will move towards player
    */
    {
        Vector3 targetPlayer = _player.transform.position; 
        _agent.SetDestination(targetPlayer); 
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

}