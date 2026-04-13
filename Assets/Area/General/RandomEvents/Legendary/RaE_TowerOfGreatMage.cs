using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaE_TowerOfGreatMage : RE_RandomEvents
{
    [SerializeField] List<ItemData> eqs;
    public override void StartRandomEvent()
    {
        List<ItemData> obtain=new List<ItemData>(eqs);
        Definer.equipments.ForEach(E =>
        {
            obtain.AddRange(E.Where(e => e.manager.GetComponent<PassiveAbility>().PATags.Contains(PassiveAbility.PATag.ñÇèp)));
        });

        lootPanel.AddItem(obtain);
        lootPanel.Loot();
    }
}
