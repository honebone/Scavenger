using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaySE(AudioClip clip) { audioSource.PlayOneShot(clip); }

  
}
