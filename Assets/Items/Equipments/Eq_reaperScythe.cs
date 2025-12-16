using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_reaperScythe : PA_Equipment
{
    public int exTurnTH;
    public int CRITC;
    public int maxCRITCCount;
    public Action.ActionStatus exTurn;
    int count;
    public override void OnKill(List<Action.OnKillParams> onKillParamsList)
    {
        onKillParamsList.ForEach(x =>
        {
            if (!x.target.IsObstacle()&&count< maxCRITCCount)
            {
                count++;
                if (count == exTurnTH) Enqueue_Self(exTurn);
                character.AddCRITC(CRITC);
                Log($"{"CRIT".ToSpr_withName()}率+{CRITC}％ (+{CRITC * count}％)");
            }
        });
    }

    public override void OnBattleEnd()
    {
        character.AddCRITC(-CRITC * count);
        count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        string s= count >= exTurnTH ? "(追加ターン取得済み)" : "(追加ターン未取得)";
        return $"カウント：{count}/{maxCRITCCount} ({"CRIT".ToSpr_withName()}率+{CRITC * count}％)\n{s}";
    }
}
