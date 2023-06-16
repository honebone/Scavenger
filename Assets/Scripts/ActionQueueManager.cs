using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    [SerializeField]
    GameObject actionInfoPanel;

    [SerializeField]
    GameObject actionQueuePanel;
    [SerializeField]
    Transform content;
    List<Action> inQueueActions;

    BattleManager battleManager;

    bool resolving;
    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</summary>
    int resolveMode;
    // Start is called before the first frame update
    void Start()
    {
        inQueueActions = new List<Action>();
        battleManager = FindObjectOfType<BattleManager>();
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
        a.GetComponent<Action>().Init(this, status,p.GetComponent<ActionInfoPanel>());
        inQueueActions.Add(a.GetComponent<Action>());

        OpenQueuePanel();
    }

    /// <summary>0:BattleStart 1:RoundStart 2:TurnStart 3:ActivateAbility 4;TurnEnd 5:RoundEnd</summary>

    public void StartResolve(int mode)
    {
        resolveMode = mode;
        if (inQueueActions.Count > 0)
        {
            Debug.Log("resolveäJén");
            resolving = true;
        }
        else
        {
            EndResolve();
        }
    }

    public void Dequeue(string actionName)
    {
        Debug.Log(string.Format("{0}Çâåà", actionName));
        inQueueActions.RemoveAt(0);
        Destroy(content.transform.GetChild(0).gameObject);
        if (inQueueActions.Count == 0)
        {
            resolving = false;
            Debug.Log("resolveèIóπ");
            EndResolve();
            CloseQueuePanel();
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
}
