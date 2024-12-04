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
    [SerializeField] Image currentRoomImage;
    [SerializeField]
    Text currentPosText;

    [SerializeField]
    Sprite blindIcon;

    ExpeditionManager expeditionManager;
    InfoText InfoText;
    MouseOverUI mouseOver;

    bool selectable;
    bool blind;
    public void Init(ExpeditionManager.Room r,Vector2Int rp,ExpeditionManager em,InfoText it,ScrollRect s,MouseOverUI mo)
    {
        room = r;
        room.roomPos = rp;
        expeditionManager = em;
        InfoText = it;
        scroll = s;
        mouseOver = mo;

        blind = room.blind;

        if (room.empty)
        {
            roomEventIcon.enabled = false;
            //roomEventIcon.color = Color.gray;
        }
        else
        {
            if (blind)
            {
                roomEventIcon.sprite = blindIcon;
                //frame.color = Color.gray;
            }
            else { roomEventIcon.sprite = room.eventIcon; }
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
        currentRoomImage.enabled = true;
        //frame.color = Color.green;
    }
    public void ResetState()
    {
        currentPosText.enabled = false;
        selectable = false;
        frame.color = Color.clear;
        currentRoomImage.enabled = false;
        //if (blind) { frame.color = Color.gray; }
        //else { frame.color = Color.white; }
    }

    public void SetBranchUp(int value)
    {
        room.up = value;
        if (value >= 1) { up.enabled = true; }
        else { up.enabled = false; }
    }
    public void SetBranchDown(int value)
    {
        room.down = value;
        if (value >= 1) { down.enabled = true; }
        else { down.enabled = false; }
    }
    public void SetBlind(bool set)
    {
        room.blind = set;
        blind = set;
        if (blind) { roomEventIcon.sprite = blindIcon; }
        else { roomEventIcon.sprite = room.eventIcon; }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1) && !room.empty)
        {
            if (blind) { InfoText.SetText("ˆÃˆÅ", "‰½‚ª‹N‚±‚é‚©•ª‚©‚ç‚È‚¢"); }
            else { InfoText.SetText(room.eventName, room.eventInfo); }
        }
        if (Input.GetMouseButtonDown(0) && selectable)
        {
            selectable = false;
            mouseOver.ResetUI();
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
        if (!room.empty)
        {
            if (blind) { mouseOver.SetUI("ˆÃˆÅ", true); }
            else { mouseOver.SetUI(room.eventName, true); }
        }
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
        p = false;
    }


    public ExpeditionManager.Room GetRoom() { return room; }
}
