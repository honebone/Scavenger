using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_HungryAxe : PA_Equipment
{
    [SerializeField] List<Vector2Int> neigbor;
    [SerializeField] float ATKRatio;
    [SerializeField] Action.ActionStatus actionStatus;
    public override void OnTurnEnd(TurnEndParams tep)
    {
        if (tep.myTurn)
        {
            List<Character> target=new List<Character>(character.GetNeigbor(neigbor));
            if(target.Count > 0)
            {
                //Action.ActionStatus action = actionStatus;
                //action.decreaseHP_max = (character.CharaStatus().ATK * ATKRatio / 100f).ToInt();
                //action.decreaseHP_min = (character.CharaStatus().ATK * ATKRatio / 100f).ToInt();

                Enqueue(actionStatus, true, target, target.Count);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
}
