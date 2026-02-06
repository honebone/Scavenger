using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Definer : MonoBehaviour
{
    public static Definer inst;

    [System.Serializable]
    public class ColorRef
    {
        public Color debug;
        public Color player;
        public Color enemy;
        /// <summary>0:other 1:attack 2:heal 3:buff 4:debuff 5:summon</summary>
        public Color[] abilityColors;
        public Color[] personalityColors;
        /// <summary>0:other 1:buff 2:debuff 3:focus</summary>
        public Color[] statusEffectColors;
        /// <summary>0:other 1:buff 2:debuff 3:focus</summary>
        public Color[] positionEffectColors;
        /// <summary>0:common 1:uncommon 2:rare 3:epic 4:legendary</summary>
        public Color[] rarityColors;
        /// <summary>0:negative 1:common 2:rare 3:epic 4:legendary</summary>
        public Color[] RaERarityColors;

        public Color affricted;

        public Color decreaseHP;
        public Color damage;
        public Color INTDamage;
        public Color CRIT;
        public Color ACC;
        public Color evade;
        public Color ACT;
        public Color heal;
        public Color shield;
        public Color shieldDecrease;
        public Color SANHeal;
        public Color SANDecrease;
        public Color echo;

        public Color failed_unavailable;
        public Color currentState;
        public Color emphasize;

        public Color AMod;
        public Color expOrb;
        public Color coin;
    }
    [System.Serializable]
    public class SoundRef
    {
        public AudioClip miss;
        public AudioClip evade;
        public AudioClip damage;
        public AudioClip shieldDMG;
        public AudioClip CRIT;
        public AudioClip dying;
        public AudioClip heal;
        public AudioClip shield;
        public AudioClip SANHeal;
        public AudioClip SANDecrease;
        public AudioClip[] ApplyStE;
        public AudioClip consumeFocus;
        public AudioClip summoned;
        public AudioClip stun;
        public AudioClip die1;
        public AudioClip die2;
        public AudioClip[] getItem;

        public AudioClip expOrb;
        public AudioClip coin;

        public AudioClip select;
        public AudioClip mouseover;
        public AudioClip info;
    }
    [System.Serializable]
    public class VisualEffectRef
    {
        public GameObject die;
        public GameObject damage;
        public GameObject CRIT;
        public GameObject heal;
        /// <summary>0:other 1:buff 2:debuff 3:focus</summary>
        public GameObject[] applyStE;
        public GameObject shieldDMG;

        public GameObject UI_smoke;
    }

    public static Character.CharacterStatus nonCharaStatus;
    public static Action.ActionStatus actionRef;
    public static ColorRef colorRef;
    public static SoundRef soundRef;
    public static VisualEffectRef VERef;
    public static GameObject abilityManager_General;
    public static GameObject actionManager_General;
    public static GameObject statusEffectIcon;
    public static GameObject positionEffectIcon;
    [SerializeField] CharacterData nonCharacterData;
    [SerializeField] Action.ActionStatus actionRef_Inspector;
    //[SerializeField]
    //ColorRef colorRef_Inspector;
    //[SerializeField]
    //SoundRef soundRef_Inspector;
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
    [SerializeField] List<GameObject> generalRaEDataBase_Inspector;

    //[SerializeField] List<CharacterData> playerDataBase;
    //[SerializeField] List<CharacterData> enemyDataBase;
    //[SerializeField] List<CharacterData> obstacleDataBase;
    //[SerializeField] List<CharacterData> summonDataBase;
    [SerializeField] List<ItemData> lootDataBase_Inspector;
    [SerializeField] List<ItemData> equipmentDataBase;
    [SerializeField] List<ItemData> eqDataBase_excludeRandomPool;
    [SerializeField] List<GameObject> personalityDataBase;
    public List<GameObject> mutationDataBase;
    [SerializeField] List<GameObject> affrictionDataBase;
    //[SerializeField] List<GameObject> statusEffectDataBase;
    [SerializeField] List<GameObject> positionEffectDataBase;
    

    public static List<GameObject> DoTDataBase = new List<GameObject>();
    public List<CharacterData> GetPlayerDataBase() { return cp.playerDataBase; }
    public List<ItemData> GetAllLoots() { return lootDataBase_Inspector; }

    /// <summary>ランダムプール外も含むので注意！</summary>
    public List<ItemData> GetAllEquipments()
    {
        List<ItemData> list = new List<ItemData>(equipmentDataBase);
        list.AddRange(eqDataBase_excludeRandomPool);
        return list;
    }
    public List<GameObject> GetPersonalityDataBase() { return personalityDataBase; }
    public List<GameObject> GetAffrictionDataBase() { return affrictionDataBase; }

    [System.Serializable]
    public class UniqueLinkInfo
    {
        public string linkKey;
        public string linkName;
       [TextArea(3,10)] public string linkInfo;
    }

    public CommonParams cp;
    public List<UniqueLinkInfo> uniqueLinkInfos = new List<UniqueLinkInfo>();

    /// <summary>埋め込んだリンク -> 説明文</summary>
    public List<string> LinkKeyToStr(string key)
    {
        string s = "";
        switch (key[0])//keyの頭文字がタグの役割を果たす
        {
            case 'S'://状態異常
                foreach (GameObject StE in cp.statusEffectDataBase)
                {
                    PA_StatusEffect PA = StE.GetComponent<PA_StatusEffect>();
                    PA_StatusEffect.StatusEffectStatus status = PA.GetStatusEffectStatus();
                    if ($"S_{status.StEName}" == key)
                    {
                        s += PA.GetInfo_ForLink(InfoText.inst.IsSimple()) + "\n";
                        //s += StE.GetComponent<PassiveAbility>().GetPAInfo() + "\n";
                        if (status.maxStack > 0) { s += string.Format("(最大{0}スタック)\n", status.maxStack).ColorStr(Color.gray); }
                        return new List<string> { s };
                    }
                }
                Debug.Log("error:状態異常が見つかりませんでした");
                return new List<string> { "error:状態異常が見つかりませんでした" };



            case 'P'://ポジション効果
                foreach (GameObject PE in positionEffectDataBase)
                {
                    PositionEffect PA = PE.GetComponent<PositionEffect>();
                    PositionEffect.PositionEffectStatus status = PA.GetPositionEffectStatus();
                    if ($"P_{status.PEName}" == key)
                    {
                        s += PA.GetInfo_ForLink() + "\n";
                        if (status.maxStack > 0) { s += string.Format("(最大{0}スタック)\n", status.maxStack).ColorStr(Color.gray); }
                        return new List<string> { s };
                    }
                }
                Debug.Log("error:ポジション効果が見つかりませんでした");
                return new List<string>{ "error:ポジション効果が見つかりませんでした" };



            case 'C'://キャラ
                List<CharacterData> database;
                switch (key[2])
                {
                    case 'P':
                        database= new List<CharacterData>(cp.playerDataBase);
                        break; 
                    case 'E':
                        database= new List<CharacterData>(cp.enemyDataBase);
                        break; 
                    case 'S':
                        database= new List<CharacterData>(cp.summonDataBase);
                        break;
                    case 'O':
                        database = new List<CharacterData>(cp.obstacleDataBase);
                        break;
                    default:
                        Debug.Log($"error:キャラの分類が不適切です：{key[2]}");
                        return new List<string>{ $"error:キャラの分類が不適切です：{key[2]}" };
                }
                foreach (CharacterData data in database)
                {
                    if ($"C_{data.fileName}" == key)
                    {
                        Character.CharacterStatus status = new Character.CharacterStatus();
                        status.Init(data);
                        string info = "";
                        string sub = "";
                        if (data.introduction != "") { info += $"\"{data.introduction}\"\n\n".ColorStr(colorRef.emphasize); }
                        info += status.GetInfo();
                        sub += "\n◇◇特性◇◇\n";
                        foreach (GameObject obj in status.passiveAbilities)
                        {
                            PassiveAbility pa = obj.GetComponent<PassiveAbility>();
                            sub += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo(InfoText.inst.IsSimple()));
                        }
                        return new List<string> { info, sub };
                    }
                }
                Debug.Log($"error:キャラが見つかりませんでした：{key}");
                return new List<string> { $"error:キャラが見つかりませんでした：{key}" };

            case 'T':
                foreach (var info in cp.textSpriteParamsList)
                {
                    if ($"T_{info.key}" == key)
                    {
                        return new List<string> { info.GetInfo() };
                    }
                }
                Debug.Log("error:keyに合う説明文が見つかりませんでした");
                return new List<string> { "error:keyに合う説明文が見つかりませんでした" };


            case 'U'://その他
                foreach(UniqueLinkInfo linkInfo in uniqueLinkInfos)
                {
                    if ($"U_{linkInfo.linkKey}" == key)
                    {
                        if(linkInfo.linkName=="") return new List<string> { $"<{linkInfo.linkKey}>\n{linkInfo.linkInfo}" };
                        else return new List<string> { $"<{linkInfo.linkName}>\n{linkInfo.linkInfo}" };
                    }
                }
                Debug.Log("error:keyに合う説明文が見つかりませんでした");
                return new List<string> { "error:keyに合う説明文が見つかりませんでした" };
            default:
                Debug.Log("error:頭文字のタグが一致しません");
                return new List<string> { "error:頭文字のタグが一致しません" };
        }
    }

    public string GetTS_withName(string key, string nameOverride = null ,bool outline=false)
    {
        foreach (var ts in cp.textSpriteParamsList)
        {
            if (ts.key == key)
            {
                return ts.GetTextSprite(SpriteTextMode.withName, outline, nameOverride);
            }
        }
        Debug.Log("error:keyが不一致");
        return "error:keyが不一致";
    }

    public string GetTS_withLink(string key, string nameOverride = null, bool outline = false)
    {
        foreach (var ts in cp.textSpriteParamsList)
        {
            if (ts.key == key)
            {
                return ts.GetTextSprite(SpriteTextMode.withLink, outline, nameOverride);
            }
        }
        Debug.Log("error:keyが不一致");
        return "error:keyが不一致";
    }

    public static List<ItemData> lootDataBase = new List<ItemData>();
    public static List<List<ItemData>> equipments = new List<List<ItemData>>();
    public static List<GameObject> generalRaEDataBase;
    public static List<List<GameObject>> RaEDataBase = new List<List<GameObject>>();
    public static List<List<GameObject>> personalities = new List<List<GameObject>>();


    public static Dictionary<AbilityData.AbilityType, string> AbiltyTypeName = new Dictionary<AbilityData.AbilityType, string>(){
    {AbilityData.AbilityType.other,"特殊"}, {AbilityData.AbilityType.attack,"攻撃"},{AbilityData.AbilityType.heal,"回復"},
    {AbilityData.AbilityType.buff,"強化"},{AbilityData.AbilityType.debuff,"弱体化"},{AbilityData.AbilityType.summon,"召喚"}
    ,{AbilityData.AbilityType.pass,"パス"},{AbilityData.AbilityType.move,"移動"}
};
    public static Dictionary<CharacterData.CharacterTag, string> CharacterTagName = new Dictionary<CharacterData.CharacterTag, string>(){
        {CharacterData.CharacterTag.other,"特殊" },{CharacterData.CharacterTag.corpse,"死体" },{CharacterData.CharacterTag.human,"人間" },{CharacterData.CharacterTag.beast,"獣"  }
        ,{CharacterData.CharacterTag.insect,"虫"  },{CharacterData.CharacterTag.undead,"不死者"  },{CharacterData.CharacterTag.artifact,"人工物"  },{CharacterData.CharacterTag.plant,"植物"  }
        ,{CharacterData.CharacterTag.horror,"異形"  },{CharacterData.CharacterTag.obstacle,"障害物"  },{CharacterData.CharacterTag.demihuman,"亜人"  }
    };
    public static Dictionary<ItemData.Rarity, string> rarityName = new Dictionary<ItemData.Rarity, string>()
    {
        { ItemData.Rarity.common,"コモン"},{ ItemData.Rarity.uncommon,"アンコモン"},{ ItemData.Rarity.rare,"レア"},{ ItemData.Rarity.epic,"エピック"},{ ItemData.Rarity.legendary,"レジェンダリー"}
        ,{ ItemData.Rarity.madness,"狂気"}
    };

    public static Dictionary<ItemData.MaterialTag, string> materialTagName = new Dictionary<ItemData.MaterialTag, string>()
    {
        { ItemData.MaterialTag.other,"その他"},{ ItemData.MaterialTag.valuables,"貴重品"},{ ItemData.MaterialTag.slay,"討伐"},{ ItemData.MaterialTag.ore,"採掘"},{ ItemData.MaterialTag.food,"食料"}
        ,{ ItemData.MaterialTag.plant,"植物"},{ ItemData.MaterialTag.processed,"加工品"},{ ItemData.MaterialTag.junk,"ガラクタ"},{ ItemData.MaterialTag.sundries,"雑貨"},{ ItemData.MaterialTag.book," 本"}
        ,{ ItemData.MaterialTag.holy,"神聖"},{ ItemData.MaterialTag.weapon,"軍事"},{ ItemData.MaterialTag.crops,"作物"}
    };
    //public static Dictionary<ItemData.EquipmentTag, string> equipmentTagName = new Dictionary<ItemData.EquipmentTag, string>()
    //{
    //    { ItemData.EquipmentTag.none,"その他"},{ ItemData.EquipmentTag.weapon,"武器"},{ ItemData.EquipmentTag.armor,"防具"}
    //};
    public static Dictionary<PA_StatusEffect.StatusEffectStatus.StatusEffectType, string> StETypeName = new Dictionary<PA_StatusEffect.StatusEffectStatus.StatusEffectType, string>()
    {
        {PA_StatusEffect.StatusEffectStatus.StatusEffectType.neutral,"その他" },{PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff,"バフ" }
        ,{PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff,"デバフ" },{PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus,"フォーカス" }
        ,{PA_StatusEffect.StatusEffectStatus.StatusEffectType.unique,"固有" }/*,{PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT,"ダメージ" }*/
    };
    public static Dictionary<PA_Personality.PersonalityStatus.PersonalityType, string> PerTypeName = new Dictionary<PA_Personality.PersonalityStatus.PersonalityType, string>()
    {
        {PA_Personality.PersonalityStatus.PersonalityType.neutral,"特殊" },{PA_Personality.PersonalityStatus.PersonalityType.unique,"固有" },{PA_Personality.PersonalityStatus.PersonalityType.awoken,"覚醒" }
        ,{PA_Personality.PersonalityStatus.PersonalityType.good,"ポジティブ" },{PA_Personality.PersonalityStatus.PersonalityType.bad,"ネガティブ" }
        ,{PA_Personality.PersonalityStatus.PersonalityType.affricted,"精神崩壊" } ,{PA_Personality.PersonalityStatus.PersonalityType.mutation,"変異" }
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
        public string GetInfo(bool simple)
        {
            string s = "";

            if (!data.specialInfo)
            {
                s += string.Format("{0}\n", Definer.rarityName[data.rarity].ColorStr(data.rarity.ToColor()));
                bool f = false;

                switch (data.itemType)
                {
                    case ItemData.ItemType.material:
                        s += "<<素材>>\n\n";
                        s += "タグ：";
                        foreach (ItemData.MaterialTag tag in data.materialTags)
                        {
                            if (f) { s += ", "; }
                            f = true;
                            s += $"[{Definer.materialTagName[tag]}]";
                        }
                        s += "\n";
                        s += "現在素材の使い道は実装されていません...\n".ColorStr(Color.red);
                        s += string.Format("スロットあたりの所持数：{0}\n", data.amountPerStack.ToString());
                        s += string.Format("価値：{0}G\n", data.price.ToString());
                        s += string.Format("スロット単価：{0}G\n", (data.price * data.amountPerStack).ToString());
                        break;


                    case ItemData.ItemType.equipment:
                        s += "<<装備品>>\n";
                        //if (data.equipmentTag != ItemData.EquipmentTag.none)
                        //{
                        //    s += string.Format("[{0}]\n", Definer.equipmentTagName[data.equipmentTag]);
                        //}
                        //s += "\n";
                        s += data.manager.GetComponent<PassiveAbility>().GetPAInfo(simple);
                        break;


                    case ItemData.ItemType.tool:
                        s += "<<道具>>\n\n";
                        s += string.Format("スロットあたりの所持数：{0}\n", data.amountPerStack.ToString());
                        s += data.manager.GetComponent<PassiveAbility>().GetPAInfo();
                        break;
                }
            }

            if (data.info != "") s += $"{Extentions.NL(s, 3)}<i>{data.info}</i>".ColorStr(Color.grey);

            return s;
        }

        public string GetName()
        {
            if (data.manager != null && data.manager.GetComponent<PassiveAbility>()) return data.manager.GetComponent<PassiveAbility>().GetPAName();
            else return $"error:{data.itemName}";
        }
    }

    private void Awake()
    {
        if (inst == null) inst = this;
        nonCharaStatus = new Character.CharacterStatus();
        nonCharaStatus.Init(nonCharacterData);

        actionRef = actionRef_Inspector;

        colorRef = cp.colorRef;
        soundRef = cp.soundRef;
        VERef = VERef_Inspector;
        abilityManager_General = abilityManager_General_Inspector;
        actionManager_General = actionManager_General_Inspector;
        statusEffectIcon = statusEffectIcon_Inspector;
        positionEffectIcon = positionEffectIcon_Inspector;

        lootDataBase = lootDataBase_Inspector;

        for (int i = 0; i < Enum.GetNames(typeof(ItemData.Rarity)).Length; i++)
        {
            equipments.Add(new List<ItemData>());
        }
        foreach (ItemData equipment in equipmentDataBase)
        {
            equipments[(int)equipment.rarity].Add(equipment);
        }

        for (int i = 0; i < 6; i++)
        {
            personalities.Add(new List<GameObject>());
        }
        foreach (GameObject per in personalityDataBase)
        {
            PA_Personality.PersonalityStatus status = per.GetComponent<PA_Personality>().GetPersonalityStatus();
            personalities[(int)status.personalityType].Add(per);
        }

        DoTDataBase = new List<GameObject>();
        foreach (GameObject StE in cp.statusEffectDataBase)
        {
            if (StE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().DoT) { DoTDataBase.Add(StE); }
        }

        generalRaEDataBase = new List<GameObject>(generalRaEDataBase_Inspector);

        for (int i = 0; i < Enum.GetNames(typeof(RE_RandomEvents.Rarity)).Length; i++)
        {
            RaEDataBase.Add(new List<GameObject>());
        }
        foreach(GameObject RaE in cp.RaEDataBase)
        {
            RaEDataBase[(int)RaE.GetComponent<RE_RandomEvents>().GetRarity()].Add(RaE);
        }
        //cp.RaEDataBase.ForEach(r => RaEDataBase[(int)r.GetComponent<RE_RandomEvents>().GetRarity()].Add(r));
    }


}

public enum SpriteTextMode { spriteOnly, withName, withLink }

[System.Serializable]
public class TextSpriteParams
{
    public string key;
    public string defaultName;
    public Color color;
    [TextArea(10, 20)] public string info;
    public string GetTextSprite(SpriteTextMode mode, bool outline, string nameOverride = null)
    {
        switch (mode)
        {
            case SpriteTextMode.spriteOnly:
                return key.ToSpr(outline);
            case SpriteTextMode.withName:
                string n = (nameOverride == null ? defaultName : nameOverride).ColorStr(color);
                return $"{key.ToSpr(outline)}{n}";
            case SpriteTextMode.withLink:
                string n2 = nameOverride == null ? defaultName : nameOverride;
                return $"{key.ToSpr(outline)}<link=T_{key}><u>{n2}</u></link>".ColorStr(color);
            default:
                return "";
        }
    }

    public string GetInfo()
    {
        return $"<{GetTextSprite(SpriteTextMode.withName, false)}>\n{info}";
    }
}
