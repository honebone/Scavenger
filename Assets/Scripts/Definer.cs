using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definer : MonoBehaviour
{
    [System.Serializable]
    public class ColorRef
    {
        public Color debug;
        /// <summary>0:other 1:attack 2:heal 3:buff 4:debuff 5:summon</summary>
        public Color[] abilityColors;
        public Color[] personalityColors;
        /// <summary>0:other 1:buff 2:debuff 3:focus</summary>
        public Color[] statusEffectColors;
        /// <summary>0:other 1:buff 2:debuff 3:focus</summary>
        public Color[] positionEffectColors;
        /// <summary>0:common 1:uncommon 2:rare 3:epic 4:legendary</summary>
        public Color[] rarityColors;

        public Color affricted;

        public Color decreaseHP;
        public Color damage;
        public Color CRIT;
        public Color evade;
        public Color heal;
        public Color shield;
        public Color shieldDecrease;
        public Color SANHeal;
        public Color SANDecrease;

        public Color failed_unavailable;
        public Color currentState;
        public Color emphasize;

        public Color AMod;

    }
    [System.Serializable]
    public class SoundRef
    {
        public AudioClip miss;
        public AudioClip evade;
        public AudioClip damage;
        public AudioClip CRIT;
        public AudioClip dying;
        public AudioClip heal;
        public AudioClip shield;
        public AudioClip shieldRemove;
        public AudioClip SANHeal;
        public AudioClip SANDecrease;
        public AudioClip[] ApplyStE;
        public AudioClip summoned;
        public AudioClip stun;
        public AudioClip die1;
        public AudioClip die2;
        public AudioClip[] getItem;
    }
    [System.Serializable]
    public class VisualEffectRef
    {
        public GameObject die;
    }

    public static Character.CharacterStatus nonCharaStatus;
    public static ColorRef colorRef;
    public static SoundRef soundRef;
    public static VisualEffectRef VERef;
    public static GameObject abilityManager_General;
    public static GameObject actionManager_General;
    public static GameObject statusEffectIcon;
    public static GameObject positionEffectIcon;
    [SerializeField]
    CharacterData nonCharacterData;
    [SerializeField]
    ColorRef colorRef_Inspector;
    [SerializeField]
    SoundRef soundRef_Inspector;
    [SerializeField]
    VisualEffectRef VERef_Inspector;
    [SerializeField]
    GameObject abilityManager_General_Inspector;
    [SerializeField]
    GameObject actionManager_General_Inspector;
    [SerializeField]
    GameObject statusEffectIcon_Inspector;
    [SerializeField]
    GameObject positionEffectIcon_Inspector;

    [SerializeField] List<CharacterData> playerDataBase;
    [SerializeField] List<ItemData> equipmentDataBase;
    [SerializeField] List<GameObject> personalityDataBase;
    [SerializeField] List<GameObject> affrictionDataBase;
    public List<CharacterData> GetPlayerDataBase() { return playerDataBase; }
    public List<ItemData> GetAllEquipments() { return equipmentDataBase; }
    public List<GameObject> GetPersonalityDataBase() { return personalityDataBase; }
    public List<GameObject> GetAffrictionDataBase() { return affrictionDataBase; }


    public static List<List<ItemData>> equipments = new List<List<ItemData>>();



    public static Dictionary<AbilityData.AbilityType, string> AbiltyTypeName = new Dictionary<AbilityData.AbilityType, string>(){
    {AbilityData.AbilityType.other,"特殊"}, {AbilityData.AbilityType.attack,"攻撃"},{AbilityData.AbilityType.heal,"回復"},
    {AbilityData.AbilityType.buff,"強化"},{AbilityData.AbilityType.debuff,"弱体化"},{AbilityData.AbilityType.summon,"召喚"}
    ,{AbilityData.AbilityType.pass,"パス"}
};
    public static Dictionary<CharacterData.CharacterTag, string> CharacterTagName = new Dictionary<CharacterData.CharacterTag, string>(){
        {CharacterData.CharacterTag.other,"特殊" },{CharacterData.CharacterTag.corpse,"死体" },{CharacterData.CharacterTag.human,"人間" },{CharacterData.CharacterTag.beast,"獣"  }
        ,{CharacterData.CharacterTag.insect,"虫"  },{CharacterData.CharacterTag.undead,"不死者"  },{CharacterData.CharacterTag.artifact,"人工物"  },{CharacterData.CharacterTag.plant,"植物"  }
        ,{CharacterData.CharacterTag.horror,"異形"  },{CharacterData.CharacterTag.obstacle,"障害物"  },{CharacterData.CharacterTag.demihuman,"亜人"  }
    };
    public static Dictionary<ItemData.Rarity, string> rarityName = new Dictionary<ItemData.Rarity, string>()
    {
        { ItemData.Rarity.common,"コモン"},{ ItemData.Rarity.uncommon,"アンコモン"},{ ItemData.Rarity.rare,"レア"},{ ItemData.Rarity.epic,"エピック"},{ ItemData.Rarity.legendary,"レジェンダリー"}
    };
    public static Dictionary<ItemData.MaterialTag, string> materialTagName = new Dictionary<ItemData.MaterialTag, string>()
    {
        { ItemData.MaterialTag.other,"その他"},{ ItemData.MaterialTag.valuables,"貴重品"},{ ItemData.MaterialTag.slay,"討伐"},{ ItemData.MaterialTag.ore,"採掘"},{ ItemData.MaterialTag.food,"食料"}
        ,{ ItemData.MaterialTag.plant,"植物"},{ ItemData.MaterialTag.processed,"加工品"}
    };
    public static Dictionary<ItemData.EquipmentTag, string> equipmentTagName = new Dictionary<ItemData.EquipmentTag, string>()
    {
        { ItemData.EquipmentTag.none,"その他"},{ ItemData.EquipmentTag.weapon,"武器"},{ ItemData.EquipmentTag.armor,"防具"}
    };
    public static Dictionary<PA_StatusEffect.StatusEffectStatus.StatusEffectType, string> StETypeName = new Dictionary<PA_StatusEffect.StatusEffectStatus.StatusEffectType, string>()
    {
        {PA_StatusEffect.StatusEffectStatus.StatusEffectType.neutral,"その他" },{PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff,"バフ" }
        ,{PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff,"デバフ" },{PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus,"フォーカス" }
        ,{PA_StatusEffect.StatusEffectStatus.StatusEffectType.unique,"固有" },{PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT,"ダメージ" }
    };

    [System.Serializable]
    public struct Item
    {
        public int amount;
        /// <summary>[装備品、道具] 生成したPassiveAbilityがアタッチされたオブジェクト</summary>
        public GameObject createdManager;
        public ItemData data;
        public void Init(ItemData d)
        {
            data = d;
        }
        public string GetInfo()
        {
            string s = "";

            if (!data.specialInfo)
            {
                s += string.Format("{0}\n", Definer.rarityName[data.rarity].ColorStr(data.rarity.ToColor()));
                //bool f = false;

                switch (data.itemType)
                {
                    case ItemData.ItemType.material:
                        s += "<<素材>>\n\n";
                        //s += "[";
                        //foreach (ItemData.MaterialTag tag in data.materialTags)
                        //{
                        //    if (f) { s += ", "; }
                        //    f = true;
                        //    s += Definer.materialTagName[tag];
                        //}
                        //s += "]\n";
                        s += "現在素材の使い道は実装されていません...\n".ColorStr(Color.red);
                        s += string.Format("スロットあたりの所持数：{0}\n", data.amountPerStack.ToString());
                        s += string.Format("価値：{0}G\n", data.price.ToString());
                        s += string.Format("スロット単価：{0}G\n", (data.price * data.amountPerStack).ToString());
                        break;


                    case ItemData.ItemType.equipment:
                        s += "<<装備品>>\n";
                        if (data.equipmentTag != ItemData.EquipmentTag.none)
                        {
                            s += string.Format("[{0}]\n", Definer.equipmentTagName[data.equipmentTag]);
                        }
                        s += "\n";
                        s += data.manager.GetComponent<PassiveAbility>().GetPAInfo();
                        break;


                    case ItemData.ItemType.tool:
                        s += "<<道具>>\n\n";
                        s += string.Format("スロットあたりの所持数：{0}\n", data.amountPerStack.ToString());
                        s += data.manager.GetComponent<PassiveAbility>().GetPAInfo();
                        break;
                }
                s += data.info;
            }

            return s;
        }
    }

    private void Awake()
    {
        nonCharaStatus = new Character.CharacterStatus();
        nonCharaStatus.Init(nonCharacterData, -1);

        colorRef = colorRef_Inspector;
        soundRef = soundRef_Inspector;
        VERef = VERef_Inspector;
        abilityManager_General = abilityManager_General_Inspector;
        actionManager_General = actionManager_General_Inspector;
        statusEffectIcon = statusEffectIcon_Inspector;
        positionEffectIcon = positionEffectIcon_Inspector;

        for (int i = 0; i < 5; i++)
        {
            equipments.Add(new List<ItemData>());
        }
        foreach (ItemData equipment in equipmentDataBase)
        {
            equipments[(int)equipment.rarity].Add(equipment);
        }
        //for (int i = 0; i < 5; i++)
        //{
        //    foreach (ItemData equipment in equipments[i])
        //    {
        //        print(equipment.itemName);
        //    }
        //    print("");
        //}
    }
}
