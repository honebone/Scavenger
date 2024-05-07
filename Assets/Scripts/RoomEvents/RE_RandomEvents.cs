using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_RandomEvents : RoomEvent
{
    //[SerializeField]
    //List<GameObject> events;

    protected int choice = 0;
    //GameObject eventManager;
    public override void StartRoomEvent()
    {
        infoText.SwitchToLog();
    }
    public override void OnEndREInfo()
    {
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
