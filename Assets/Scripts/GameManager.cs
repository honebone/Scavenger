using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    public class ExpeditionToResult
    {

    }
}
