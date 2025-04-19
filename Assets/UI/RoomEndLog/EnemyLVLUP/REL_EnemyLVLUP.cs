using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class REL_EnemyLVLUP : RoomEndLog
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    public CanvasGroup titleCanvas;
    public CanvasGroup infoCanvas;

    public AudioClip SE;

   
    public override void OnLogStart()
    {
        StartCoroutine(EnemyLVLUpC());
    }

    IEnumerator EnemyLVLUpC()
    {
        string info = ExpeditionManager.inst.EnemyLVLUP();

        soundManager.StopBGMs();
        soundManager.PlaySE(SE);

        titleText.text = "ìGÇÃLVLÇ™è„è∏";
        titleCanvas.DOFade(1, 1f);
        yield return new WaitForSeconds(1.5f);
        infoText.text = info;
        infoCanvas.DOFade(1, 1f);
        yield return new WaitForSeconds(2f);

        soundManager.PlayBGM_Normal();
        manager.LogEnd();
    }
}
