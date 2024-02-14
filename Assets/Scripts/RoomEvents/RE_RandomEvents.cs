using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_RandomEvents : RoomEvent
{
    //[SerializeField]
    //List<GameObject> events;
    [SerializeField]
    string eventName;
    [SerializeField, TextArea(3, 10)]
    string eventInfo;

    protected int choice = 0;
    //GameObject eventManager;
    public override void StartRoomEvent()
    {
        if (eventName != "") { expeditionManager.LogREName(eventName); }
        if (eventInfo != "") { infoText.AddLogText(eventInfo + "\n"); }
        infoText.SwitchToLog();
        StartRandomEvent();
    }
    public virtual void StartRandomEvent()
    {

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        EndRoomEvent();
    }
}
