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
        public AudioClip summoned;
        public AudioClip stun;
    }
    [System.Serializable]
    public class VisualEffectRef
    {
        public GameObject die;
    }

    public static ColorRef colorRef;
    public static SoundRef soundRef;
    public static VisualEffectRef VERef;
    public static GameObject abilityManager_General;
    public static GameObject actionManager_General;
    public static GameObject statusEffectIcon;
    public static GameObject positionEffectIcon;
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


    public static Dictionary<AbilityData.AbilityType, string> AbiltyTypeName = new Dictionary<AbilityData.AbilityType, string>(){
    {AbilityData.AbilityType.other,"特殊"}, {AbilityData.AbilityType.attack,"攻撃"},{AbilityData.AbilityType.heal,"回復"},
    {AbilityData.AbilityType.buff,"強化"},{AbilityData.AbilityType.debuff,"弱体化"},{AbilityData.AbilityType.summon,"召喚"}
};
    public static Dictionary<CharacterData.CharacterTag, string> CharacterTagName = new Dictionary<CharacterData.CharacterTag, string>(){
        {CharacterData.CharacterTag.other,"特殊" },{CharacterData.CharacterTag.corpse,"死体" },{CharacterData.CharacterTag.human,"人間" },{CharacterData.CharacterTag.beast,"獣"  }
        ,{CharacterData.CharacterTag.insect,"虫"  },{CharacterData.CharacterTag.undead,"不死者"  },{CharacterData.CharacterTag.artifact,"人工物"  },{CharacterData.CharacterTag.plant,"植物"  }
        ,{CharacterData.CharacterTag.horror,"異形"  }
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

    [System.Serializable]
    public struct Item
    {
        public ItemData.ItemType itemType;
        public ItemData.MaterialTag[] materialTags;
        public ItemData.TemporaryTag temporaryTag;

        public string itemName;
        [TextArea(3, 10)]
        public string info;
        public int amountPerStack;
        public ItemData.Rarity rarity;
        public Sprite sprite;
        public int price;//基本となる買値

        public bool specialInfo;
        public GameObject manager;

        public int amount;
        public ItemData itemData;
        public void Init(ItemData data)
        {
            itemType = data.itemType;
            materialTags = data.materialTags;
            temporaryTag = data.temporaryTag;
            itemName = data.itemName;
            info = data.info;
            amountPerStack = data.amountPerStack;
            rarity = data.rarity;
            sprite = data.sprite;
            price = data.price;
            manager = data.manager;


            itemData=data;
        }
        public string GetInfo()
        {
            string s = "";

            if (!specialInfo)
            {
                s += string.Format("{0}\n", Definer.rarityName[rarity].ColorStr(rarity.ToColor()));
                bool f = false;
                s += "[";
                foreach (ItemData.MaterialTag tag in materialTags)
                {
                    if (f) { s += ", "; }
                    f = true;
                    s += Definer.materialTagName[tag];
                }
                s += "]\n";
                s += string.Format("スロットあたりの所持数：{0}\n", amountPerStack.ToString());
                s += string.Format("価値：{0}G\n", price.ToString());
                s += string.Format("スロット単価：{0}G\n", (price * amountPerStack).ToString());
                s += info;
            }
           
            return s;
        }
    }

    private void Awake()
    {
        colorRef = colorRef_Inspector;
        soundRef = soundRef_Inspector;
        VERef = VERef_Inspector;
        abilityManager_General = abilityManager_General_Inspector;
        actionManager_General=actionManager_General_Inspector;
        statusEffectIcon = statusEffectIcon_Inspector;
        positionEffectIcon=positionEffectIcon_Inspector;
    }
}
