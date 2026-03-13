using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_StormBow : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int ModPerAttack;
    int ACCMod;
   
    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        foreach (Action.OnAttackParams onAttackParams in onAttackParamsList)
        {
            if (onAttackParams.hit)
            {
                if (onAttackParams.actionParams.actionStatus.abilityEffect || onAttackParams.actionParams.actionStatus.source == this)
                {
                    Action.ActionStatus action = actionStatus;
                    action.ACCMod += ACCMod;
                    List<Character> target = charactersManager.SearchCharaWithCondition(condition);

                   if(target.Count>0) Enqueue(action, true, target, 1);
                    {
                        ACCMod -= ModPerAttack;

                    }
                    break;
                }
            }
           
        }
    }

    public override void OnTurnEnd(TurnEndParams tep)
    {
        ACCMod = 0;
    }

    public override void OnBattleEnd()
    {
        ACCMod = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
}
