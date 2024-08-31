using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float duration = 1.25f;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] Vector2 endPos;

    public void Init(string s,Color color,bool player)
    {
        int n = player ? 1 : -1;

        text.text = s;
        text.color = color;

        var textSeq = DOTween.Sequence();

        
        textSeq.Append(text.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutQuint));
        textSeq.Append(canvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        }));
        textSeq.Join(text.transform.DOLocalMove(endPos * n, duration).SetRelative(true));//.SetEase(Ease.InBack)

        //StartCoroutine(FadeAway());
    }
    //IEnumerator FadeAway()
    //{
    //    yield return new WaitForSeconds(duration);
    //    Color c = text.color;
    //    while (c.a > 0)
    //    {
    //        yield return new WaitForSeconds(0.2f);
    //        c.a -= 0.2f;
    //        text.color = c;
    //    }
    //    Destroy(gameObject);
    //}
}
