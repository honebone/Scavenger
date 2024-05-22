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

    public void Init(Ability.AbilityStatus status, Character chara, GuideMessage gm, InfoText it,CharaDetailUI d,TextMeshProUGUI ui)
    {
        abilityStatus = status;
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
            if (abilityStatus.locked)
            {
                upgradeInfo.text = "アビリティを解放し、使用可能にする";
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
        if (abilityStatus.locked) { abilityStatus.Unlock(); }
        detailUI.Refresh();
    }
}
