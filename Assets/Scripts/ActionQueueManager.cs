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
    AudioClip SE_ability;

    [SerializeField]
    float abilityPause;
    [SerializeField]
    float abilityPause_followthrough;
    [SerializeField]
    Text abilityNameText_player;
    [SerializeField]
    Text abilityNameText_enemy;

    
    [SerializeField]
    List<Action> inQueueActions = new List<Action>();
    Action resolvingAction;

    BattleManager battleManager;
    CharactersManager charactersManager;
    InfoText infoText;
    Utility util;
    SoundManager soundManager;
    ExpeditionManager expeditionManager;
    GuideMessage guideMessage;
    [SerializeField] CameraManager cameraMnager;
    [SerializeField] TotalDamageText totalDamageText;

    [SerializeField]
    bool autoResolve;
    bool canResolve;
    //bool resolveInterval;
    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd 6:turnOrderDecide 7:BatleEnd</summary>
    int resolveMode = -1;
    // Start is called before the first frame update
    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        charactersManager = FindObjectOfType<CharactersManager>();
        infoText=FindObjectOfType<InfoText>();
        util = FindObjectOfType<Utility>();
        soundManager = FindObjectOfType<SoundManager>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        guideMessage = FindObjectOfType<GuideMessage>();
    }

    public void ActionQueueButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText("アクションキュー", "発動した誘発能力の一覧を見る");
        }
        if (Input.GetMouseButtonDown(0))
        {
            ToggleQueuePanel();
        }
    }
    public void ToggleQueuePanel()
    {
        if (actionQueuePanel.activeSelf)
        {
            if (inQueueActions.Count == 0 || inQueueActions[0].CheckIfAbilityEffect())
            {
                actionQueuePanel.SetActive(false);
            }
            else
            {
                guideMessage.SetWaringText("誘発能力を全て解決する必要がある");
            }
        }
        else { actionQueuePanel.SetActive(true); }
    }
    public void OpenQueuePanel()
    {
        if (!actionQueuePanel.activeSelf) { actionQueuePanel.SetActive(true); }
        if (actionInfoPanel.activeSelf && inQueueActions.Count > 0)
        {
            expeditionManager.StartTutorial_Passive();
        }
    }
    public void CloseQueuePanel()
    {
        if (actionQueuePanel.activeSelf) { actionQueuePanel.SetActive(false); }
    }

    public void Enqueue(Action.ActionStatus status)
    {
        GameObject obj;
        if (status.actionObject != null) { obj = status.actionObject; }
        else { obj = Definer.actionManager_General; }
        var p = Instantiate(actionInfoPanel, content);
        var a = Instantiate(obj, p.transform);
        a.GetComponent<Action>().Init(this, status, p.GetComponent<ActionInfoPanel>(), infoText, util, soundManager, cameraMnager,battleManager);

        inQueueActions.Add(a.GetComponent<Action>());

        //if (status.abilityEffect)
        //{
        //    bool found = false;
        //    for (int i = inQueueActions.Count - 1; i >= 0; i--)
        //    {
        //        if (inQueueActions[i].GetActionStatus().abilityEffect)
        //        {
        //            found = true;
        //            inQueueActions.Insert(i + 1, a.GetComponent<Action>());
        //            break;
        //        }
        //    }
        //    if (!found) { inQueueActions.Insert(0, a.GetComponent<Action>()); }

        //}
        //else
        //{
        //    inQueueActions.Add(a.GetComponent<Action>());
        //}
    }

    /// <summary>
    /// 誘発が発生しうるタイミングの後に呼ばれる
    /// 0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd 6:turnOrderDecide 6:BattleEnd
    /// </summary>
    public void StartResolve(int mode)
    {
        if (resolveMode != -1) { infoText.AddErrorText(string.Format("resolveModeが予期せぬ値になっています 期待:-1 現在:{0}", resolveMode)); }
        resolveMode = mode;
        if (resolveMode == 7 && inQueueActions.Count > 0) { infoText.AddErrorText("戦闘終了時にActionがEnqueueされています"); }

        if (inQueueActions.Count > 0)
        {
            if (inQueueActions[0].GetActionStatus().abilityEffect)//アビリティの効果があるなら勝手に解決開始
            {
                SetResolvingAction();
                StartCoroutine(ResolveAbility());
            }
            else { WaitForResolve(); }
        }
        else
        {
            StartCoroutine(EndResolve());
        }
    }

    

    void WaitForResolve()
    {
        canResolve = true;
        if (autoResolve)
        {
            ResolveOne();
        }
        else
        {
            OpenQueuePanel();
        }
    }

    public void Resolve_Manual()
    {
        if (!autoResolve && canResolve)
        {
            canResolve = false;
            ResolveOne();
        }
    }

    public void ResolveOne()
    {
        canResolve = false;
        if (inQueueActions[0] == null) { infoText.AddErrorText(""); }
        SetResolvingAction();

        if (resolvingAction.GetActionStatus().abilityEffect) { infoText.AddErrorText("ここでアビリティの解決をすることはあり得ません"); }
        else { ResolveAction(); }
    }

    void SetResolvingAction()
    {
        resolvingAction = inQueueActions[0];
        inQueueActions.RemoveAt(0);
    }

    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</summary>
    IEnumerator ResolveAbility()//1つめのアビリティ効果は名前の表示等の演出をつける
    {
        Action.ActionStatus actionStatus = resolvingAction.GetActionStatus();
        if (!actionStatus.abilityEffect) { infoText.AddErrorText("abilityでないアクションを解決しています"); }

        Text abilityNameText;
        if (actionStatus.actionOwner.GetCharacterStatus().position < 9) { abilityNameText = abilityNameText_player; }
        else { abilityNameText = abilityNameText_enemy; }

        abilityNameText.text = util.GetColoredText(Definer.colorRef.abilityColors[(int)actionStatus.abilityType], actionStatus.actionName);
        abilityNameText.transform.GetChild(0).GetComponent<Image>().color = Definer.colorRef.abilityColors[(int)actionStatus.abilityType];
        abilityNameText.transform.parent.GetComponent<Image>().enabled = true;

        if (actionStatus.actionOwner != null && !actionStatus.actionOwner.GetCharacterStatus().player)
        {
            soundManager.PlaySE(SE_ability);

            actionStatus.actionOwner.SetActionInvolvedIcon(true);

            if (!actionStatus.condition.searchAsPos && actionStatus.targetType != Action.ActionStatus.TargetType.move)//キャラクターを対象とする場合、そのキャラクターのスクリプト経由でアイコン表示
            {
                foreach (Character target in actionStatus.actionTargets) { target.SetActionInvolvedIcon(false); }
            }

            foreach (Action action in inQueueActions)//2つめ以降のアビリティ効果についても同様の処理
            {
                if (action.GetActionStatus().abilityEffect)
                {
                    if (!action.GetActionStatus().condition.searchAsPos && action.GetActionStatus().targetType != Action.ActionStatus.TargetType.move)//キャラクターを対象とする場合、そのキャラクターのスクリプト経由でアイコン表示
                    {
                        foreach (Character target in action.GetActionStatus().actionTargets) { target.SetActionInvolvedIcon(false); }
                    }
                }
            }

            yield return new WaitForSeconds(abilityPause);
        }
       

        //if (!actionStatus.dontChangeSprite) { actionStatus.actionOwner.SetCharaSprite(actionStatus.activateSprite); }
        charactersManager.ResetAllActionInvolvedIcons();

        resolvingAction.Resolve();

        yield return new WaitForSeconds(abilityPause_followthrough);

        abilityNameText.text = "";
        abilityNameText.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        abilityNameText.transform.parent.GetComponent<Image>().enabled = false;

        //actionStatus.actionOwner.ResetCharaSprite();
    }
    IEnumerator ResolveNextAbilityEffect()//2つ目以降のアビリティ効果
    {
        yield return new WaitForSeconds(0.2f);

        if (inQueueActions[0] == null) { infoText.AddErrorText(""); }
        SetResolvingAction();

        resolvingAction.Resolve();
    }

    void ResolveAction()//誘発効果
    {
        resolvingAction.Resolve();
    }
   

    [SerializeField]
    float autoResolveInterval;
    public void SetResolveSpeed(float value) { autoResolveInterval = value; }
    IEnumerator ResolveFirstActionEffect()
    {
        yield return new WaitForSeconds(0.2f);
        ResolveOne();
    }
    IEnumerator ResolveNextActionEffect()
    {
        yield return new WaitForSeconds(autoResolveInterval);
        ResolveOne();
    }
    public void ToggleAutoResolve()
    {
        autoResolve = !autoResolve;
        if (autoResolve && canResolve)
        {
            canResolve = false;
            ResolveOne();
        }
    }

    /// <summary>アクションの処理が終わったらアクション内で呼ばれる </summary>
    public void Dequeue()
    {
        bool prevAbility = resolvingAction.GetActionStatus().abilityEffect;
        resolvingAction = null;
        Destroy(content.transform.GetChild(0).gameObject);

        if (inQueueActions.Count == 0)//キューにアクションが残ってなければ解決終了
        {
            CloseQueuePanel();
            StartCoroutine(EndResolve());
        }
        else//まだアクションがある
        {
            if (inQueueActions[0].GetActionStatus().abilityEffect) { StartCoroutine(ResolveNextAbilityEffect()); }//アビリティ効果はプレイヤーの入力待たずに解決する
            else 
            {
                if (prevAbility)//アビリティ効果を終えた後はフォロースルー待つ
                {
                    StartCoroutine(EndAbilityEffect());
                }
                else
                {
                    if (autoResolve)
                    {
                        StartCoroutine(ResolveNextActionEffect());
                    }
                    else
                    {
                        OpenQueuePanel();
                        WaitForResolve();
                    }
                }
            }
        }
    }

    IEnumerator EndAbilityEffect()
    {
        yield return new WaitForSeconds(abilityPause_followthrough);
        if (autoResolve)
        {
            StartCoroutine(ResolveFirstActionEffect());
        }
        else
        {
            OpenQueuePanel();
            WaitForResolve();
        }
    }

    IEnumerator EndResolve()
    {
        if (resolveMode != 7 && (charactersManager.CheckVictory() || charactersManager.CheckDefeat()))
        {
            totalDamageText.ResetText();
            if (charactersManager.CheckDefeat())
            {
                expeditionManager.Defeat();//test
            }
            else if (charactersManager.CheckVictory())
            {
                resolveMode = -1;
                battleManager.BattleEnd();
            }
           
        }
        else
        {
            switch (resolveMode)
            {
                case 0:
                    resolveMode = -1;
                    totalDamageText.ResetText();
                    battleManager.RoundStart();
                    break;
                case 1:
                    resolveMode = -1;
                    totalDamageText.ResetText();
                    battleManager.DicideTurnOrder();
                    break;
                case 2:
                    resolveMode = -1;
                    totalDamageText.ResetText();
                    battleManager.GetCurrntTurnChara().MainPhase();
                    break;
                case 3:
                    resolveMode = -1;
                    yield return new WaitForSeconds(abilityPause_followthrough);
                    totalDamageText.ResetText();
                    battleManager.GetCurrntTurnChara().EndPhase();
                    break;
                case 4:
                    resolveMode = -1;
                    totalDamageText.ResetText();
                    battleManager.TurnEnd(0);
                    break;
                case 5:
                    resolveMode = -1;
                    totalDamageText.ResetText();
                    battleManager.RoundStart();
                    break;
                case 6:
                    resolveMode = -1;
                    totalDamageText.ResetText();
                    battleManager.EndTrigger_TurnOrderDecide();
                    break;
                case 7:
                    resolveMode = -1;
                    totalDamageText.ResetText();
                    battleManager.EndTrigger_BattleEnd();
                    break;
            }
        }
    }

 
    
   
}
