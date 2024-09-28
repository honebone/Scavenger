using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class TtileSceneManager : MonoBehaviour
{
    [SerializeField] CanvasGroup textMask;
    [SerializeField] Light2D sunLight;
    [SerializeField] Toggle skipTutorial;

    public static bool skipTutorial_bool;

    GameManager gameManager;

    bool canStart;

    public void CanStart()
    {
        canStart = true;
        textMask.alpha = 1;
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        skipTutorial_bool = PlayerPrefs.GetInt("Setting_skipTutorial", 0) == 1;
        skipTutorial.isOn = skipTutorial_bool;
        //gameManager.SetTutorialMode(true);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (canStart&&Input.GetMouseButtonDown(0))
    //    {
    //        StartCoroutine(StartExpedition());
    //    }
    //}

    IEnumerator StartExpedition()
    {
        FindObjectOfType<FadeOutUI>().FadeOut_SetDuration(1);
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            sunLight.intensity -=0.126f;
        }
        yield return new WaitForSeconds(1f);
        gameManager.GoToExpeditionScene(!skipTutorial.isOn);
    }

    public void ToggleTutorial() { PlayerPrefs.SetInt("Setting_skipTutorial", (skipTutorial.isOn) ? 1 : 0); }
    public void StartGame()
    {
        if (canStart)
        {
            canStart = false;
            StartCoroutine(StartExpedition());
        }
    }
}
