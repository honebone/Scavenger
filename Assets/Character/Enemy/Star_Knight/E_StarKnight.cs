using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class E_StarKnight : PA_Personality
{
    public int healPerStardust;
    public GameObject stardust;
    public Action.ActionStatus onDMG;
    public Action.ActionStatus onDying;

    bool available;

    public override void OnBattleStart()
    {
        available = true;
    }
    public override void OnSummoned(Action.OnSummonParams onSummonParams)
    {
        available = true;
    }
    public override void OnRoundStart()
    {
        if (!available) Log("Ä”­“®‰Â”\");
        available = true;
    }

    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        var enemyDMG = onDamageParamsList.Where(m => m.ap.target.PlayerPos() != character.PlayerPos());
        if (enemyDMG.Count() > 0)
        {
            Enqueue_Self(onDMG);
        }
    }
    public override void OnDecreasedHP(int value)
    {
        int dusts = character.GetStEStack_Sum(stardust);
        if (character.CharaStatus().HP == 0 && available && dusts > 0)
        {
            available = false;
            if (dusts > 0)
            {
                Action.ActionStatus action = onDying;
                action.healPercent_min = dusts * healPerStardust;
                action.healPercent_max = dusts * healPerStardust;
                Enqueue_Self(action);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = onDMG.GetInfo() + "\n";
        s += onDying.GetInfo();
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return available ? "”­“®‰Â”\" : "”­“®•s‰Â".ColorStr(Color.gray);
    }
}
