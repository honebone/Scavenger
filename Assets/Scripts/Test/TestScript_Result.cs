using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TestScript_Result : MonoBehaviour
{
    [SerializeField] GameObject clear;
    [SerializeField] GameObject dead;
    GameManager.ExpeditionToResult result;

    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        result = gameManager.GetExpeditionToResult();
        if (result.survive) { clear.SetActive(true); }
        else { dead.SetActive(true); }

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        FindObjectOfType<FadeOutUI>().FadeIn_SetDuration(1);
        yield return new WaitForSeconds(1f);
        if (gameManager.CheckIfTutorialArea())
        {
            yield return new WaitForSeconds(3f);
            dead.transform.DOLocalMoveY(-2000, 25f).SetRelative(true);
            yield return new WaitForSeconds(16f);
            FindObjectOfType<GameManager>().GoTotitleScene();
        }
        else
        {
            yield return new WaitForSeconds(3f);
            FindObjectOfType<FadeOutUI>().FadeOut_SetDuration(1);
            yield return new WaitForSeconds(1f);
            FindObjectOfType<GameManager>().GoTotitleScene();
        }



    }
}
