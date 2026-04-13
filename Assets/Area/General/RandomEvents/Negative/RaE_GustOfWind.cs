using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RaE_GustOfWind : RE_RandomEvents
{
    [SerializeField] Vector2Int coin;
    [SerializeField] Vector2Int eq;
    [SerializeField] List<REOptionParams> options;

    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            inventory.RemoveCoin(coin.Range());
        }
        else
        {
            List<Definer.Item> remove = inventory.GetEquipments().Sample(eq.Range());
            remove.ForEach(r => inventory.RemoveItem(r, 1));
        }

        EndRoomEvent();
    }
}
