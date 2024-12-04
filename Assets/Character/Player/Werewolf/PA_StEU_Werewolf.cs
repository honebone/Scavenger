using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Werewolf : PA_StatusEffect
{
    [SerializeField] int baseATK;
    [SerializeField] int ACT;
    [SerializeField] float ATKPerStack;
    [SerializeField] float PROTPerStack;
    [SerializeField] Action.ActionStatus onTurnEnd;
    [SerializeField] ActionMod.ActionModStatus AMod;
    [SerializeField] ActionMod.ActionModStatus AMod2;
    [SerializeField] int drain_ability;
    [SerializeField] int drain_passive;


    [SerializeField] GameObject bleed;
    [SerializeField] Action.ActionStatus onApplyBleed;
    

    public override void OnPAInit()
    {
        character.AddATK(baseATK, 0);
        character.AddACT(ACT);
        character.AddATK(0, StEStatus.stack * ATKPerStack);
        character.AddPROT(StEStatus.stack * PROTPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddATK(0, add * ATKPerStack);
        character.AddPROT(add * PROTPerStack);
    }


    public override void AtTheEnd()
    {
        character.AddATK(-baseATK, 0);
        character.AddACT(-ACT);
        character.AddATK(0, StEStatus.stack * ATKPerStack * -1);
        character.AddPROT(StEStatus.stack * PROTPerStack * -1);
    }

    public override void OnTurnEnd()
    {
        Enqueue_Self(onTurnEnd);
    }

    public override Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef, bool forCalcDMG)
    {
        if (statusRef.DoesHeal() && statusRef.actionOwner != character)
        {
            statusRef=statusRef.Modify(AMod);
        }
        return statusRef;
    }

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.DoesAttack())
        {
            for (int i = 0; i < statusRef.actionTargets.Count; i++)
            {
                ActionMod.ActionModStatus mod = AMod2; ;
                if (statusRef.abilityEffect)
                {
                    mod.drain = drain_ability;
                }
                else
                {
                    mod.drain = drain_passive;
                }
                actionsStatus[i] = actionsStatus[i].Modify(mod);
            }
        }
        return actionsStatus;
    }

    public override void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        foreach(Action.OnApplyStEParams onApplyStEParams in onApplyStEParamsList)
        {
            foreach(PA_StatusEffect.StatusEffectParams StEParams in onApplyStEParams.appliedParams)
            {
                if(StEParams.applyStE == bleed)
                {
                    Enqueue_Self(onApplyBleed);
                    break;
                }
            }
        }
    }

    public override string GetAdditionalInfo()
    {
        string s = "";
        s+= onTurnEnd.GetInfo(false, new Character.CharacterStatus());
        s+= "\n"+onApplyBleed.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
