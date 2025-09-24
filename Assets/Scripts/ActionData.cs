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

    public GameObject VE_OnTargets;

    public GameObject actionObject;
    public List<GameObject> actionMods;

    public bool kill;

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
    public int shieldPercent_min;
    public int shieldPercent_max;
    public bool shieldRemove_all;
    public int shieldRemove_min;
    public int shieldRemove_max;
    [Header("ApplyStE")]
    public List<PA_StatusEffect.StatusEffectParams> applyStEParams;
    [Header("ApplyPE")]
    public List<PositionEffect.PositionEffectParams> applyPEParams;

    [System.Serializable]
    public class RemoveStE
    {
        /// <summary>消去能力の時はプレハブ StE内から呼ばれるときは自身</summary>
        public GameObject removeStE;

        public bool removeAll;
        [Header("減らすときは-つけろ!!")]public int addAmount;
    }
    [Header("RemoveStE")]
    public List<RemoveStE> removeStEs;

    [System.Serializable]
    public class RemovePE
    {
        /// <summary>消去能力の時はプレハブ StE内から呼ばれるときは自身</summary>
        public GameObject removePE;

        public bool removeAll;
        [Header("減らすときは-つけろ!!")] public int addAmount;
    }
    [Header("RemoveStE")]
    public List<RemovePE> removePEs;

    public bool summon;
    //public int summonSize;
    public CharacterData[] summonChara;
    public float[] summonChanceWeight;

    [System.Serializable]
    public struct AbilityRemainControll
    {
        public AbilityData abilityData;

        [Header("true:使用回数をvalueにする false:使用回数にvalueを足す")]
        public bool set;
        public int value;

        public bool set_CD;
        public int value_CD;
    }
    public List<AbilityRemainControll> abilityRemainControlls;

    public float moveChance;
    public int moveUpper;
    public int moveLower;
    public int moveForword;
    public int moveBackword;
}
