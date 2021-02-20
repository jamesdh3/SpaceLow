using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    CharacterController controller;
    AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (controller.isGrounded == true && controller.velocity.magnitude >2f && audioSrc.isPlaying == false)
        //{
        //    audioSrc.volume = Random.Range(0.5f, 0.8f);
        //    audioSrc.pitch = Random.Range(0.7f, 1.1f);
        //    audioSrc.Play();
        //}
    }


}
