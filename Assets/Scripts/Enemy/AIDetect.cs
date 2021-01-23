// AIDetect.cs 

/**
Class: AIDetect

Methods: 
 - TBD

How it works: 
2 options: 
 1: have a radius for detection. once player walks within radius, AI will start moving towards player 
 2: apply some sort field of view for AI. Only if and when player is in field of view, AI
    will move towards player  

    NOTE: may be good to have both implemented to have different AI behavior 

Unity Requirements: 
 - Agents will need NavMeshAgent component 
 - also this script if feature is desired 
 
Additional Features TBD: 
 - distance or time that enemy will stop chasing player 
 - Enemy hearing (player radius of noise should be implemented first)
 - 
*/

using UnityEngine;
using UnityEngine.AI; 


public class AIDetect : MonoBehaviour
{

    public void Start() 
    {

    }

    public void Update() 
    {

    }
}