using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_SelectTutorial : MonoBehaviour
{
    public void SkipTutorial()
    {
        StartCoroutine(SkipTutorialC());
    }
    IEnumerator SkipTutorialC()
    {
        FindObjectOfType<FadeOutUI>().FadeOut();
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameManager>().GoTotitleScene();
    }
    public void PlayTutorial()
    {
        StartCoroutine(PlayTutorialC());
    }
    IEnumerator PlayTutorialC()
    {
        FindObjectOfType<FadeOutUI>().FadeOut();
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameManager>().GoToExpeditionScene(true);
    }
}
