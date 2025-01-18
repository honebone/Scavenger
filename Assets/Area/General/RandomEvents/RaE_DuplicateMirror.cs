using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_DuplicateMirror : RE_RandomEvents
{
    [SerializeField] REOptionParams skip;
    [SerializeField] int optionsAmount;


    List<Definer.Item> eq = new List<Definer.Item>();
    public override void OnEndREInfo()
    {
        List<REOptionParams> list = new List<REOptionParams>();
        eq = new List<Definer.Item>(Inventory.inst.GetEquipments_WithEquipped()).Sample(optionsAmount);
        foreach (Definer.Item item in eq)
        {
            REOptionParams option = new REOptionParams();
            string eqName = item.data.itemName.ColorStr(item.data.rarity.ToColor());
            option.optionName = eqName;
            option.optionInfo = $"{eqName}Çï°êªÇ∑ÇÈ";
            option.optionInfo += $"\n\n{item.GetInfo()}";

            list.Add(option);
        }

        list.Add(skip);
        expeditionManager.SetREOptionButtons(list);
    }

    public override void SelectOption(int index)
    {
        if(index < eq.Count)
        {
            LootPanel.inst.AddItem(eq[index], 1);
            LootPanel.inst.Loot();
        }
        else { EndRoomEvent(); }
    }
}
