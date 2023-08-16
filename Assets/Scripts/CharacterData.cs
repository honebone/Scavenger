using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharaData_", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("内部で参照する際の名前")]
    public string fileName;
    public GameObject manager;

    public string charaName;
    public enum CharacterTag { other, corpse, human, beast, insect, undead, artifact, plant, horror }
    public List<CharacterTag> characterTags;
    public int size = 1;
    public bool immovable;

    public bool player;
    public bool playable;
    [Header("0:idle 1:damaged")]
    public GameObject[] variableSprites; 
    public Sprite spriteForUI;

    public AbilityData[] abilities;

    public List<GameObject> passiveAbilities;
    public List<GameObject> actionMods;

    //public EquipmentType[] equipableTypes;
    //[Header("equipableTypesと要素数を合わせる")]
    //public Equipment[] equipments;

    [Header("HPが0の時に攻撃を受けると死亡する")]
    public bool surviveFatalWounds;
    public int maxHP;
    public int maxSAN;

    public int ATK;

    public float CRITC;
    public float CRITD;

    public float EVD;
    public float ACC;

    public int ACT = 5;
    public int turnPerRound = 1;

    public float GHeal = 100f;
    public float RHeal = 100f;

    //counterAction

    //public DropItem[] dropItems;
    public string leftBehind;//死亡時に変身するキャラクター名

    public float debuffRes;

    public float stunRes;
    public float bleedRes;
    public float poisonRes;
    public float burnRes;

    public float moveRes;
    


}
