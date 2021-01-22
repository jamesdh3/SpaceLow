// AIMove.cs
/** Moves the player 

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

public class AIMove : MonoBehaviour
{

    [SerializeField] 
    Transform _destination; 

    UnityEngine.AI.NavMeshAgent _navMeshAgent; 


    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>(); 

        if (_navMeshAgent == null) 
        {
            // navmesh component needed 
        }
        else 
        {
            SetDestination(); 
        }
    }

    private void SetDestination() 
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position; 
            _navMeshAgent.SetDestination(targetVector); 
        }
    }

}
