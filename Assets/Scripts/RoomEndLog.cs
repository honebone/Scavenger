using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomEndLog : MonoBehaviour
{
    protected RoomEndLogManager manager;
    protected SoundManager soundManager;

    public CanvasGroup canvas;

    Sequence sequence;
    public void Init(RoomEndLogManager relm)
    {
        manager = relm;
        soundManager = SoundManager.instance;
    }
    public void LogStart(bool first)
    {
        //gameObject.SetActive(true);
        gameObject.SetActive(true);

        if (first) OnLogStart();
        else FadeIn();
    }
    public virtual void OnLogStart()
    {

    }
    float move = 25f;

    public void FadeIn()
    {
        transform.localPosition = new Vector3(0, -move, 0);
        canvas.alpha = 0;
        if (sequence != null) sequence.Kill(true);
        sequence = DOTween.Sequence();

        sequence.Append(canvas.DOFade(1, 0.25f));
        sequence.Join(transform.DOLocalMoveY(move, 0.25f)).SetRelative(true);

        sequence.Play();
    }
    public void FadeOut()
    {
        if (sequence != null) sequence.Kill(true);
        sequence = DOTween.Sequence();

        sequence.Append(canvas.DOFade(0, 0.25f));
        sequence.Join(transform.DOLocalMoveY(move, 0.25f)).SetRelative(true);

        sequence.Play().OnComplete(() =>
        {
            FadeOutEnd();
        }).OnKill(() =>
        {
            FadeOutEnd();
        });
    }

    void FadeOutEnd()
    {
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        canvas.alpha = 1;
    }
}
