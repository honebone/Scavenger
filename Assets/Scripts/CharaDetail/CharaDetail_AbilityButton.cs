using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharaDetail_AbilityButton : MonoBehaviour
{

    [SerializeField]
    Sprite[] frames;
    [SerializeField]
    float holdDuration;

    [SerializeField]
    Image frame;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image holdGauge;

    Ability.AbilityStatus abilityStatus;
    Character character;
    GuideMessage guideMessage;
    CharaDetailUI detailUI;
    InfoText infoText;
    TextMeshProUGUI upgradeInfo;
    Inventory inventory;

    Coroutine hold;
    /// <summary>0:unlock 1:upgrade</summary>
    int upgradeMode;

    public void Init(Ability.AbilityStatus status,int mode, Character chara, GuideMessage gm, InfoText it,CharaDetailUI d,TextMeshProUGUI ui)
    {
        abilityStatus = status;
        upgradeMode = mode;
        character = chara;
        guideMessage = gm;
        infoText = it;
        detailUI = d;
        upgradeInfo = ui;
        inventory = FindObjectOfType<Inventory>();

        nameText.text = abilityStatus.abilityName;
        frame.sprite = frames[(int)abilityStatus.abilityType];
        frame.color = Definer.colorRef.abilityColors[(int)abilityStatus.abilityType];
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (upgradeMode==0)
            {
                upgradeInfo.text = "アビリティを解放し、使用可能にする";
            }
            else
            {
                upgradeInfo.text = string.Format("アビリティを強化する\n\n{0}", abilityStatus.abilityData.upgradeInfo);
            }
            infoText.SetText(abilityStatus.abilityName.ColorStr(Definer.colorRef.abilityColors[(int)abilityStatus.abilityType])
                , abilityStatus.GetInfo(false, new Character.CharacterStatus()));
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (FindObjectOfType<ExpeditionManager>().CheckInRoomEvent())
            {
                guideMessage.SetWaringText("イベント中のアップグレード不可");
            }
            else if (inventory.GetExp() == 0)
            {
                guideMessage.SetWaringText("経験のオーブが足りない");
            }
            else
            {
                hold = StartCoroutine(Hold());
            }
        }
    }

    public void EndHold()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (hold != null) { StopCoroutine(hold); }
            holdGauge.fillAmount = 0;
        }
    }

    IEnumerator Hold()
    {
        var wait = new WaitForSeconds(holdDuration / 20f);
        for (int i = 0; i < 20; i++)
        {
            yield return wait;
            holdGauge.fillAmount += 0.05f;
        }
        inventory.RemoveExp(1, true);
        if (upgradeMode == 0) { abilityStatus.Unlock(); }
        else { character.UpgradeAbility(abilityStatus.abilityData); }
        detailUI.Refresh();
    }
}
