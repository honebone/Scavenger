using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Commander2 : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] ActionMod.ActionModStatus mod_3;
    [SerializeField] ActionMod.ActionModStatus mod_4;
    [SerializeField] ActionMod.ActionModStatus mod_5;
    [SerializeField] ActionMod.ActionModStatus mod_8;
    int count;

    public override void OnBattleStart()
    {
        count = 0;
    }
    public override void OnSomeoneFocus(List<Action.OnFocusParams> focusParamsList)
    {
        foreach (Action.OnFocusParams onFocusParams in focusParamsList)
        {
            if (onFocusParams.actionParams.owner.PlayerPos() == character.PlayerPos()) { count++; }
        }
    }

    public override void OnRoundEnd()
    {
        if(count >= 2)
        {
            Action.ActionStatus action = actionStatus;
            if (count >= 3) action = action.Modify(mod_3);
            if (count >= 4) action = action.Modify(mod_4);
            if (count >= 5) action = action.Modify(mod_5);
            if (count >= 8) action = action.Modify(mod_8);

            Enqueue(action, true, charactersManager.SearchChara_AllInOneSide(character.PlayerPos()));
        }
        count = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = "";
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        s += "\n" + mod_3.GetModInfo();
        s += "\n" + mod_4.GetModInfo();
        s += "\n" + mod_5.GetModInfo();
        s += "\n" + mod_8.GetModInfo();

        return s;
    }

    public override string GetCurrentStateInfo()
    {
        string s = "";
        s += $"現在のカウント：{count}\n";
        s += "カウント2：ATK・INT上昇\n".ColorStr((count >= 2) ? Definer.colorRef.currentState : Color.gray);
        s += "カウント3：訓練\n".ColorStr((count >= 3) ? Definer.colorRef.currentState : Color.gray);
        s += "カウント4：SAN回復\n".ColorStr((count >= 4) ? Definer.colorRef.currentState : Color.gray);
        s += "カウント5：回復\n".ColorStr((count >= 5) ? Definer.colorRef.currentState : Color.gray);
        s += "カウント8：追加ターン\n".ColorStr((count >= 8) ? Definer.colorRef.currentState : Color.gray);

        return s;
    }
}
