using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideMessageText : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDelay;
    [SerializeField] float fadeDuration;
    [SerializeField] TextMeshProUGUI text;
   public void Init(string s)
    {
        text.text = s;

        StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(fadeDelay);
        var wait = new WaitForSeconds(fadeDuration / 20f);
        for(int i = 0; i < 20; i++)
        {
            yield return wait;
            canvasGroup.alpha -= 0.05f;
        }

        Destroy(gameObject);
    }
}
