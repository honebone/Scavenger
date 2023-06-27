using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueueManager : MonoBehaviour
{
    [SerializeField]
    GameObject actionInfoPanel;

    [SerializeField]
    GameObject actionQueuePanel;
    [SerializeField]
    Transform content;

    [SerializeField]
    float abilityPause;
    [SerializeField]
    float abilityPause_followthrough;
    [SerializeField]
    Text abilityNameText_player;
    [SerializeField]
    Text abilityNameText_enemy;

    List<Action.ActionStatus> actionsBuffer=new List<Action.ActionStatus>();
    List<Action> inQueueActions = new List<Action>();
    List<Action> inQueueAbilityEffects = new List<Action>();

    BattleManager battleManager;
    CharactersManager charactersManager;
    InfoText infoText;
    Utility util;

    bool resolving;
    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</summary>
    int resolveMode;
    // Start is called before the first frame update
    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        charactersManager = FindObjectOfType<CharactersManager>();
        infoText=FindObjectOfType<InfoText>();
        util = FindObjectOfType<Utility>();
    }
    public void ToggleQueuePanel()
    {
        actionQueuePanel.SetActive(!actionQueuePanel.activeSelf);
    }
    public void OpenQueuePanel()
    {
        if (!actionQueuePanel.activeSelf) { actionQueuePanel.SetActive(true); }
    }
    public void CloseQueuePanel()
    {
        if (actionQueuePanel.activeSelf) { actionQueuePanel.SetActive(false); }
    }



    public void Enqueue(Action.ActionStatus status)
    {
        if (status.abilityEffect)//アビリティ効果の場合はマネージャーを即時作成
        {
            GameObject obj;
            if (status.actionObject != null) { obj = status.actionObject; }
            else { obj = Definer.actionManager_General; }
            var p = Instantiate(actionInfoPanel, content);
            var a = Instantiate(obj, p.transform);
            a.GetComponent<Action>().Init(this, status, p.GetComponent<ActionInfoPanel>(), util);

            inQueueAbilityEffects.Add(a.GetComponent<Action>());
        }
        else//そうでなければ一旦バッファに預ける
        {
            actionsBuffer.Add(status);
            //inQueueActions.Add(a.GetComponent<Action>());
            //OpenQueuePanel();
        }
        

    }  
    bool CheckIfActionsRemain() { return inQueueAbilityEffects.Count > 0 || inQueueActions.Count > 0 || actionsBuffer.Count > 0; }

    /// <summary>
    /// 誘発が発生しうるタイミングの後に呼ばれる
    /// </summary>
    /// <param name="mode">0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</param>
    public void StartResolve(int mode)
    {
        resolveMode = mode;

        if (CheckIfActionsRemain())//今回の誘発タイミングでアクションが発生したなら
        {
            if (inQueueAbilityEffects.Count>0)//アビリティ効果はプレイヤーの入力待たずに解決する
            {
                infoText.AddDebugText("アビリティ効果の解決");
                StartCoroutine(ResolveAbility());
            }
            else//アビリティの発動効果を含まない(=ActivateAbility以外の誘発タイミングである)場合は即座に表示アニメーション
            {
                StartCoroutine(DisplayActionInqueueAnim());
            }
        }
        else
        {
            EndResolve();
        }
    }



    IEnumerator DisplayActionInqueueAnim()
    {
        OpenQueuePanel();
        var wait = new WaitForSeconds(0.2f);
        while (actionsBuffer.Count > 0)
        {
            DisplayActionInQueue(actionsBuffer[0]);
            actionsBuffer.RemoveAt(0);
            yield return wait;
        }

        resolving = true;//表示アニメーション終わったので、解決ボタン押せるようにする
    }
    void DisplayActionInQueue(Action.ActionStatus status)
    {
        GameObject obj;
        if (status.actionObject != null) { obj = status.actionObject; }
        else { obj = Definer.actionManager_General; }
        var p = Instantiate(actionInfoPanel, content);
        var a = Instantiate(obj, p.transform);
        a.GetComponent<Action>().Init(this, status, p.GetComponent<ActionInfoPanel>(), util);
        inQueueActions.Add(a.GetComponent<Action>());

    }

    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</summary>

   
   
    IEnumerator ResolveAbility()
    {
        Action.ActionStatus actionStatus = inQueueAbilityEffects[0].GetActionStatus();

        Text abilityNameText;
        if (actionStatus.actionOwner.GetCharacterStatus().position < 9) { abilityNameText = abilityNameText_player; }
        else { abilityNameText = abilityNameText_enemy; }

        abilityNameText.text = util.GetColoredText(Definer.colorRef.abilityColors[(int)actionStatus.abilityType], actionStatus.actionName);
        abilityNameText.transform.GetChild(0).GetComponent<Image>().color = Definer.colorRef.abilityColors[(int)actionStatus.abilityType];

        actionStatus.actionOwner.SetActionInvolvedIcon(true);
        foreach(Action action in inQueueAbilityEffects)
        {
            foreach(Character target in action.GetActionStatus().actionTargets) { target.SetActionInvolvedIcon(false); }
        }

        yield return new WaitForSeconds(abilityPause);

        abilityNameText.text = "";
        abilityNameText.transform.GetChild(0).GetComponent<Image>().color = Color.clear;

        if (!actionStatus.dontChangeSprite) { actionStatus.actionOwner.SetCharaSprite(actionStatus.activateSprite); }
        charactersManager.ResetAllActionInvolvedIcons();

        inQueueAbilityEffects[0].Resolve();

        yield return new WaitForSeconds(abilityPause_followthrough);
        actionStatus.actionOwner.ResetCharaSprite();


        if (CheckIfActionsRemain()) { StartCoroutine(DisplayActionInqueueAnim()); }//アビリティ処理終えてもアクションがある=誘発がある ==>　それらを表示


    }
    IEnumerator ResolveNextAbilityEffect()
    {
        yield return new WaitForSeconds(0.2f);
        inQueueAbilityEffects[0].Resolve();
    }
    public void ResolveOne()
    {
        if (resolving)
        {
            inQueueActions[0].Resolve();
        }
    }

    /// <summary>アクションの処理が終わったらアクション内で呼ばれる </summary>
    public void Dequeue(string actionName)
    {
        infoText.AddDebugText(string.Format("{0}を解決", actionName));
        if(inQueueAbilityEffects.Count > 0) { inQueueAbilityEffects.RemoveAt(0); }
        else { inQueueActions.RemoveAt(0); }       
        Destroy(content.transform.GetChild(0).gameObject);

        if (!CheckIfActionsRemain())//キューにアクションが残ってなければ解決終了
        {
            resolving = false;
            infoText.AddDebugText("resolving=false");

            EndResolve();
            CloseQueuePanel();
        }
        else
        {
            if (inQueueAbilityEffects.Count>0)//アビリティ効果はプレイヤーの入力待たずに解決する
            {
                StartCoroutine(ResolveNextAbilityEffect());
            }
            //else//アビリティ以外なら解決ボタンが押されるのを待つ
            //{
            //    resolving = true;
            //}
        }

    }
    public void EndResolve()
    {

        switch (resolveMode)
        {
            case 0:
                break;
            case 2:
                resolveMode = -1;
                battleManager.GetCurrntTurnChara().MainPhase();
                break;
            case 3:
                resolveMode = -1;
                battleManager.GetCurrntTurnChara().EndPhase();
                break;
        }
        
    }

 
    
   
}
