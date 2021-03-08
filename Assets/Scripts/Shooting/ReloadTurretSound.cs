using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadTurretSound : MonoBehaviour
{
    public AudioSource secondAudioSrc;

    // Start is called before the first frame update
    void Start()
    {
        secondAudioSrc = GetComponent<AudioSource>();
    }
  
    public void TurretReloadSound()
    {
       // Debug.Log("I should be playng");
        secondAudioSrc.Play();
    }
}
