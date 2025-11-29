using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    public override Texture2D RenderStaticPreview
    (
        string assetPath,
        Object[] subAssets,
        int width,
        int height
    )
    {
        var obj = target as CharacterData;
        var sprite = obj.spriteForUI;

        if (sprite == null)
        {
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        var preview = AssetPreview.GetAssetPreview(sprite);
        var final = new Texture2D(width, height);
        if (preview == null) { return base.RenderStaticPreview(assetPath, subAssets, width, height); }
        EditorUtility.CopySerialized(preview, final);

        return final;
    }
}

#endif


[CreateAssetMenu(fileName = "CharaData_", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("内部で参照する際の名前")]
    public string fileName;
    public GameObject manager;

    public string charaName;
    [TextArea(10, 15)] public string introduction;

    [Header("\n\nここからプレイヤーのみ\n")] public string difficulty;
    public bool preferFront;
    public bool preferMid;
    public bool preferBack;
    public List<string> mainRole;
    public List<string> subRole;
    public enum CharacterTag { other, corpse, human, beast, insect, undead, artifact, plant, horror, obstacle, demihuman }
    [Header("ここまでプレイヤーのみ\n\n")] public List<CharacterTag> characterTags;
    //public int size = 1;
    public bool immovable;

    [Header("フィールド効果などの場合はこれをtrueに")]
    public bool notChara;
    public bool player;
    public bool boss;
    public bool playable;
    //public bool obstacle;
    [Header("0:idle 1:damaged")]
    public GameObject[] variableSprites;
    public Vector2 spriteOffset;
    public Sprite spriteForUI;

    public AbilityData[] abilities;

    public List<GameObject> passiveAbilities;
    public List<GameObject> actionMods;

    public CharacterData corpse;
    //public DropItem[] dropItems;
    public LootPanel.LootStatus loot;

    //public EquipmentType[] equipableTypes;
    //[Header("equipableTypesと要素数を合わせる")]
    //public Equipment[] equipments;

    public int lifetime;
    [Header("HPが0の時に攻撃を受けると死亡する")]
    public bool surviveFatalWounds;
    public int maxHP;
    public int maxSAN;

    public int ATK;
    public int INT;

    public float CRITC;
    public float CRITD;

    public float EVD;
    public float ACC;

    public int ACT = 10;
    public int turnPerRound = 1;

    public float GHeal;
    public float RHeal;

    public float debuffRes;

    public List<StEResist> StEResists;
    public List<StEApplyBonus> StEApplyBonus;

    public float moveRes;

    public string GetInfo(bool simple)
    {
        Character.CharacterStatus charaStatus = new Character.CharacterStatus();
        charaStatus.Init(this);

        string info = $"{GetRoleInfo()}\n";

        info += string.Format("使用難易度：{0}\n", charaStatus.characterData.difficulty);       
        info += $"得意な列：{GetPreferedPos()}列\n\n";

        info += string.Format("\n\"{0}\"\n\n", charaStatus.characterData.introduction).ColorStr(Definer.colorRef.emphasize);

        info += charaStatus.GetInfo();
        info += "\n◇◇特性◇◇\n";
        foreach (GameObject obj in charaStatus.passiveAbilities)
        {
            PassiveAbility pa = obj.GetComponent<PassiveAbility>();
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo(simple));
        }

        return info;
    }
    public string GetPreferedPos()
    {
        string prefer = "";
        if (preferFront) prefer += "前";
        if (preferMid) prefer += prefer == "" ? "中" : ",中";
        if (preferBack) prefer += prefer == "" ? "後" : ",後";
        return prefer;
    }

    public string GetRoleInfo()
    {
        string role="";
        foreach(string main in mainRole)
        {
            role += $"◎{main}\n";
        }
        foreach (string sub in subRole)
        {
            role += $"○{sub}\n".ColorStr(Color.gray);
        }
        return role;
    }
}
[System.Serializable]
public class StEResist
{
    public GameObject ResStE;
    public float value;
}
[System.Serializable]
public class StEApplyBonus
{
    public GameObject applyStE;
    public float exChance;
    public int exStack;
    public int exValue;
    public int exDMGPerTurn;

    public void AddBonus(StEApplyBonus bonus, bool add = true)
    {
        int n = 1;
        if (!add) { n = -1; }
        exChance += bonus.exChance * n;
       exStack += bonus.exStack * n;
        exValue += bonus.exValue * n;
        exDMGPerTurn += bonus.exDMGPerTurn * n;
        //exChance += bonus.exChance * n;
        //exStack += bonus.exStack * n;
        //exValue += bonus.exValue * n;
    }

    public StEApplyBonus(GameObject StE)
    {
        this.applyStE = StE;
        this.exChance = 0;
        this.exStack = 0;
        this.exValue = 0;
    }

    public StEApplyBonus(StEApplyBonus copy)
    {
        this.applyStE = copy.applyStE;
        this.exChance = copy.exChance;
        this.exStack = copy.exStack;
        this.exValue = copy.exValue;
    }

    public string GetInfo()
    {
        string s = "";
        string StEName = GetPA().GetStatusEffectStatus().ToLinkKey();
        if (exChance != 0) { s += ValueToStr(string.Format("・{0}付与確率", StEName), exChance, "％",s); }
        if (exStack != 0) { s += ValueToStr(string.Format("・{0}付与スタック数", StEName), exStack, "", s); }
        //if (bonus.exValue != 0) { s += ValueToStr(string.Format("付与する{0}の<color=#FFBF69><i>{効果量}</i></color>", StEName), bonus.exValue, ""); }
        if (exValue != 0) { s += ValueToStr($"・付与する{StEName}の<color=#FFBF69><i>{{効果量}}</i></color>", exValue, "", s); }

        return s;
    }

