using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_AreaEnd : RoomEvent
{
    [SerializeField]
    string eventName;
    [SerializeField, TextArea(3, 10)]
    string eventInfo;
    [SerializeField]
    List<RoomEvent.REOptionParams> options;

    protected int choice = 0;
    //GameObject eventManager;
    public override void StartRoomEvent()
    {
        if (eventName != "") { expeditionManager.LogREName(eventName); }
        if (eventInfo != "") { infoText.AddLogText(eventInfo + "\n"); }
        infoText.SwitchToLog();

        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        choice = index;

        expeditionManager.EndExpediton();//test
    }
    //IEnumerator Consequence()
    //{
    //    switch (choice)
    //    {

    //    }
    //}
}
