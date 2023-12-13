using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Equipment : PassiveAbility
{
    [System.Serializable]
    public struct EquipmentStatus
    {
        public ItemData itemData;
        public Character.CharaStatusMod statusMod;

        [TextArea(3, 10)]
        public string info_start;

        public string GetName()
        {
            return itemData.itemName.ColorStr(itemData.rarity.ToColor());
        }
        public string GetInfo()
        {
            string s = "";
            if (info_start != "") { s = info_start + "\n"; }         
            s += statusMod.GetInfo();
            return s;
        }
    }
    public override string GetPAName()
    {
        return equipmentStatus.GetName();
    }
    public override string GetPAInfo()
    {
        return equipmentStatus.GetInfo();
    }
    [SerializeField]
    protected EquipmentStatus equipmentStatus;
    public override void OnPAInit()
    {
        character.ModifyStatus(equipmentStatus.statusMod, true);
    }
    public override void AtTheEnd()
    {
        character.ModifyStatus(equipmentStatus.statusMod, false);
    }
}
