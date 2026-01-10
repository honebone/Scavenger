using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameResultManager : MonoBehaviour
{
    public static GameResultManager inst;
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI text;
    public GameObject chara;
    public Transform charasP;
    public Image frame;

    public GameObject closeButton;
    public GameObject toTitleButton;
    public GameObject endlessButton;
    public ParticleSystem par_gameover;

    public AudioClip SE_gameclear;
    public AudioClip SE_gameover;

    ExpeditionManager.PartyStatus partyStatus;
    private void Awake()
    {
        if (inst == null) inst = this;
    }

    /// <summary>
    /// mode 0:statistics 1:clear 2:gameover
    /// </summary>
    /// <param name="mode"></param>
    public void Toggle(int mode)
    {
        if (panel.activeSelf) Close();
        else SetResult(mode);
    }

    public void SetPartyStatus(ExpeditionManager.PartyStatus status) { partyStatus = status; }

    /// <summary>
    /// mode 0:statistics 1:clear 2:gameover
    /// </summary>
    public void SetResult(int mode)
    {
        if (CharactersManager.inst.GetExistingCharacters_All().Count > 0)
        {
            panel.SetActive(true);

            titleText.text = mode == 0 ? "" : mode == 1 ? "GAME CLEAR!!".ColorStr(Color.yellow) : "GAME OVER".ColorStr(Definer.colorRef.damage);
            if (mode == 0) SoundManager.instance.PlaySE_Select();
            if (mode == 1) SoundManager.instance.PlaySE(SE_gameclear);
            if (mode == 2) SoundManager.instance.PlaySE(SE_gameover);
            if (mode == 2) par_gameover.Play();
            if (mode != 2) par_gameover.Stop();
            toTitleButton.SetActive(mode != 0);
            endlessButton.SetActive(mode == 1);
            frame.color = mode == 2 ? Definer.colorRef.damage : Color.white;

            int time = (partyStatus.endTime == 0 ? Time.time - partyStatus.startTime : partyStatus.endTime - partyStatus.startTime).ToInt();
            if(mode==1)GameManager.instance.SendScoreborad(1, time);    
            text.text = $"ìûíBÇµÇΩäKëwÅFëÊ{partyStatus.areaCount}ÉGÉäÉA/ëÊ{partyStatus.currentPos.x}äKëw\nì|ÇµÇΩìGÅF{partyStatus.killCount}ëÃ\nÉvÉåÉCéûä‘ÅF{(time / 60):00}:{time % 60:00}";
            for (int i = 0; i < charasP.childCount; i++) { Destroy(charasP.GetChild(i).gameObject); }
            List<int> best = new List<int>();
            best.Add(partyStatus.totalBattleReports.Max(x => x.ATKDMG + x.INTDMG + x.decreaseHP));
            best.Add(partyStatus.totalBattleReports.Max(x => x.RDMG + x.RShieldDMG));
            best.Add(partyStatus.totalBattleReports.Max(x => x.GHeal + x.GShield));
            partyStatus.totalBattleReports.ForEach(x =>
            {
                var c = Instantiate(chara, charasP);
                c.GetComponent<GameResult_Character>().Init(x, best);
            });

            closeButton.SetActive(mode == 0);
        }
    }

    public void Close()
    {
        SoundManager.instance.PlaySE_Select();
        panel.SetActive(false);
    }
    public void ToTitle()
    {
        StartCoroutine(ToTitleC());
    }
    IEnumerator ToTitleC()
    {
        FadeOutUI.inst.FadeOut();
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameManager>().GoTotitleScene();
    }
    public void Endless()
    {
        panel.SetActive(false);
        FadeOutUI.inst.FadeIn();
        ExpeditionManager.inst.EnterEndless();
    }
}
