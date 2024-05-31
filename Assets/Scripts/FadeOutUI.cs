using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeOutUI : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    private void Awake()
    {
        Color c = panel.GetComponent<Image>().color;
        c.a = 1;
        panel.GetComponent<Image>().color = c;
    }
    /// <summary>0.4s‚ÅŠ®—¹</summary>
    public void FadeOut()
    {
        StartCoroutine("FadingOut");
    }

    IEnumerator FadingOut()
    {
        for (int i = 0; i < 5; i++)
        {
            Color c = panel.GetComponent<Image>().color;
            c.a += 0.2f;
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.08f);
        }
        Color e = panel.GetComponent<Image>().color;
        e.a = 1;
        panel.GetComponent<Image>().color = e;
    }
    /// <summary>0.4s‚ÅŠ®—¹</summary>
    public void FadeIn()
    {
        StartCoroutine("FadingIn");
    }

    IEnumerator FadingIn()
    {
        for (int i = 0; i < 5; i++)
        {
            Color c = panel.GetComponent<Image>().color;
            c.a -= 0.2f;
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.08f);
        }
        Color e = panel.GetComponent<Image>().color;
        e.a = 0;
        panel.GetComponent<Image>().color = e;
    }
}
