using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
   
    void Awake()
    {
        CheckInstance();
    }

    void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource BGM_normal;
    [SerializeField] AudioSource BGM_battle;


    List<AudioClip> clipList;

    public void SetSEVolume(float value)
    {
        audioSource.volume = value;
    } 
    public void SetBGMVolume(float value)
    {
        BGM_normal.volume = value;
        BGM_battle.volume = value;
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip != null && !clipList.Contains(clip))
        {
            clipList.Add(clip);
            audioSource.PlayOneShot(clip);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        clipList = new List<AudioClip>();
    }
    private void Update()
    {
        clipList.Clear();
    }

    public void SetBGM_Normal(AudioClip bgm)
    {
        BGM_normal.clip = bgm;
        BGM_normal.Play();

    }
    public void PlayBGM_Normal()
    {
        if (BGM_battle.isPlaying) { BGM_battle.Stop(); }
        if (!BGM_normal.isPlaying) BGM_normal.Play();
    }
    public void StartBGM_Battle(AudioClip bgm)
    {
        if (BGM_normal.isPlaying) { BGM_normal.Pause(); }
        BGM_battle.clip = bgm;
        BGM_battle.Play();
    }

    public void StopBGMs()
    {
        if (BGM_battle.isPlaying) { BGM_battle.Stop(); }
        if (BGM_normal.isPlaying) { BGM_normal.Pause(); }
    }

}
