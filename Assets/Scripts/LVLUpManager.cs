using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LVLUpManager : MonoBehaviour
{
    [SerializeField] ExpeditionManager expeditionManager;
    [SerializeField] GameObject panel;

    [SerializeField] TextMeshProUGUI lvlUpTitle;
    [SerializeField] List<LVLUp_AbilityButton> buttons;
    [SerializeField] TextMeshProUGUI statusGrowthText;
    [SerializeField] List<Transform> panelsTF;
    [SerializeField] CharaDetailUI charaDetail;
    [SerializeField] CharaDetail_LVLUp charaDetail_lvlUp;

    [SerializeField] AudioClip jingle_LVLUp;
    List<Character> LVLUpQueue = new List<Character>();

    SoundManager soundManager;

    bool inLVLUp;

    public struct LVLUpParams
    {
        public Character character;
        public bool upgrade;
        public Ability.AbilityStatus abilityStatus;
    }

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void LVLUp(Character chara)
    {
        LVLUpQueue.Add(chara);
        if (!inLVLUp)
        {
            inLVLUp = true;
            StartelectUpgrade();
        }
    }

    void StartelectUpgrade()
    {
        // charaDetail.CloseUI();
        soundManager.PlaySE(jingle_LVLUp);
        panel.SetActive(true);
        LVLUpQueue[0].DisplayInfo();

        List<LVLUpParams> pool = new List<LVLUpParams>();
        foreach(Ability.AbilityStatus status in LVLUpQueue[0].GetCharacterStatus().abilitiesStatus)
        {
            if(status.locked|| status.abilityData.upgradeAbility != null)
            {
                LVLUpParams lvlUpParams = new LVLUpParams();
                lvlUpParams.character = LVLUpQueue[0];
                if (status.locked)
                {
                    lvlUpParams.abilityStatus = status;
                }
                else if (status.abilityData.upgradeAbility != null)
                {
                    Ability.AbilityStatus upgrade = new Ability.AbilityStatus(status.abilityData.upgradeAbility, 0);
                    lvlUpParams.abilityStatus = upgrade;
                }
                lvlUpParams.upgrade = !status.locked;
                pool.Add(lvlUpParams);
            }
        }

        List<LVLUpParams> upgrades = pool.Sample(2);
        buttons[0].SetUpgrade(upgrades[0]);
        if (upgrades.Count == 2) { buttons[1].SetUpgrade(upgrades[1]); }
        else { buttons[1].SetUpgrade(upgrades[0]); }

        StartCoroutine(SelectUpgradeC());
    }
    IEnumerator SelectUpgradeC()
    {
        Character.CharacterStatus charaStatus = LVLUpQueue[0].GetCharacterStatus();
        lvlUpTitle.text = string.Format("{0} LVLUP!! {1}->{2}", charaStatus.charaName, charaStatus.level, charaStatus.level + 1);
        string growthStr = "";
        int HPGrowth = Mathf.CeilToInt(charaStatus.maxHP_base * (ExpeditionManager.playerMaxHPGrowth - 1));
        int ATKGrowth = Mathf.CeilToInt(charaStatus.ATK_base * (ExpeditionManager.playerATKGrowth - 1));
        int INTGrowth = Mathf.CeilToInt(charaStatus.INT_base * (ExpeditionManager.playerATKGrowth - 1));
        growthStr += $"Šî‘bHP+{HPGrowth}\nŠî‘bATK+{ATKGrowth}\nŠî‘bINT+{INTGrowth}\n";
        growthStr += LVLUpQueue[0].GetCharacterStatus().statusGrowth.GetInfo(charaStatus.level);

        statusGrowthText.text = growthStr;
        for (int i = 0; i < 3; i++)
        {
            panelsTF[i].DORotate(new Vector3(0, -90, 0), 0.5f,RotateMode.WorldAxisAdd);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void EndSelectUpgrade()
    {
        charaDetail_lvlUp.SetValue();
        LVLUpQueue[0].DisplayInfo();
        LVLUpQueue.RemoveAt(0);
        StartCoroutine(EndSelectUpgradeC());
    }
    IEnumerator EndSelectUpgradeC()
    {
       
        for (int i = 0; i < 3; i++)
        {
            panelsTF[i].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.WorldAxisAdd);
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(0.5f);
        lvlUpTitle.text = "";
        statusGrowthText.text = "";
        if (LVLUpQueue.Count > 0) { StartelectUpgrade(); }
        else
        {
            charaDetail.Refresh();
            panel.SetActive(false);
            inLVLUp = false;
        }
    }

    public bool GetInLVLUp() { return inLVLUp; }
}
