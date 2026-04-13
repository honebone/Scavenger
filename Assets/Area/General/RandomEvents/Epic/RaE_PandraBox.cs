using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RaE_PandraBox : RE_RandomEvents
{
    [SerializeField] ItemData pandora;
    [SerializeField] int perAdd;
    [SerializeField] List<REOptionParams> options;
    public override void StartRandomEvent()
    {
        options[1] = new REOptionParams(options[1]);
        options[1].AddEqInfo(pandora);
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            players.ForEach(p => expeditionManager.SetRandomPer(p, perAdd));
            EndRoomEvent();
        }
        else
        {
            lootPanel.AddItem(pandora);
            lootPanel.Loot();
        }
    }
}
