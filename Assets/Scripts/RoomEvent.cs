using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : MonoBehaviour
{
    protected AreaManager.Area currentArea;
    protected ExpeditionManager expeditionManager;
    public void Init(AreaManager.Area area)
    {
        currentArea = area;
        expeditionManager=FindObjectOfType<ExpeditionManager>();
        StartRoomEvent();
    }
    public virtual void StartRoomEvent() { }

    public virtual void OnEndBattle() { }
    public virtual void OnEndLoot() { }

    public  void EndRoomEvent()
    {
        expeditionManager.EndRoomEvent();
        Destroy(this.gameObject);
    }
}
