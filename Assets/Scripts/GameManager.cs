using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    /// <summary>’Tõ‰æ–Ê‚©‚ç’TõŒ‹‰Ê‰æ–Ê‚É‚¿‰z‚·“à—e</summary>
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

        SceneManager.LoadScene("Result");
    }
    public ExpeditionToResult GetExpeditionToResult() { return resultParams; }
}
