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
    void FixedUpdate()
    {
        Step();
    }

    // This is function is being called using the Animation Events.  Go to Animation tab and select run animation, then at the top of bar you will see I created two events
    // which connect to this function.  It's set to play each time a foot hits the ground.
    void Step()
    {
        if (controller.isGrounded == true && controller.velocity.magnitude >2f && audioSrc.isPlaying == false)
        {
            audioSrc.volume = Random.Range(0.5f, 0.8f);
            audioSrc.pitch = Random.Range(0.7f, 1.1f);
            audioSrc.Play();
        }
    }

    // If I want to remove the event, just delete it from the animation, and then move this function to be called in Update(), then it works for all movements, forwards, back, etc

}
