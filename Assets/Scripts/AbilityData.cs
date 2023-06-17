using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "A", menuName = "ScriptableObjects/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;

    public GameObject abilityManager;

    public bool dontChangeSprite;
    [Header("スプライトの直接指定")]
    public GameObject activateSprite;
    [Header("汎用スプライトの番号")]
    public int spriteIndex;

    public enum AbilityType { other, attack, heal, buff, debuff, summon }
    public AbilityType abilityType;
    //public enum TargetType { other, single, all, self, row, column, withoutSelf, random }
    //public TargetType targetType;
    //public bool targetPlayerSide;
    //public bool targetEnemySide;
    //public bool targetEmpty;
    //public bool selectableFront;
    //public bool selectableMid;
    //public bool selectableBack;
    //[Header("味方を対象とするアビリティはignoreHideにチェック!!")]
    //public bool ignoreHide;

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
