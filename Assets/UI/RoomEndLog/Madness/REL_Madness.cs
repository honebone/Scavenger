using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class REL_Madness : RoomEndLog
{
    public float startDelay;
    public float fillDur;
    public float eyeDelay;
    public float eyeDur;
    public float infoFadeDelay;
    public float infoFadeDur;
    public float endDelay;

    public Image fill;
    public List<float> fillAmount;
    public List<Image> eyes;
    public List<Sprite> eye_sprites;
    public TextMeshProUGUI infoText;
    public CanvasGroup infoCanvas;


    public AudioClip SE;
    int madnessCount;
    GameObject madnessPA;

    public void Init_Madness(int m, GameObject PA)
    {
        madnessCount = m;
        madnessPA = PA;
        if (madnessCount >= 2)
        {
            fill.fillAmount = fillAmount[madnessCount - 2];
            for (int i = 0; i < madnessCount - 1; i++)
            {
                eyes[i].sprite = eye_sprites[4];
            }
        }
    }

    public override void OnLogStart()
    {
        StartCoroutine(EnemyLVLUpC());
    }

    IEnumerator EnemyLVLUpC()
    {
        soundManager.StopBGMs();
        yield return new WaitForSeconds(startDelay);

        PA_Personality pa = madnessPA.GetComponent<PA_Personality>();
        string info = ExpeditionManager.inst.GetMadnessInfo(pa);
        DOTween.To(() => fill.fillAmount, (x) => fill.fillAmount = x, fillAmount[madnessCount - 1], fillDur);

        yield return new WaitForSeconds(fillDur + eyeDelay);
        soundManager.PlaySE(SE);

        var eyeWait = new WaitForSeconds(eyeDur / 4f);
        for (int i = 1; i < 5; i++)
        {
            eyes[madnessCount - 1].sprite = eye_sprites[i];
            yield return eyeWait;
        }

        yield return new WaitForSeconds(infoFadeDelay);

        infoText.text = info;
        infoCanvas.DOFade(1, infoFadeDur);

        yield return new WaitForSeconds(infoFadeDur + endDelay);
        soundManager.PlayBGM_Normal();
        TutorialManager.inst.SetTutorial("madness");

        manager.LogEnd();
    }
}
