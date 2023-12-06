using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "A", menuName = "ScriptableObjects/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;

    [Header("設定しなければ汎用的なオブジェクトになる")]
    public GameObject abilityManager;

    public bool dontChangeSprite;
    [Header("スプライトの直接指定")]
    public GameObject activateSprite;
    [Header("汎用スプライトの番号")]
    public int spriteIndex;

    public AudioClip SE;

    public enum AbilityType { other, attack, heal, buff, debuff, summon }
    public AbilityType abilityType;

    [Header("\n操作不可キャラに関係")]
    [Header("ランダム選択から除外するか")]
    public bool excludeRandomPool;
    [Header("選択される重み")]
    public int selectWeight = 1;

    [TextArea(3,10)]
    public string conditionInfo;

    public int cooldownOnUse;
    public bool hasRemain;
    public int remainOnBattleStart;
    public int maxRemain;

    public bool availableFront;
    public bool availableMid;
    public bool availableBack;

    //public ActionData[] actions;
    public Action.ActionStatus[] actionsStaus;

}
