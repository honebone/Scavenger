using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqM_Storm : Eq_Magic
{
    public int castChance;
    public int DMGPerCast;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;

    int castCount;
    public override void OnBattleStart()
    {
        castCount = 0;
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (BattleManager.inst.GetCurrntTurnChara().PlayerPos() == character.PlayerPos() && castChance.Dice())
        {
            Cast();
        }
    }

    public override void Cast()
    {
        Action.ActionStatus action = actionStatus;
        action.INTMod_max += DMGPerCast * castCount;
        action.INTMod_min += DMGPerCast * castCount;
        List<Character> targets = new List<Character>(charactersManager.SearchCharaWithCondition(condition));

        if (Enqueue(action, true, targets,1))
        {
            Log("カウント+1");
            castCount++;
            character.OnCast(this);
        }
            
    }

    public override void OnBattleEnd()
    {
        castCount = 0;
    }
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo();
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"詠唱回数：{castCount}回(INT補正+{DMGPerCast * castCount}％)";
    }
}
