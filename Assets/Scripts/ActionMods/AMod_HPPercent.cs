using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMod_HPPercent : ActionMod
{
    [SerializeField, Header("trueなら減少HP falseなら残HP")] bool HPLoss;
    [SerializeField, Header("失った/残りHP[PercentPerCount]％毎に")] float percentPerCount;
    [SerializeField, Header("カウント開始の閾値")] float THPercent;
    [SerializeField, Header("0ならリミット無し")] int maxCount;
    [SerializeField] bool checkOwner;

    public override Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        int count = 0;
        float percent = 0;
        float percentDelta = 0;
        Character owner = statusRef.actionOwner;
        Character.CharacterStatus status = checkOwner ? owner.CharaStatus() : new Character.CharacterStatus();
        for (int i = 0; i < statusRef.actionTargets.Count; i++)
        {
            if (!checkOwner) status = statusRef.actionTargets[i].CharaStatus();
            percent = status.GetHPPercent();
            if ((HPLoss && percent >= THPercent) || (!HPLoss && percent <= THPercent)) { break; }//減少HP参照でHPが閾値以上　または　残HP参照でHPが閾値以下の場合はスキップ
            percentDelta = Mathf.Abs(THPercent - percent);//閾値と現在HPの差分の絶対値
            count = Mathf.FloorToInt(percentDelta / percentPerCount);
            if(maxCount!=0) count =  count.Limit(maxCount);//最大カウントをリミットに
            for (int j = 0; j < count; j++)
            {
                actionsStatus[i] = actionsStatus[i].Modify(actionModStatus);
            }
            string log = $"{THPercent}{((HPLoss) ? "を超えて減少" : "を超えて残存")}したHP{percentPerCount}％ごと\n現在割合：{percent}％(差分{percentDelta}％)\nカウント：{count}(最大{maxCount})";
            ExpeditionRef.infoText.AddDebugText(log);
        }
        return actionsStatus;
    }
}
