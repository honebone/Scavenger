using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string difficulty;
    [TextArea(3, 10)] public string introduction;
    [TextArea(3, 10)] public string preferredPos;
    public enum CharacterTag { other, corpse, human, beast, insect, undead, artifact, plant, horror, obstacle, demihuman }
    public List<CharacterTag> characterTags;
    //public int size = 1;
    public bool immovable;

    [Header("フィールド効果などの場合はこれをtrueに")]
    public bool notChara;
    public bool player;
    public bool playable;
    /// <summary>勝敗に関係ないか</summary>
    public bool obstacle;
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

    public float GHeal = 100f;
    public float RHeal = 100f;

    public StatusGrowth statusGrowth;

    public float debuffRes;

    public List<StEResist> StEResists;
    public List<StEApplyBonus> StEApplyBonus;

    public float moveRes;

    public string GetInfo()
    {
        Character.CharacterStatus charaStatus = new Character.CharacterStatus();
        charaStatus.Init(this, 0);

        string info = string.Format("\n\"{0}\"\n\n", charaStatus.characterData.introduction).ColorStr(Definer.colorRef.emphasize);
        info += string.Format("使用難易度：{0}\n得意なポジション：{1}\n\n", charaStatus.characterData.difficulty, charaStatus.characterData.preferredPos);
        info += charaStatus.GetInfo();
        info += "\n◇◇特性◇◇\n";
        foreach (GameObject obj in charaStatus.passiveAbilities)
        {
            PassiveAbility pa = obj.GetComponent<PassiveAbility>();
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo());
        }

        return info;
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

    public void AddBonus(StEApplyBonus bonus, bool add = true)
    {
        int n = 1;
        if (!add) { n = -1; }
        exChance += bonus.exChance * n;
       exStack += bonus.exStack * n;
        exValue += bonus.exValue * n;
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
}

[System.Serializable]
public class StatusGrowth
{
    public float maxHP;
    public float ATK;
    public float INT;
    public float ACT;
    public float CRITC;
    public float CRITD;

    public int CalcGrowth(int currentLVL, float value)
    {
        int currentGrowth = Mathf.FloorToInt((currentLVL - 1) * value);
        int nextGrowth = Mathf.FloorToInt((currentLVL + 1 - 1) * value);
        return nextGrowth - currentGrowth;
    }

    public string GetInfo(int LVL)
    {
        string s = "";
        //s += ValueToStr("基礎HP", CalcGrowth(LVL,maxHP), "");
        //s += ValueToStr("基礎ATK", CalcGrowth(LVL, ATK), "");
        //s += ValueToStr("基礎INT", CalcGrowth(LVL, INT), "");
        s += ValueToStr("CRIT率", CalcGrowth(LVL, CRITC), "％");
        s += ValueToStr("CRITダメージ", CalcGrowth(LVL, CRITD), "％");
        s += ValueToStr("ACT", CalcGrowth(LVL, ACT), "");
        List<int> unlockEqSlotLVL = new List<int> { 4, 6, 8, 10 };
        if (unlockEqSlotLVL.Contains(LVL + 1))
        {
            s += "装備品スロット+1\n";
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
}