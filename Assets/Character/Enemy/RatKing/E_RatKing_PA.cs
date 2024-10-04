using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_RatKing_PA : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus_BattleStart;
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition emptyCondition;
    [SerializeField] float spawnHPRatio = 0.1f;
    int count;
    int countGoal;

    public override void OnBattleStart()
    {
        countGoal = Mathf.RoundToInt(character.GetCharacterStatus().maxHP * spawnHPRatio);
        Enqueue_Self(actionStatus_BattleStart);
    }

    public override void OnDecreasedHP(int value)
    {
        count += value;
        while (count >= countGoal)
        {
            Action.ActionStatus action = actionStatus;
            action.actionTargetsInt = charactersManager.SearchPosWithCondition(emptyCondition);
            Enqueue(action, false, new List<Character>(), 1);

            count -= countGoal;
        }
    }
    public override string GetPAInfo_Base()
    {
        string s = actionStatus_BattleStart.GetInfo(true, character.GetCharacterStatus()) + "\n";
        s += actionStatus.GetInfo(true, character.GetCharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"é∏Ç¡ÇΩHPÅF{count}/{countGoal}";
    }
}
