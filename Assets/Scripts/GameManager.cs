using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using unityroom.Api;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   [SerializeField] bool doTutorial;//test
    public GameParams gp;
    public static GameParams gameParams;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Awake()
    {
        gameParams = gp;
        CheckInstance();
    }

    void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendScoreborad(int index, float score)
    {
        if (index == 1) { UnityroomApiClient.Instance.SendScore(index, score, ScoreboardWriteMode.HighScoreDesc); }
        else if (index == 2) { UnityroomApiClient.Instance.SendScore(index, score, ScoreboardWriteMode.HighScoreDesc); }
    }

    //=================================[testÅ®title]=============================================
    public void GoTotitleScene()
    {
        SceneManager.LoadScene("Title");
    }
    //=================================[titleÅ®expedition]=============================================
    public void  GoToExpeditionScene(bool tutorial)
    {
        doTutorial = tutorial;

        SceneManager.LoadScene("Expedition");
    }

    /// <summary>íTçıâÊñ Ç©ÇÁíTçıåãâ âÊñ Ç…éùÇøâzÇ∑ì‡óe</summary>
    [System.Serializable]
    public class ExpeditionToResult
    {
        public bool survive;
        public List<Definer.Item> materials;
    }
    ExpeditionToResult resultParams;

    public void GoToResultScene(bool survive)
    {
        resultParams = new ExpeditionToResult();
        resultParams.survive = survive;
        resultParams.materials = FindObjectOfType<Inventory>().GetMaterials();

        SceneManager.LoadScene("TestResult");//test
    }
    public ExpeditionToResult GetExpeditionToResult() { return resultParams; }

    public void SetTutorialMode(bool f) { doTutorial = f; }
    public bool DoTutorial() { return doTutorial; }
}
