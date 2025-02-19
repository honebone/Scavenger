using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Blader_PA : PA_Personality
{
    [SerializeField] int attackChance;
    [SerializeField] int danceOnMovedAll;
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] CharactersManager.SearchCharaCondition condition_focus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] Action.ActionStatus combo;

    [SerializeField] GameObject bladestorm;

    bool[] movedDir = new bool[4];
    string[] dirName = { "前", "上", "下", "後ろ" };

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if (!movedDir[onMoveParams.dir] || attackChance.Dice())
        {
            if (!movedDir[onMoveParams.dir])
            {
                movedDir[onMoveParams.dir] = true;
                Log($"{dirName[onMoveParams.dir]}方向達成");
            }
            Dance();
        }
    }

    public override void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        foreach (Action.OnAttackParams attackParams in onAttackParamsList)
        {
            if (attackParams.hit)
            {
                Enqueue_Self(combo);
                break;
            }
        }
    }

    void Dance(bool second = false)
    {
        List<Character> target_focus = charactersManager.SearchCharaWithCondition(condition_focus);
        List<Character> target = charactersManager.SearchCharaWithCondition(condition);
        if (target_focus.Count > 0) { Enqueue(attack, true, target_focus, 1); }
        else if (target.Count > 0) { Enqueue(attack, true, target, 1); }
        if (!second && character.CheckHasStE(bladestorm))
        {
            Dance(true);
            character.AddStEStack(bladestorm, -1);
        }
    }

    public override void OnRoundEnd()
    {
        List<int> moved = new List<int>();

        for (int i = 0; i < movedDir.Length; i++)
        {
            if (movedDir[i]) { moved.Add(i); }
        }

        if (moved.Count == movedDir.Length)
        {
            Log($"全方向達成");
            movedDir = new bool[4];
            for (int i = 0; i < danceOnMovedAll; i++) { Dance(); }
        }
        else if (moved.Count > 0)
        {
            int reset = moved.Choice();
            movedDir[reset] = false;
            Log($"記録リセット：{dirName[reset]}");
        }
    }

    public override void OnBattleEnd()
    {
        movedDir = new bool[4];
    }


    public override string GetPAInfo_Base()
    {
        string s = attack.GetInfo(false, new Character.CharacterStatus());
        s += "\n" + combo.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        string s = "記録した移動方向\n";
        for (int i = 0; i < movedDir.Length; i++)
        {
            s += $"{dirName[i]}：{(movedDir[i] ? "記録あり" : "記録なし".ColorStr(Color.gray))}\n";
        }
        return s;
    }
}
