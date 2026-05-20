using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_StarInterpreter2 : PA_Personality
{
    [SerializeField] int interval;
    [SerializeField] Action.ActionStatus endless;
    [SerializeField] Action.ActionStatus darkness;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    bool f = false;
    int count;

    public override void OnRoundStart()
    {
        count--;
        if(count <= 0)
        {
            count = interval;
            if (f) Enqueue_SearchTarget(darkness, condition,2);
            else Enqueue_SearchTarget(endless, condition,2);
            f = !f;
        }
    }

    public override string GetCurrentStateInfo()
    {
        string fear = f ? "ˆÃ‚«‹°•|" : "‰Ê‚Ä‚È‚«‹°•|";
        return $"ŽŸ‚Ì‹°•|‚Ü‚Å{count}ƒ‰ƒEƒ“ƒh\nŽŸ‚Ì‹°•|F{fear}";
    }
}
