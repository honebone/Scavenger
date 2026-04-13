using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RaE_OrbOfWish : RE_RandomEvents
{
    [SerializeField] ItemData mimic;
    [SerializeField] int mimicAmount;
    [SerializeField] int exp;

    [SerializeField] int HPLose;
    [SerializeField] Vector2Int coin;

    [SerializeField] List<REOptionParams> options;
    public override void StartRandomEvent()
    {
        PassiveAbility PA = mimic.manager.GetComponent<PassiveAbility>();
        options[0].optionInfo += $"{PA.GetPAName()}‚ð“¾‚é\n\n{PA.GetPAName()}F\n{PA.GetPAInfo()}";
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            lootPanel.AddItem(mimic, mimicAmount);
            lootPanel.Loot();
        }
        else if(index == 1) 
        {
            lootPanel.AddExp(exp);
            lootPanel.Loot();
        }
        else
        {
            players.ForEach(p =>
            {
                p.DecreaseHP_Per(HPLose);
            });
            lootPanel.AddCoin(coin.Range());
            lootPanel.Loot();
        }        
    }
}
