using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_RoomButton : MonoBehaviour
{
    [SerializeField]
    ExpeditionManager.Room room;

    [SerializeField]
    Image up;
    [SerializeField]
    Image straight;
    [SerializeField]
    Image down;
    [SerializeField]
    Image roomEventIcon;
    [SerializeField]
    Image frame;
    [SerializeField]
    Text currentPosText;

    [SerializeField]
    Sprite emptyIcon;

    ExpeditionManager expeditionManager;
    InfoText InfoText;
    bool selectable;
    public void Init(ExpeditionManager.Room r,Vector2Int rp,ExpeditionManager em,InfoText it,ScrollRect s)
    {
        room = r;
        room.roomPos = rp;
        expeditionManager = em;
        InfoText = it;
        scroll = s;

        if (room.empty)
        {
            roomEventIcon.sprite = emptyIcon;
            roomEventIcon.color = Color.gray;
        }
        else
        {
            roomEventIcon.sprite = room.eventIcon;
            if (room.up >= 1) { up.enabled = true; }
            if (room.straight >= 1) { straight.enabled = true; }
            if (room.down >= 1) { down.enabled = true; }
        }

        //if (!room.empty)
        //{
        //    up.enabled = true;
        //    switch (room.up)
        //    {
        //        case -1:
        //            up.color = Color.red;
        //            break;
        //        case 0:
        //            up.color = Color.gray;
        //            break;
        //        case 2:
        //            up.color = Color.green;
        //            break;
        //    }

        //    straight.enabled = true;
        //    switch (room.straight)
        //    {
        //        case -1:
        //            straight.color = Color.red;
        //            break;
        //        case 0:
        //            straight.color = Color.gray;
        //            break;
        //        case 2:
        //            straight.color = Color.green;
        //            break;
        //    }

        //    down.enabled = true;
        //    switch (room.down)
        //    {
        //        case -1:
        //            down.color = Color.red;
        //            break;
        //        case 0:
        //            down.color = Color.gray;
        //            break;
        //        case 2:
        //            down.color = Color.green;
        //            break;
        //    }
        //}
       
    }

    public void SetState_Selectable()
    {
        selectable = true;
        frame.color = Color.yellow;
    }
    public void SetState_currentPos()
    {
        currentPosText.enabled = true;
        frame.color = Color.green;
    }
    public void ResetState()
    {
        currentPosText.enabled = false;
        selectable = false;
        frame.color = Color.white;
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1) && !room.empty)
        {
            InfoText.SetText(room.eventName, room.eventInfo);
        }
        if (Input.GetMouseButtonDown(0) && selectable)
        {
            selectable = false;
            expeditionManager.GoToNextRoom(room.roomPos);
        }
    }
    [SerializeField]
    float scrollSpeed = 0.1f;
    ScrollRect scroll;
    float wheel;
    bool p;
    private void Update()
    {
        if (p)
        {
            wheel += Input.mouseScrollDelta.y;
            if (wheel != 0)
            {
                scroll.horizontalNormalizedPosition += wheel * scrollSpeed;
                wheel = 0;
            }
        }
    }
    public void OnMouseEnter()
    {
        p = true;
    }
    public void OnMouseExit()
    {
        p = false;
    }
    

    public ExpeditionManager.Room GetRoom() { return room; }
}
