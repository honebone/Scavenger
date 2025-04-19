using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEndLog : MonoBehaviour
{
   protected RoomEndLogManager manager;
    protected SoundManager soundManager;
    public void Init(RoomEndLogManager relm)
    {
        manager = relm;
        soundManager = SoundManager.instance;
    }
    public void LogStart(bool first)
    {
        gameObject.SetActive(true);
       if(first) OnLogStart();
    }
    public virtual void OnLogStart()
    {

    }
}
