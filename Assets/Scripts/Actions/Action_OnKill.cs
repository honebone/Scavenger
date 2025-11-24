using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Action_OnKill : Action
{
    [Header("”­“®‰ñ”‚ÌãŒÀ")]
    public int maxActivate;
    public bool excludeObstacle;
    public bool targetSelf;
    public ActionStatus secondAction;
    public CharactersManager.SearchCharaCondition condition;
    public int targetCount;

    public override void SecondEffect()
    {
        int remain = maxActivate;
        actionResults.ForEach(r =>
        {
            if (r.kill && remain > 0 && (!excludeObstacle || !r.target.CharaStatus().characterTags.Contains(CharacterData.CharacterTag.obstacle)))
            {
                remain--;
                if (targetSelf)
                {
                    Enqueue_Self(secondAction);
                }
                else
                {
                    Enqueue_SearchTarget(secondAction, condition, targetCount);
                }
                return;
            }
        });
    }

    public override string GetAdditionalInfo()
    {
        return secondAction.GetInfo();
    }
}
