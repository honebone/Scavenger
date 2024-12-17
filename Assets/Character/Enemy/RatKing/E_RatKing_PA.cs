using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_RatKing_PA : PA_Personality
{
    [SerializeField] Action.ActionStatus actionStatus_BattleStart;
    [SerializeField] Action.ActionStatus actionStatus_summon;
    [SerializeField] Action.ActionStatus actionStatus_remove;
    [SerializeField] CharactersManager.SearchCharaCondition emptyCondition;

    [SerializeField] List<GameObject> StE_organ;

    [SerializeField] List<CharacterData> chara_organ; 


    [SerializeField] float spawnHPRatio = 0.1f;
    int count;
    int countGoal;

    public override void OnBattleStart()
    {
        countGoal = Mathf.RoundToInt(character.CharaStatus().maxHP * spawnHPRatio);
        Enqueue_Self(actionStatus_BattleStart);
    }

    public override void OnDecreasedHP(int value)
    {
        count += value;

        if (count >= countGoal)
        {
            List<int> stacks = new List<int>();//現在の各器官のスタック状況
            foreach (GameObject body in StE_organ)
            {
                stacks.Add(character.GetStEStack_Sum(body));
            }
            while (count >= countGoal)
            {
                List<int> availableIndex = new List<int>();//スタックが1以上の器官のインデックス
                for (int i = 0; i < stacks.Count; i++)
                {
                    if (stacks[i] > 0) { availableIndex.Add(i); }
                }

                if (availableIndex.Count > 0)//スタックが1以上の器官があるなら
                {
                    int index = availableIndex.Choice();//その中から1つ選ぶ
                    GameObject removeStE = StE_organ[index]; ;
                    CharacterData summonBody = chara_organ[index];
                    stacks[index]--;

                    Action.ActionStatus summon = actionStatus_summon;
                    summon.summonChara = new List<CharacterData>() { summonBody };
                    summon.actionTargetsInt = charactersManager.SearchPosWithCondition(emptyCondition);
                    Enqueue(summon, false, new List<Character>(), 1);

                    Action.ActionStatus remove = actionStatus_remove;
                    ActionData.RemoveStE removeParams = new ActionData.RemoveStE();
                    removeParams.removeStE = removeStE;
                    removeParams.addAmount = -1;
                    remove.removeStEs = new List<ActionData.RemoveStE>() { removeParams };
                    Enqueue_Self(remove);
                }

                count -= countGoal;
            }
        }
       
    }
    public override string GetPAInfo_Base()
    {
        string s = actionStatus_BattleStart.GetInfo(true, character.CharaStatus()) + "\n";
        //s += actionStatus_summon.GetInfo(true, character.GetCharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"失ったHP：{count}/{countGoal}";
    }
}
