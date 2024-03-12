using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_RatsGift : RoomEvent
{
    [SerializeField, TextArea(3, 10)]
    string info;
    public override void StartRoomEvent()
    {
        expeditionManager.LogREName("ƒlƒYƒ~‚Ì‘¡‚è•¨");
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        //yield return new WaitForSeconds(1f);
        //infoText.AddLogText(info);
        yield return new WaitForSeconds(1f);
        supplyManager.SetSupply_Eq(partyStatus.supplyOptions);
        supplyManager.StartSupply();
    }
}
