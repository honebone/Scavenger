using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_LayerPanel : MonoBehaviour
{
    [SerializeField]
    Map_RoomButton[] roomButtons;
    [SerializeField]
    Text layerText;

    int layerCount;

    public void Init(ExpeditionManager.Room[] r,int lc,ExpeditionManager em,InfoText it,ScrollRect scroll)
    {
        layerCount = lc;
        layerText.text = layerCount.ToString();

        for(int i = 0; i < 5; i++) { roomButtons[i].Init(r[i], new Vector2Int(layerCount, i),em,it,scroll); }
    }
    public void ResetButtonsState()
    {
        for (int i = 0; i < 5; i++) { roomButtons[i].ResetState(); }
    }

    public Map_RoomButton GetRoomButton(int row) { return roomButtons[row]; }
    public ExpeditionManager.Room GetRoom(int row) { return roomButtons[row].GetRoom(); }
}
