using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
   
    List<AudioClip> clipList;
    public void PlaySE(AudioClip clip) {
        if (!clipList.Contains(clip))
        {
            clipList.Add(clip);
            audioSource.PlayOneShot(clip);
        }
    }
    private void Start()
    {
        clipList = new List<AudioClip>();
    }
    private void Update()
    {
        clipList.Clear();
    }

}
