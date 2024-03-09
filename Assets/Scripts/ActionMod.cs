using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMod : MonoBehaviour
{
    
    [System.Serializable]
    public struct ActionModStatus
    {
        public int decreaseHP;

        public bool cantCounter;

        public float ATKMod;
        public float exDMG_mul;
        public int exDMG_int;
        public float ACCMod;
        public float CRITCMod;
        public float CRITDMod;
        public bool sureHit;
        public bool unevadable;

        public int healValue;
        public float healPercent;

        public int SANHeal;
        public int SANDamage;
        public int shieldAdd;
        public int shieldRemove;

        //public PA_StatusEffect.StatusEffectParams[] applySteParams;

        //public float moveChance;
        //public int moveForword;
        //public int moveUpper;
        //public int moveLower;
        //public int moveBackword;
    }
    [SerializeField]
    protected ActionModStatus actionModStatus;
    
    public virtual Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.actionTargets.Count != actionsStatus.Length) { FindObjectOfType<InfoText>().AddErrorText("‘ÎÛ‚Ì”‚Æs“®“à—e‚Ì”‚ªˆê’v‚µ‚Ü‚¹‚ñ"); }
       
        return actionsStatus;
    }
}
