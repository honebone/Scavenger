using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_End : AreaManager
{
    [SerializeField] RoomEventData battle1;
    [SerializeField] RoomEventData battle2;
    [SerializeField] RoomEventData rest;
    [SerializeField] RoomEventData treasure;

    public override void GenerateMap()
    {
        map.ResetMap();

        int layerCount = 0;

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//0‘w–Ú
        layer[2].empty = false;
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(areaData.start);
        map.SetLayerPanel(layer, layerCount);
        layerCount++;

        //<<1‘w–Ú>>
        layer[2] = new ExpeditionManager.Room();
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(battle1);

        map.SetLayerPanel(layer, layerCount);
        layerCount++;

        //<<2‘w–Ú>>
        layer[2] = new ExpeditionManager.Room();
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(treasure);

        map.SetLayerPanel(layer, layerCount);
        layerCount++;

        //<<3‘w–Ú>>
        layer[2] = new ExpeditionManager.Room();
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(battle2);

        map.SetLayerPanel(layer, layerCount);
        layerCount++;

        //<<4‘w–Ú>>
        layer[2] = new ExpeditionManager.Room();
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(rest);

        map.SetLayerPanel(layer, layerCount);
        layerCount++;

        //<<5‘w–Ú>>
        layer[2] = new ExpeditionManager.Room();
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(areaData.boss);

        map.SetLayerPanel(layer, layerCount);

        map.EndGenerateMap();
    }
}
