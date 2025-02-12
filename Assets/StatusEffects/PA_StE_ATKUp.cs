using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_ATKUp : PA_StatusEffect
{
    public override void OnPAInit()
    {
        character.AddATK(0, StEStatus.value);
    }
    //public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    //{
    //    bool f = false;
    //    foreach (Action.OnAttackParams attackParams in onAttackParamsList)
    //    {
    //        if (attackParams.actionStatus.ATKMod_max > 0)
    //        {
    //            f = true;
    //            break;
    //        }
    //    }
    //    if(f) AddStack(-1);
    //}

    public override void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (myTurn && applyFlag) { AddStack(-1); }
    }

    public override void AtTheEnd()
    {
        character.AddATK(0, StEStatus.value * -1);
    }
}
