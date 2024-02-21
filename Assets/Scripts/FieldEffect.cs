using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEffect : MonoBehaviour
{
    [SerializeField]
    string FEName;
    [SerializeField, TextArea(3, 10)]
    string simpleInfo;

    protected CharactersManager charactersManager;
    protected ActionQueueManager actionQueue;
    protected InfoText infoText;
   public void Init(CharactersManager cm,ActionQueueManager aq,InfoText it)
    {
        charactersManager = cm;
        actionQueue = aq;
        infoText = it;
    }

   public virtual string GetFEInfo() { return ""; }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnOrderDecide() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }

}
