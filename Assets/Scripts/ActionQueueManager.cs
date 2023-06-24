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
    Text abilityNameText_player;
    [SerializeField]
    Text abilityNameText_enemy;

    List<Action> inQueueActions;

    BattleManager battleManager;
    Utility util;

    bool resolving;
    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</summary>
    int resolveMode;
    // Start is called before the first frame update
    void Start()
    {
        inQueueActions = new List<Action>();
        battleManager = FindObjectOfType<BattleManager>();
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
        GameObject obj;
        if (status.actionObject != null) { obj = status.actionObject; }
        else { obj = Definer.actionManager_General; }
        var p = Instantiate(actionInfoPanel, content);
        var a = Instantiate(obj, p.transform);
        a.GetComponent<Action>().Init(this, status,p.GetComponent<ActionInfoPanel>(),util);
        inQueueActions.Add(a.GetComponent<Action>());

        if (!status.abilityEffect) { OpenQueuePanel(); } 
    }

    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</summary>

    public void StartResolve(int mode)
    {
        resolveMode = mode;
        if (inQueueActions.Count > 0)
        {
            if (inQueueActions[0].GetActionStatus().abilityEffect)//アビリティ効果はプレイヤーの入力待たずに解決する
            {
                StartCoroutine(ResolveAbility());
            }
            else//アビリティ以外なら解決ボタンが押されるのを待つ
            {
                Debug.Log("resolve開始");
                resolving = true;
            }          
        }
        else
        {
            EndResolve();
        }
    }
    IEnumerator ResolveAbility()
    {
        Action.ActionStatus actionStatus = inQueueActions[0].GetActionStatus();

        Text abilityNameText;
        if (actionStatus.actionOwner.GetCharacterStatus().position < 9) { abilityNameText = abilityNameText_player; }
        else { abilityNameText = abilityNameText_enemy; }

        abilityNameText.text = util.GetColoredText(Definer.colorRef.abilityColors[(int)actionStatus.abilityType], actionStatus.actionName);
        abilityNameText.transform.GetChild(0).GetComponent<Image>().color = Definer.colorRef.abilityColors[(int)actionStatus.abilityType];

        yield return new WaitForSeconds(abilityPause);

        abilityNameText.text = "";
        abilityNameText.transform.GetChild(0).GetComponent<Image>().color = Color.clear;

        ResolveAbilityEffect();
    }

    public void Dequeue(string actionName)
    {
        Debug.Log(string.Format("{0}を解決", actionName));
        inQueueActions.RemoveAt(0);
        Destroy(content.transform.GetChild(0).gameObject);

        if (inQueueActions.Count == 0)//キューにアクションが残ってなければ解決終了
        {
            resolving = false;
            Debug.Log("resolve終了");
            EndResolve();
            CloseQueuePanel();
        }
        else
        {
            if (inQueueActions[0].GetActionStatus().abilityEffect)//アビリティ効果はプレイヤーの入力待たずに解決する
            {
                ResolveAbilityEffect();
            }
            else//アビリティ以外なら解決ボタンが押されるのを待つ
            {
                resolving = true;
            }
        }

    }
    public void EndResolve()
    {

        switch (resolveMode)
        {
            case 0:
                break;
            case 2:
                battleManager.GetCurrntTurnChara().MainPhase();
                break;
            case 3:
                battleManager.GetCurrntTurnChara().EndPhase();
                break;
        }

    }

    //void Update()
    //{
    //    if (resolving && Input.GetKeyDown(KeyCode.Return))
    //    {
    //        inQueueActions[0].Resolve();
    //    }
    //}
    public void ResolveOne()
    {
        if (resolving)
        {
            inQueueActions[0].Resolve();
        }
    }
    void ResolveAbilityEffect()
    {
        inQueueActions[0].Resolve();
    }
}
