using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Cleanse : Eq_Magic
{
    public Action.ActionStatus actionStatus;
    int count;

    public override void OnRoundEnd()
    {
        count++;
        if (count % 2 == 0) { Cast(); }
    }

    public override void Cast()
    {
        Enqueue_Self(actionStatus);
        character.OnCast(this);
    }

    public override void OnBattleEnd()
    {
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += "\n" + actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
