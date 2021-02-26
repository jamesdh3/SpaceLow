using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    public AudioSource thirdAudioSrc;


    // Start is called before the first frame update
    void Start()
    {
        thirdAudioSrc = GetComponent<AudioSource>();
    }

    public void TakeDmgSoundEffect()
    {
        AudioClip clip = GetRandomClip();
        thirdAudioSrc.volume = Random.Range(0.5f, 0.8f);
        thirdAudioSrc.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

}
