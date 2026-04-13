using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Watchtower : RE_RandomEvents
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;

    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(options);
    }


    public override void SelectOption(int index)
    {
        choice = index;
        //StartCoroutine(Consequence());

        switch (choice)
        {
            case 0:
                infoText.AddLogText("行く先を照らし、自分たちを待ち受ける脅威がいないか確認した");
                infoText.SwitchToLog(); 
                expeditionManager.GetAreaManager().Reveal(100);

                break;
            case 1:
                infoText.AddLogText("入念に観察し、いくつかの抜け道を見つけた");
                infoText.SwitchToLog();
                expeditionManager.GetAreaManager().AddBranch(50);

                break;
        }

        EndRoomEvent();
    }

    //IEnumerator Consequence()
    //{

    //}
}