    public string ValueToStr(string start, float value, string end,string prevStr)
    {
        if (value == 0) { return ""; }
        string s = prevStr == "" ? "" : "\n";
        s += start;
        if (value < 0) { s += value.ToString().ColorStr(Color.red); }
        else { s += ("+" + value.ToString()).ColorStr(Color.green); }
        s += end;
        return s;
    }

    public  void Log(string note)
    {
        string log = "";
        if (note != "") { log += $"<{note}>\n"; }
        note += applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName + "\n";
        note += $"exChance:{exChance}\n";
        note += $"exStack:{exStack}\n";
        note += $"exValue:{exValue}\n";
        Debug.Log(note);
    }

    public PA_StatusEffect GetPA() { return applyStE.GetComponent<PA_StatusEffect>(); }
}

[System.Serializable]
public class StatusGrowth
{
    [Header("乗算値")] public float maxHP_mul;
    [Header("乗算値")] public float ATKINT_mul;
    [Header("実数値")] public float ACT;
    [Header("実数値")] public float EVD;
    [Header("実数値")] public float ACC;

    public int CalcGrowth(int currentLVL, float value)
    {
        int currentGrowth = Mathf.FloorToInt((currentLVL - 1) * value);
        int nextGrowth = Mathf.FloorToInt((currentLVL + 1 - 1) * value);
        return nextGrowth - currentGrowth;
    }

    public string GetInfo(int LVL)
    {
        string s = "";
        StatusMod_ByLVL next = GetStatusMod(LVL);
        s += $"LVL {LVL - 1} -> {LVL}\n";
        s += ValueToStr("基礎maxHP：", next.maxHP_mul, "％");
        s += ValueToStr("基礎ATK・INT：", next.ATKINT_mul, "％");
        s += ValueToStr("ACT：", next.ACT, "");
        s += ValueToStr("EVD：", next.EVD, "");
        s += ValueToStr("ACC：", next.ACC, "");

        return s;
    }

    public string GetLVLUPInfo(int nextLVL, bool player,int baseHP,int baseATK,int baseINT)
    {
        string s = "";
        StatusMod_ByLVL current = GetStatusMod(nextLVL - 1);
        StatusMod_ByLVL next = GetStatusMod(nextLVL);
        current.SetStatus(baseHP, baseATK, baseINT);
        next.SetStatus(baseHP, baseATK, baseINT);

        s += ValueToStr($"基礎{"maxHP".ToSpr_withName()}", next.maxHP - current.maxHP, "");
        s += ValueToStr($"基礎{"ATK".ToSpr_withLink()}", next.ATK - current.ATK,"");
        s += ValueToStr($"基礎{"INT".ToSpr_withLink()}", next.INT - current.INT, "");
        s += ValueToStr("ACT".ToSpr_withLink(), next.ACT - current.ACT, "");
        s += ValueToStr("EVD".ToSpr_withLink(), next.EVD - current.EVD, "");
        s += ValueToStr("ACC".ToSpr_withLink(), next.ACC - current.ACC, "");

        if (player)
        {
            //List<int> unlockEqSlotLVL = new List<int> { 4, 6, 8, 10 };
            if (GameManager.gameParams.unlockEqSlotLVL.Contains(nextLVL))
            {
                s += "装備品スロット+1\n";
            }
        }

        return s;
    }

    public string ValueToStr(string start, float value, string end)
    {
        if (value == 0) { return ""; }
        string s = start;
        if (value < 0) { s += value.ToString(); }
        else { s += "+" + value.ToString(); }
        s += end + "\n";
        return s;
    }

    public StatusMod_ByLVL GetStatusMod(int LVL)
    {
        StatusMod_ByLVL statusMod = new StatusMod_ByLVL();

        statusMod.ACT = Mathf.FloorToInt(ACT * LVL);
        statusMod.EVD = Mathf.FloorToInt(EVD * LVL);
        statusMod.ACC = Mathf.FloorToInt(ACC * LVL);

        if (LVL <= 1) { return statusMod; }

        statusMod.maxHP_mul = Mathf.CeilToInt(maxHP_mul);
        statusMod.ATKINT_mul = Mathf.CeilToInt(ATKINT_mul);

        for (int i = 2; i < LVL; i++)
        {
            statusMod.maxHP_mul = Mathf.CeilToInt((100 + statusMod.maxHP_mul) * ((maxHP_mul + 100) / 100f)) - 100;
            statusMod.ATKINT_mul = Mathf.CeilToInt((100 + statusMod.ATKINT_mul) * ((ATKINT_mul + 100) / 100f)) - 100;
        }


        return statusMod;
    }
}

public class StatusMod_ByLVL
{
    public int maxHP_mul;
    public int ATKINT_mul;

    public int ACT;
    public int EVD;
    public int ACC;

    public int maxHP;
    public int ATK;
    public int INT;

    public void SetStatus(int baseHP, int baseATK, int baseINT)
    {
        maxHP = Mathf.FloorToInt(baseHP * (100 + maxHP_mul) / 100f) - baseHP;
        ATK = Mathf.FloorToInt(baseATK * (100 + ATKINT_mul) / 100f) - baseATK;
        INT = Mathf.FloorToInt(baseINT * (100 + ATKINT_mul) / 100f) - baseINT;
    }

    public void DeltaMode(StatusMod_ByLVL prev)
    {
        maxHP -= prev.maxHP;
        ATK -= prev.ATK;
        INT -= prev.INT;
        ACT -= prev.ACT;
        EVD -= prev.EVD;
        ACC -= prev.ACC;
    }
}