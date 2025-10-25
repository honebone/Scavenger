using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_BlankGrimoire : PA_Equipment
{
    List<PassiveAbility> castList=new List<PassiveAbility>();
    public override void OnCast(PassiveAbility cast)
    {
        if(!castList.Contains(cast))
        {
            castList.Add(cast);
            Log($"{cast.GetPAName()}‚ð‹L˜^");
        }
    }

    //public override void OnTurnStart(bool myTurn, int turnCount)
    //{
    //    if(myTurn)
    //    {
    //        castList.ForEach(cast =>
    //        {
    //            cast.Cast();
    //            cast.Cast();
    //        });
    //    }
    //}
    public override void OnRoundEnd()
    {
        castList.ForEach(cast =>
        {
            cast.Cast();
            cast.Cast();
        });
    }

    public override void OnBattleEnd()
    {
        castList.Clear();
    }

    public override string GetCurrentStateInfo()
    {
        string s = "‹L˜^‚µ‚½–‚pF";
        if (castList.Count > 0) { castList.ForEach(cast => s += $"\n{cast.GetPAName()}"); }
        else s += "–³‚µ";
        return s;
    }
}
