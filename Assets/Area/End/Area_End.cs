using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_End : AreaManager
{
    [SerializeField] RoomEventData battle1;
    [SerializeField] RoomEventData battle2;
    [SerializeField] RoomEventData rest;
    [SerializeField] RoomEventData treasure;

    List<RoomEventData> list=new List<RoomEventData>();

    public override void GenerateMap()
    {
        map.ResetMap();

        areaLength = 6;
        list = new List<RoomEventData>();
        list.Add(areaData.start);
        list.Add(battle1);
        list.Add(treasure);
        list.Add(battle2);
        list.Add(rest);
        list.Add(areaData.boss);
        list.Add(areaData.endArea);

        for(int i = 0; i <= 6; i++) { MakeLayer(i); }

        map.SetMap(layers);
        map.EndGenerateMap();
    }

    void MakeLayer(int x)
    {
        List<ExpeditionManager.Room> layer = new List<ExpeditionManager.Room>();
        for (int i = 0; i < 5; i++)
        {
            layer.Add(new ExpeditionManager.Room());
        }
        layer[0].empty = true;
        layer[1].empty = true;

        layer[2].SetBranch_OnInit(-1, 2, -1);
        layer[2].SetRoomEvent(list[x]);

        layer[3].empty = true;
        layer[4].empty = true;

        layers.Add(layer);
    }
}
