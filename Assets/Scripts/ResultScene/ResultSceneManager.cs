using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]//test
    GameManager.ExpeditionToResult result;
    [SerializeField]
    GameObject resultPanel;

    [SerializeField, Header("<<持ち帰った素材>>")]
    GameObject materialIcon;
    [SerializeField]
    Transform materialIconsP;

    enum resultPhase { other, materials, quests }
    resultPhase phase = resultPhase.other;
    Coroutine coroutine;
    int maxState = 1;
    int currentState = 1;

    void Start()
    {
        Init(FindObjectOfType<GameManager>().GetExpeditionToResult());
    }

    public void Init(GameManager.ExpeditionToResult r)
    {
        result = r;
        coroutine = StartCoroutine(StartRevealingMaterial());//test
    }

    public void ChangePhase(bool forward)
    {
        if (forward)
        {
            currentState++;
            if (maxState < currentState)//表示開始
            {
                switch (currentState)
                {
                    case 2:
                        //クエスト
                        break;
                    case 3:
                        //統計
                        break;
                }
                maxState = currentState;
            }
            resultPanel.transform.DOLocalMoveX(-800f, 0.5f).SetRelative(true);
        }
        else
        {
            currentState--;
            resultPanel.transform.DOLocalMoveX(800f, 0.5f).SetRelative(true);
        }
    }
    
    IEnumerator StartRevealingMaterial()
    {
        phase = resultPhase.materials;

        foreach(Definer.Item item in result.materials)
        {
            var m = Instantiate(materialIcon, materialIconsP);
            m.GetComponent<Result_MaterialIcon>().Init(item);
        }
        for(int i = 0; i < materialIconsP.childCount; i++)
        {
            materialIconsP.GetChild(i).GetComponent<Result_MaterialIcon>().Reveal();
            yield return new WaitForSeconds(0.25f);
        }
        phase = resultPhase.other;
    }

    IEnumerator DisplayCompleteQuests()
    {
        phase = resultPhase.quests;
        Debug.Log("ok");
        yield return new WaitForSeconds(1f);//test
        phase = resultPhase.other;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (phase)
            {
                case resultPhase.materials:
                    StopCoroutine(coroutine);
                    for (int i = 0; i < materialIconsP.childCount; i++)
                    {
                        Result_MaterialIcon materialIcon = materialIconsP.GetChild(i).GetComponent<Result_MaterialIcon>();
                        if (!materialIcon.GetRevealed()) { materialIcon.Reveal(); }                      
                    }
                    break;
            }
            phase = resultPhase.other;
        }
    }
}
