using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : MonoBehaviour
{
    public void Init()
    {
        StartRoomEvent();
    }
    public virtual void StartRoomEvent() { }
    public virtual void EndRoomEvent() { }
}
