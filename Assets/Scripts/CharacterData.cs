using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharaData_", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("“à•”‚ÅQÆ‚·‚éÛ‚Ì–¼‘O")]
    public string fileName;
    public GameObject manager;

    public string charaName;
    public enum CharacterTag { other, corpse, human, beast, insect, undead, artifact, plant, horror, obstacle,demihuman }
    public List<CharacterTag> characterTags;
    //public int size = 1;
    public bool immovable;

    [Header("ƒtƒB[ƒ‹ƒhŒø‰Ê‚È‚Ç‚Ìê‡‚Í‚±‚ê‚ğtrue‚É")]
    public bool notChara;
    public bool player;
    public bool playable;
    /// <summary>Ÿ”s‚ÉŠÖŒW‚È‚¢‚©</summary>
    public bool obstacle;
    [Header("0:idle 1:damaged")]
    public GameObject[] variableSprites; 
    public Sprite spriteForUI;

    public AbilityData[] abilities;

    public List<GameObject> passiveAbilities;
    public List<GameObject> actionMods;

    public CharacterData corpse;
    //public DropItem[] dropItems;
    public List<LootPanel.DropItem> dropItems;

    //public EquipmentType[] equipableTypes;
    //[Header("equipableTypes‚Æ—v‘f”‚ğ‡‚í‚¹‚é")]
    //public Equipment[] equipments;

    [Header("HP‚ª0‚Ì‚ÉUŒ‚‚ğó‚¯‚é‚Æ€–S‚·‚é")]
    public bool surviveFatalWounds;
    public int maxHP;
    public int maxSAN;

    public int ATK;

    public float CRITC;
    public float CRITD;

    public float EVD;
    public float ACC;

    public int ACT = 10;
    public int turnPerRound = 1;

    public float GHeal = 100f;
    public float RHeal = 100f;

    //counterAction

    public float debuffRes;

    public List<StEResist> StEResists;
    public List<StEApplyBonus> StEApplyBonus;

    public float moveRes;
    


}
[System.Serializable]
public class DropItem
{
    public ItemObject dropItemData;
    public int quantity;

    public ItemData GetItemData() { return dropItemData.GetItemData(); }
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
}
