using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   [SerializeField] bool doTutorial;//test

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Awake()
    {
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
