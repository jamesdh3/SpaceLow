using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        playAnimations();
    }

    void playAnimations()
    {
        if (Input.GetKey("w"))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetKey("a"))
        {
            anim.SetBool("isStrafeL", true);
        }
        else
        {
            anim.SetBool("isStrafeL", false);
        }

        if (Input.GetKey("d"))
        {
            anim.SetBool("isStrafeR", true);
        }
        else
        {
            anim.SetBool("isStrafeR", false);
        }

        if (Input.GetKey("s"))
        {
            anim.SetBool("isWalkingBackW", true);
        }
        else
        {
            anim.SetBool("isWalkingBackW", false);
        }
    }
}
