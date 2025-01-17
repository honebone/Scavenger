using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PE_Acid : PositionEffect
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] int PROTValue;

    public override void OnTurnStart(Character currentTurnChara, int turnCount)
    {
        if (character != null && character == currentTurnChara)
        {
            Action.ActionStatus action = actionStatus;
            action.decreaseHP_min = PEStatus.DMGPerTurn;
            action.decreaseHP_max = PEStatus.DMGPerTurn;
            Enqueue(action, true, new List<Character>() { character }, 0, true);
        }
    }

    public override void OnRoundEnd()
    {
        AddStack(-1);
    }

    public override void OnPEInit()
    {
        if (character != null)
        {
            ApplyEffect(true);
        }
    }
    public override void OnCharaEnter()
    {
        ApplyEffect(true);
    }

    public override void OnCharaLeave()
    {
        ApplyEffect(false);
    }

    public override void AtTheEnd()
    {
        if (character != null) { ApplyEffect(false); }
    }

    void ApplyEffect(bool set)
    {
        int i = set ? 1 : -1;
        character.AddPROT(PROTValue * i);
    }
}
