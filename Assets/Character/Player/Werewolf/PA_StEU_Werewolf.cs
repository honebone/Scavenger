using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StEU_Werewolf : PA_StatusEffect
{
    [SerializeField] float exDMGPerStack;
    [SerializeField] float PROTPerStack;
    [SerializeField] Action.ActionStatus onTurnEnd;

    public override void OnPAInit()
    {
        character.AddExDMG_Mul(StEStatus.stack * exDMGPerStack);
        character.AddPROT(StEStatus.stack * PROTPerStack);
    }

    public override void OnAddStack(int add)
    {
        character.AddExDMG_Mul(add * exDMGPerStack);
        character.AddPROT(add * PROTPerStack);
    }


    public override void AtTheEnd()
    {
        character.AddExDMG_Mul(StEStatus.stack * exDMGPerStack * -1);
        character.AddPROT(StEStatus.stack * PROTPerStack * -1);
    }

    public override void OnTurnEnd()
    {
        Enqueue_Self(onTurnEnd);
    }

    public override string GetAdditionalInfo()
    {
        return onTurnEnd.GetInfo(false, new Character.CharacterStatus());
    }
}
