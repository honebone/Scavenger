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

    public GameObject closeButton;

    ExpeditionManager.PartyStatus partyStatus;
    private void Awake()
    {
        if(inst == null)inst = this;
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

    public void SetResult(int mode)
    {
        panel.SetActive(true);

        titleText.text = mode == 0 ? "" : mode == 1 ? "GAME CLEAR!!".ColorStr(Color.yellow) : "GAME OVER".ColorStr(Definer.colorRef.damage);
        int time = (partyStatus.endTime == 0 ? Time.time - partyStatus.startTime : partyStatus.endTime - partyStatus.startTime).ToInt();
        text.text = $"“’B‚µ‚½ŠK‘wF‘æ{partyStatus.areaCount}ƒGƒŠƒA/‘æ{partyStatus.currentPos.x}ŠK‘w\n“|‚µ‚½“GF{partyStatus.killCount}‘Ì\nƒvƒŒƒCŠÔF{(time / 60):00}:{time % 60:00}";
        for(int i = 0; i < charasP.childCount; i++) { Destroy(charasP.GetChild(i).gameObject); }
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

    public void Close()
    {
        panel.SetActive(false);
    }
}
