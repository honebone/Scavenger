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

    // 更新が必要かどうかのフラグ
    private bool isDirty = false;

    // 更新頻度を制限するためのタイマー
    private float updateTimer = 0f;
    private const float UPDATE_INTERVAL = 0.1f;
    public void SetSaveData(string key,float value)
    {
        PlayerPrefs.SetFloat(key, value);
        isDirty = true;
    }

    private void Update()
    {
        // 更新が必要で、かつ更新間隔を過ぎている場合のみ更新
        if (isDirty)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer >= UPDATE_INTERVAL)
            {
                PlayerPrefs.Save();
                updateTimer = 0f;
                isDirty = false;
            }
        }
    }

    //=================================[test→title]=============================================
    public void GoTotitleScene()
    {
        SceneManager.LoadScene("Title");
    }
    //=================================[title→expedition]=============================================
    public void  GoToExpeditionScene(bool tutorial)
    {
        doTutorial = tutorial;

        SceneManager.LoadScene("Expedition");
    }

    /// <summary>探索画面から探索結果画面に持ち越す内容</summary>
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
