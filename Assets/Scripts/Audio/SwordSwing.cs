using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    public AudioSource secondAudioSrc;


    // Start is called before the first frame update
    void Start()
    {
        secondAudioSrc = GetComponent<AudioSource>();
    }

    // This is function is being called using the Animation Events.  
    void SwordSwingSound()
    {
        AudioClip clip = GetRandomClip();
        secondAudioSrc.volume = Random.Range(0.5f, 0.8f);
        secondAudioSrc.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

}
