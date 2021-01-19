using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class QuitScript : MonoBehaviour
{
     
    public void Escape()
    {
		Debug.Log ("QUIT!");
        Application.Quit();
    }
}