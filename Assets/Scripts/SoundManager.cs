using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
   

    public void PlaySE(AudioClip clip) { audioSource.PlayOneShot(clip); }

  
}
