using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class RaE_PoisonSwamp : RE_RandomEvents
{
    [SerializeField] Vector2Int HPLose;
    [SerializeField] Vector2Int coin;
    [SerializeField] REOptionParams option;
    public override void StartRandomEvent()
    {
        SetOption();
    }

    public override void SelectOption(int index)
    {
        if(index == 0)
        {
            players.ForEach(p =>
            {
                p.DecreaseHP_Per(HPLose.Range());
            });

            lootPanel.AddCoin(coin.Range_ND());
            lootPanel.Loot();
        }
        else
        {
            EndRoomEvent();
        }
    }

    public override void OnEndLoot()
    {
        SetOption();
    }

    void SetOption()
    {
        option.available = players.All(p => p.CharaStatus().HP > 0);
        expeditionManager.SetREOptionButtons(new List<REOptionParams>() { option, option_exit });
    }
}
