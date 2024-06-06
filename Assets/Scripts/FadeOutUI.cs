using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeOutUI : MonoBehaviour
{
    [SerializeField] bool blockOnAwake=true;

[SerializeField] CanvasGroup canvas;
    private void Awake()
    {
        if (blockOnAwake)
        {
            canvas.alpha = 1;
        }
    }
    /// <summary>0.4s‚ÅŠ®—¹</summary>
    public void FadeOut()
    {
        StartCoroutine(FadingOut(0.4f));
    }
    public void FadeOut_SetDuration(float duration)
    {
        StartCoroutine(FadingOut(duration));
    }

    IEnumerator FadingOut(float duration)
    {
        var wait = new WaitForSeconds(duration / 10f);
        for (int i = 0; i < 10; i++)
        {
            canvas.alpha += 0.1f;
            yield return wait;
        }
        canvas.alpha = 1;
    }
    /// <summary>0.4s‚ÅŠ®—¹</summary>
    public void FadeIn()
    {
        StartCoroutine(FadingIn(0.4f));
    }
    public void FadeIn_SetDuration(float duration)
    {
        StartCoroutine(FadingIn(duration));
    }

    IEnumerator FadingIn(float duration)
    {
        var wait = new WaitForSeconds(duration / 10f);
        for (int i = 0; i < 10; i++)
        {
            canvas.alpha -= 0.1f;
            yield return wait;
        }
        canvas.alpha = 0;
    }
}
