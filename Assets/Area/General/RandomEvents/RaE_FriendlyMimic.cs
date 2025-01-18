using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_FriendlyMimic : RE_RandomEvents
{
    [SerializeField] REOptionParams skip;
    [SerializeField] int deliverOptions;
    [SerializeField] int obtainOptions;


    List<Definer.Item> eq = new List<Definer.Item>();
    List<Definer.Item> pool = new List<Definer.Item>();
    public override void OnEndREInfo()
    {
        List<REOptionParams> list = new List<REOptionParams>();
        //pool = new List<Definer.Item>(Inventory.inst.GetEquipments_WithEquipped()).Sample(deliverOptions);

        foreach(Definer.Item item in Inventory.inst.GetEquipments_WithEquipped())
        {
            if(item.data.rarity != ItemData.Rarity.uncommon) { pool.Add(item); }
        }

        eq = new List<Definer.Item>(pool.Sample(deliverOptions));
        foreach (Definer.Item item in eq)
        {
            REOptionParams option = new REOptionParams();
            string eqName = item.data.itemName.ColorStr(item.data.rarity.ToColor());
            option.optionName = eqName;
            option.optionInfo = $"{eqName}を失い、同レアリティのランダムな装備品3つのうちから1つを選んで入手する";

            list.Add(option);
        }

        list.Add(skip);
        expeditionManager.SetREOptionButtons(list);
    }

    public override void SelectOption(int index)
    {
        if (index < eq.Count)
        {
            foreach(ItemData data in Definer.equipments[(int)eq[index].data.rarity].Sample(obtainOptions))
            {
                Definer.Item item = new Definer.Item();
                item.Init(data);

                SupplyManager.inst.AddItem(item, 1);
            }

            //アイテム失う
            SupplyManager.inst.StartSupply();
        }
        else { EndRoomEvent(); }
    }
}
