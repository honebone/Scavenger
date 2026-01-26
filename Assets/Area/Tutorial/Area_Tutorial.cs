using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Tutorial : AreaManager
{
    [SerializeField] RoomEventData battle1;
    [SerializeField] RoomEventData battle2;
    [SerializeField] RoomEventData RaE;
    [SerializeField] RoomEventData rat;

    //public override void GenerateMap()
    //{
    //    map.ResetMap();

    //    int layerCount = 0;

    //    for (int i = 0; i < 5; i++) { layer[i].empty = true; }//0‘w–Ú
    //    layer[2].empty = false;
    //    layer[2].straight = 1;
    //    layer[2].SetRoomEvent(areaData.start);
    //    map.SetLayerPanel(layer, layerCount);
    //    layerCount++;

       
    //    layer[2] = SetRoom(false);//1‘w–Ú@í“¬1
    //    layer[2].SetRoomEvent(battle1);
    //    layer[2].up = 2;
    //    layer[2].straight = 2;
    //    layer[2].down = 2;


    //    map.SetLayerPanel(layer, layerCount);
    //    layerCount++;

    //    //<<2‘w–Ú>>
    //    for(int i = 1; i <= 3; i++)
    //    {
    //        layer[i] = SetRoom(false);
    //        layer[i].up = -1;
    //        layer[i].straight = 2;
    //        layer[i].down = -1;
    //    }
    //    layer[1].SetRoomEvent(battle2);
    //    layer[2].SetRoomEvent(RaE);
    //    layer[3].SetRoomEvent(rat);

    //    map.SetLayerPanel(layer, layerCount);
    //    layerCount++;

    //    //<<3‘w–Ú>>
    //    for (int i = 1; i <= 3; i++)
    //    {
    //        layer[i] = SetRoom(false);
    //        layer[i].up = -1;
    //        layer[i].straight = 2;
    //        layer[i].down = -1;
    //    }
    //    layer[1].SetRoomEvent(rat);
    //    layer[2].SetRoomEvent(battle2);
    //    layer[3].SetRoomEvent(RaE);

    //    map.SetLayerPanel(layer, layerCount);
    //    layerCount++;

    //    //<<4‘w–Ú>>
    //    for (int i = 1; i <= 3; i++) { layer[i] = SetRoom(false); }
    //    layer[1].up = 2;
    //    layer[1].straight = -1;
    //    layer[1].down = -1;
    //    layer[1].SetRoomEvent(RaE);

    //    layer[2].up = -1;
    //    layer[2].straight = 2;
    //    layer[2].down = -1;
    //    layer[2].SetRoomEvent(rat);

    //    layer[3].up = -1;
    //    layer[3].straight = -1;
    //    layer[3].down = 2;
    //    layer[3].SetRoomEvent(battle2);

    //    map.SetLayerPanel(layer, layerCount);
    //    layerCount++;

    //    //<<5‘w–Ú>>
    //    for (int i = 0; i < 5; i++) { layer[i].empty = true; }
    //    layer[2].empty = false;
    //    layer[2] = SetRoom(false);
    //    layer[2].up = -1;
    //    layer[2].straight = -1;
    //    layer[2].down = -1;
    //    layer[2].SetRoomEvent(areaData.boss);
    //    map.SetLayerPanel(layer, layerCount);

    //    map.EndGenerateMap();
    //}
}
