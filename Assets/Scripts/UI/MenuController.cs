using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class MenuController : MonoBehaviour
{
    public GameObject escape;
    public GameObject returnToMainMenuButton;
    public GameObject grayBackground;

    public bool EscapeMenuOpen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeMenuOpen == false)
            {
                GameObject.FindWithTag("Player").GetComponent<MouseLock>().enabled = false;
                EscapeMenuOpen = true;

                escape.gameObject.SetActive(true);
                returnToMainMenuButton.SetActive(true);
                grayBackground.SetActive(true);
            }
            else
            {
                GameObject.FindWithTag("Player").GetComponent<MouseLock>().enabled = true;
                EscapeMenuOpen = false;

                escape.gameObject.SetActive(false);
                returnToMainMenuButton.SetActive(false);
                grayBackground.SetActive(false);
            }
        }
    }
}