using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Act_", menuName = "ScriptableObjects/ActionData")]

public class ActionData : ScriptableObject
{
    public string actionName;
    [TextArea(3, 10)]
    public string conditionInfo;

    //public bool useGeneralInfo;
    [TextArea(3, 10)]
    public string targetInfo;
    [TextArea(3,10)]
    public string actionInfo;

    public GameObject actionObject;
    public List<GameObject> actionMods;

    public int decreaseHP_min;
    public int decreaseHP_max;

    public bool cantCounter;
    [Header("0:melee 1:ranged 2:magic")]
    public int AttackType;
    public float ATKMod_min;
    public float ATKMod_max;
    public float ACCMod;
    public float CRITCMod;
    public float CRITDMod;
    public bool sureHit;
    public bool unevadable;

    public int healValue_min;
    public int healValue_max;
    public float healPercent_min;
    public float healPercent_max;

    public int SANHeal_min;
    public int SANHeal_max;
    public int SANDamage_min;
    public int SANDamage_max;
    public int shieldAdd_min;
    public int shieldAdd_max;
    public int shieldRemove_min;
    public int shieldRemove_max;
    [Header("ApplyStE")]
    public PA_StatusEffect.StatusEffectParams[] applyStEParams;

    public float moveChance;
    public int moveUpper;
    public int moveLower;
    public int moveForword;
    public int moveBackword;
}
