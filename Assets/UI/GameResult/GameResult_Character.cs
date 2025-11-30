using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameResult_Character : MonoBehaviour
{
    public Image charaImage;
    public TextMeshProUGUI lvlText;
    public List<Slider> DDMG;
    public List<Slider> RDMG;
    public List<Slider> RHeal;
    public List<TextMeshProUGUI> reportTexts;
    public List<Image> crowns;
    public void Init(PersonalBattleReport report,List<int> best)
    {
        charaImage.sprite = report.chara.CharaStatus().characterData.spriteForUI;
        lvlText.text = $"LVL {report.chara.CharaStatus().level}";
        //与ダメージ
        DDMG[0].maxValue = best[0];
        DDMG[0].value = report.ATKDMG;

        DDMG[1].maxValue = best[0];
        DDMG[1].value = report.ATKDMG+report.INTDMG;

        DDMG[2].maxValue = best[0];
        DDMG[2].value = report.ATKDMG + report.INTDMG+report.decreaseHP;

        reportTexts[0].text = (report.ATKDMG + report.INTDMG + report.decreaseHP).ToString();
        crowns[0].enabled = report.ATKDMG + report.INTDMG + report.decreaseHP == best[0];

        //被ダメージ
        RDMG[0].maxValue = best[1];
        RDMG[0].value = report.RDMG;

        RDMG[1].maxValue = best[1];
        RDMG[1].value = report.RDMG + report.RShieldDMG;

        reportTexts[1].text = (report.RDMG + report.RShieldDMG).ToString();
        crowns[1].enabled = report.RDMG + report.RShieldDMG == best[1];

        //回復
        RHeal[0].maxValue = best[2];
        RHeal[0].value = report.GHeal;

        RHeal[1].maxValue = best[2];
        RHeal[1].value = report.GHeal + report.GShield;

        reportTexts[2].text = (report.GHeal + report.GShield).ToString();
        crowns[2].enabled = report.GHeal + report.GShield == best[2];

    }
}
