using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Equipment : PassiveAbility
{
    [System.Serializable]
    public struct EquipmentStatus
    {
        public ItemData itemData;
        //[TextArea(3, 10)]
        //public string info_start;
        //public Character.CharaStatusMod statusMod;
        //public List<GameObject> actionMods;
        
        public string GetName()
        {
            return itemData.itemName.ColorStr(itemData.rarity.ToColor());
        }
    }
    //public override string GetSimpleInfo()
    //{
    //    string s = equipmentStatus.statusMod.GetInfo() != "" ? equipmentStatus.statusMod.GetInfo()+ "\n\n" : "";
    //    return $"{s}{simpleInfo}";
    //}
    public override string GetPAName()
    {
        return equipmentStatus.GetName();
    }
    public override string GetPAInfo_Base()
    {
        return "";
    }
    [SerializeField]
    protected EquipmentStatus equipmentStatus;
    public EquipmentStatus GetEquipmentStatus() { return equipmentStatus; }
  
    //public override void OnPAInit()
    //{
    //    character.ModifyStatus(equipmentStatus.statusMod, true);
    //    foreach(GameObject mod in equipmentStatus.actionMods) { character.AddActionMod(mod, true); }
    //}
    //public override void AtTheEnd()
    //{
    //    character.ModifyStatus(equipmentStatus.statusMod, false);
    //    foreach (GameObject mod in equipmentStatus.actionMods) { character.AddActionMod(mod, false); }
    //}
}
