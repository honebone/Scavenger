using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    [System.Serializable]
    public struct Room
    {
        /// <summary>開始地点の上下2段ずつとかはtrue</summary>
        public bool empty;
        //roomevent

        public bool blind;
        /// <summary>-1:false(locked) 0:false 1:true 2:true(locked)</summary>
        public int up;
        /// <summary>-1:false(locked) 0:false 1:true 2:true(locked)</summary>
        public int straight;
        /// <summary>-1:false(locked) 0:false 1:true 2:true(locked)</summary>
        public int down;

        public Vector2Int roomPos;
    }

    /// <summary>x:現在のレイヤー y:上下</summary>
    [SerializeField]
    Vector2Int currentPos;
    Room currentRoom;
    List<Map_LayerPanel> layers;


    Map_MapPanel mapPanel;

    private void Start()
    {
        mapPanel=FindObjectOfType<Map_MapPanel>();
    }
    public void SetLayers(List<Map_LayerPanel> l)
    {
        layers = l;

        currentPos = new Vector2Int(0, 2);
        currentRoom = GetRoom(currentPos);

        GetRoomButton(currentPos).SetState_currentPos();
        SelectNextRoom();
    }
    

    public void SelectNextRoom()
    {
        if (currentRoom.up > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y + 1)).SetState_Selectable(); }
        if (currentRoom.straight > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y)).SetState_Selectable(); }
        if (currentRoom.down > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y - 1)).SetState_Selectable(); }
        mapPanel.OpenMap();
    }
    public void GoToNextRoom(Vector2Int pos)
    {
        mapPanel.CloseMap();
        currentPos = pos;
        currentRoom = GetRoom(currentPos);

        foreach(Map_LayerPanel layer in layers) { layer.ResetButtonsState(); }
        GetRoomButton(currentPos).SetState_currentPos();
    }

    public Map_RoomButton GetRoomButton(Vector2Int pos) { return layers[pos.x].GetRoomButton(pos.y); }
    public Room GetRoom(Vector2Int pos) { return layers[pos.x].GetRoom(pos.y); }
}
