using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Busket : PA_Equipment
{
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int thPercent;
    int count;
    int remain = 3;

    public override void OnRoundStart()
    {
        remain = 3;
    }

    public override void OnHealed(Character healer, Action.OnHealParams onHealParams)
    {
        count += onHealParams.healValue;
        int th = Mathf.RoundToInt(character.GetCharacterStatus().maxHP * thPercent / 100f);
        if (remain > 0)
        {
            while (count >= th)
            {
                count -= th;
                List<Character> target = charactersManager.SearchCharaWithCondition(condition);
                if (target.Count > 0)
                {
                    Enqueue(attack, true, target, 2);
                }
                remain--;
                if (remain == 0) { break; }
            }
        }
       
    }
    public override void OnBattleEnd()
    {
        remain = 3;
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        int th = Mathf.RoundToInt(character.GetCharacterStatus().maxHP * thPercent / 100f);
        return string.Format("現在のカウント：{0}/{1}\n残り発動回数：{2}", count, th, remain);
    }
}
