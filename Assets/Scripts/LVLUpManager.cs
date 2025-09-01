using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LVLUpManager : MonoBehaviour
{
    public static LVLUpManager inst;
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

    Sequence sequence;


    bool inLVLUp;

    private void Awake()
    {
        if(inst == null) inst = this;
    }

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
        foreach(Ability.AbilityStatus status in LVLUpQueue[0].CharaStatus().abilitiesStatus)
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

        Character.CharacterStatus charaStatus = LVLUpQueue[0].CharaStatus();
        lvlUpTitle.text = string.Format("{0} LVLUP!! {1}->{2}", charaStatus.charaName, charaStatus.level, charaStatus.level + 1);
        string growthStr = ExpeditionManager.inst.playerStatusGrowth.GetLVLUPInfo(charaStatus.level + 1, true, charaStatus.maxHP_base, charaStatus.ATK_base, charaStatus.INT_base);

        statusGrowthText.text = growthStr;
        if (sequence != null) sequence.Kill(true);
        sequence = DOTween.Sequence();
        //for (int i = 0; i < 3; i++)
        //{
        //    sequence.Join(panelsTF[i].DORotate(new Vector3(0, -90, 0), 0.5f, RotateMode.WorldAxisAdd).SetDelay(0.15f));
        //}
        sequence.Join(panelsTF[0].DORotate(new Vector3(0, -90, 0), 0.5f, RotateMode.WorldAxisAdd));
        sequence.Join(panelsTF[1].DORotate(new Vector3(0, -90, 0), 0.5f, RotateMode.WorldAxisAdd).SetDelay(0.15f));
        sequence.Join(panelsTF[2].DORotate(new Vector3(0, -90, 0), 0.5f, RotateMode.WorldAxisAdd).SetDelay(0.3f));
        sequence.Play();
    }
    //IEnumerator SelectUpgradeC()
    //{
    //    Character.CharacterStatus charaStatus = LVLUpQueue[0].CharaStatus();
    //    lvlUpTitle.text = string.Format("{0} LVLUP!! {1}->{2}", charaStatus.charaName, charaStatus.level, charaStatus.level + 1);
    //    string growthStr = ExpeditionManager.inst.playerStatusGrowth.GetLVLUPInfo(charaStatus.level + 1, true, charaStatus.maxHP_base, charaStatus.ATK_base, charaStatus.INT_base);
        
    //    statusGrowthText.text = growthStr;
    //    if (sequence != null) sequence.Kill(true);
    //    sequence = DOTween.Sequence();
    //    for (int i = 0; i < 3; i++)
    //    {
    //        sequence.Append(panelsTF[i].DORotate(new Vector3(0, -90, 0), 0.5f,RotateMode.WorldAxisAdd));
    //        sequence.AppendInterval(0.15f);
    //    }
    //    sequence.Play();
    //}

    public void EndSelectUpgrade()
    {
        charaDetail_lvlUp.SetValue();
        LVLUpQueue[0].DisplayInfo();
        LVLUpQueue.RemoveAt(0);
        StartCoroutine(EndSelectUpgradeC());
    }
    IEnumerator EndSelectUpgradeC()
    {
        if (sequence != null) sequence.Kill(true);
        sequence = DOTween.Sequence();
        //for (int i = 0; i < 3; i++)
        //{
        //    sequence.Join(panelsTF[i].DORotate(new Vector3(0, -90, 0), 0.5f, RotateMode.WorldAxisAdd).SetDelay(0.15f));
        //}
        sequence.Join(panelsTF[0].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.WorldAxisAdd));
        sequence.Join(panelsTF[1].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.WorldAxisAdd).SetDelay(0.15f));
        sequence.Join(panelsTF[2].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.WorldAxisAdd).SetDelay(0.3f));
        sequence.Play();
        //for (int i = 0; i < 3; i++)
        //{
        //    panelsTF[i].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.WorldAxisAdd);
        //    yield return new WaitForSeconds(0.15f);
        //}
        yield return new WaitForSeconds(0.8f);
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
