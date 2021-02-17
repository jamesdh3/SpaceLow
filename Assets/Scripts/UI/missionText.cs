// missionText.cs
/** Class to handle current objectives, and event handlings when there are new objectives 
or an objective was complete 
*/
/*using UnityEngine;
using System.Collections;


public class missionText : MonoBehaviour { 

    public GameObject text; 

    void Start() 
    { 
        text.SetActive(false); 
    }

    void OnTriggerEnter(collider player) 
    {
        if (player.GameObject.tag == "Player") 
        { 
            text.SetActive(true);
            StartCoroutine("DelayText");
        }
    }

    IEnumerator DelayText() 
    { 
        yield return new WaitForSeconds(5); 
        Destroy(text); 
        Destroy(GameObject); 
    }
}

*/