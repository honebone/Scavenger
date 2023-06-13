using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "A", menuName = "ScriptableObjects/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;

    public bool dontChangeSprite;
    [Header("スプライトの直接指定")]
    public GameObject activateSprite;
    [Header("汎用スプライトの番号")]
    public int spriteIndex;

    public enum AbilityType { other, attack, heal, buff, debuff, summon }
    public AbilityType abilityType;
    public int cooldownOnUse;
    public bool hasRemain;
    public int remainOnBattleStart;
    public int maxRemain;

    public bool availableFront;
    public bool availableMid;
    public bool availableBack;

    public ActionData[] actions;
}
