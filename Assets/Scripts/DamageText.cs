using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageText : MonoBehaviour
{
    [SerializeField]
    Text text;
    [SerializeField]
    float duration = 1.25f;

    public void Init(string s,Color color)
    {
        text.text = s;
        text.color = color;
        StartCoroutine(FadeAway());
    }
    IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(duration);
        Color c = text.color;
        while (c.a > 0)
        {
            yield return new WaitForSeconds(0.2f);
            c.a -= 0.2f;
            text.color = c;
        }
        Destroy(gameObject);
    }
}
