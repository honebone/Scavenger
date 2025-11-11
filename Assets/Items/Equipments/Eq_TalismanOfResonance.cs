using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_TalismanOfResonance : PA_Equipment
{
    [SerializeField] List<Vector2Int> neigbors;
    [SerializeField] Action.ActionStatus actionStatus;
    
    public override void OnHeal(List<Action.OnHealParams> onHealParamsList)
    {
        if (onHealParamsList.Count > 0 && onHealParamsList[0].ability)
        {
            List<Character> pool = new List<Character>();
            foreach(Action.OnHealParams onHealParams in onHealParamsList)
            {
                List<int> neigbor = onHealParams.target.CharaStatus().position.RelPosToAbs(neigbors);
                pool.AddRangeWithNoOverlap(charactersManager.GetCharactersWithPos(neigbor));
            }
            if(pool.Count > 0)
            {
                Enqueue(actionStatus, true, pool.Sample(1));
            }
        }
        
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo();
        return s;
    }
   
}
