using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Pest : PA_Personality
{
    [SerializeField] Action.ActionStatus applySecretion;

    [SerializeField] Action.ActionStatus heal;
    [SerializeField] Action.ActionStatus acid;
    [SerializeField] int maxHPRatio = 5;
    [SerializeField] GameObject secretion;
    [SerializeField] int thRatio;
    bool applyed;
    public override void OnDecreasedHP(int value)
    {
        int th = (character.CharaStatus().maxHP * thRatio / 100f).ToInt();
        if (character.CharaStatus().HP <= th&&!applyed)
        {
            applyed = true;
            Enqueue_Self(applySecretion);
        }
    }

    public override void OnDie(Character killer)
    {
        if (killer != null)
        {
            if (character.CheckHasStE(secretion))
            {
                Action.ActionStatus action = acid;
                action.exATKDMG_int = (killer.CharaStatus().maxHP * maxHPRatio / 100f).ToInt();
                Enqueue(action, true, new List<Character> { killer });
            }
            else
            {
                Enqueue(heal, true, new List<Character> { killer });
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += applySecretion.GetInfo(false, new Character.CharacterStatus()) + "\n";
        s += heal.GetInfo(false, new Character.CharacterStatus()) + "\n";
        s += acid.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
