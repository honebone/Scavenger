using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_GuardianShield : PA_Equipment
{
    public int CD;
    public int HPTH;
    public float shieldPercent;
    public Action.ActionStatus selfAction;
    public Action.ActionStatus allyAction;

    int count;

    public override void OnBattleStart()
    {
        count = 0;
    }

    public override void OnSomeoneDamaged(Action.OnDamageParams onDamageParams)
    {
        Character target = onDamageParams.ap.target;
        if (count == 0 && target != character && target.IsPlayer() && onDamageParams.ap.target.CharaStatus().GetHPPercent() <= HPTH)
        {
            count = CD;
            Enqueue_Self(selfAction);
            Action.ActionStatus ally = allyAction;
            ally.shieldAdd_max = (character.CharaStatus().maxHP * shieldPercent / 100f).ToInt();
            ally.shieldAdd_min = (character.CharaStatus().maxHP * shieldPercent / 100f).ToInt();
            Enqueue(ally, true, new List<Character> { onDamageParams.ap.target });
        }
    }

    public override void OnRoundEnd()
    {
        if (count > 0)
        {
            count--;
            if (count == 0) Log("再発動可能");
        }
    }

    public override string GetCurrentStateInfo()
    {
        return count == 0 ? "発動可能" : $"再発動まで{count}ラウンド";
    }

    public override string GetPAInfo_Base()
    {
        string s = selfAction.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
