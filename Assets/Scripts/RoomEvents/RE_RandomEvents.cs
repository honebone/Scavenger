using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_RandomEvents : RoomEvent
{
    [SerializeField]
    List<GameObject> events;

    //GameObject eventManager;
    public override void StartRoomEvent()
    {
        var e  =Instantiate(events[Random.Range(0,events.Count)]);
        //eventManager=e
    }
    public override void SelectOption(int index)
    {
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        EndRoomEvent();
    }
}
