using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sniper : PA_Personality
{
    [SerializeField] GameObject bullet;
    [SerializeField] int CRITDPerBullet;
    [SerializeField] Action.ActionStatus actionStatus;
    int count;
    public override void OnBattleStart()
    {
        Enqueue_Self(actionStatus);
    }

    public override void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        onApplyStEParams.appliedParams.ForEach(p =>
        {
            if (p.applyStE == bullet)
            {
                character.AddCRITD(CRITDPerBullet * p.stack);
                count += p.stack;
                Log($"{"CRIT".ToSpr_withName()}ダメージ+{CRITDPerBullet * p.stack}％");
            }
        });
    }

    public override void OnBattleEnd()
    {
        character.AddCRITD(-CRITDPerBullet * count);
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"{"CRIT".ToSpr_withName()}ダメージ+{CRITDPerBullet * count}％";
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }
}
