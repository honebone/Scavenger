using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_FuryAndAshes : PA_Equipment
{
    [SerializeField] GameObject burn;
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int countGoal;
    [SerializeField] int defRemain;
    int count;
    int remain;
    public override void OnBattleStart()
    {
        remain = defRemain;
    }
    public override void OnRoundStart()
    {
        remain = defRemain;
    }
    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);

        foreach(Action.OnDamageParams onDamageParams in onDamageParamsList)
        {
            if (remain > 0)
            {
                if (onDamageParams.ap.target.CheckHasStE(burn))
                {
                    count++;
                    if (target.Count > 0 && count >= countGoal)
                    {
                        count -= countGoal;
                        remain--;
                        Enqueue(attack, true, target, 2);
                    }
                }
            }
        }
       
        
    }
    public override void OnBattleEnd()
    {
        remain = defRemain;
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
        return $"カウント：{count}/{countGoal}\n残り発動回数：{remain}";
    }
}
