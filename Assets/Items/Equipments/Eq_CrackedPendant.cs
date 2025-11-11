using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_CrackedPendant : PA_Equipment
{
    [SerializeField] List<int> loseHPPer;
    [SerializeField] List<int> exDMG;
    [SerializeField] Action.ActionStatus actionStatus;

    int row;
    public override void OnBattleStart()
    {
        Action.ActionStatus action = actionStatus;
        row = character.CharaStatus().position.GetRow();
        int loseHP = (character.CharaStatus().HP * loseHPPer[row] / 100f).ToInt();
        action.decreaseHP_max = loseHP;
        action.decreaseHP_min = loseHP;
        action.shieldAdd_max = loseHP;
        action.shieldAdd_min = loseHP;

        Enqueue_Self(action);

        character.AddExDMG_Mul(exDMG[row]);
    }

    public override void OnBattleEnd()
    {
        character.AddExDMG_Mul(-exDMG[row]);

    }
    public override string GetCurrentStateInfo()
    {
        return $"ó^É_ÉÅÅ[ÉW+{exDMG[row]}Åì";
    }
}
