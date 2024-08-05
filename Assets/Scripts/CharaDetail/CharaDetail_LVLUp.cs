using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CharaDetail_LVLUp : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] CharaDetail_ExpBar expBar;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] TextMeshProUGUI currentLVLText;
    [SerializeField] TextMeshProUGUI nextLVLText;
    [SerializeField] Image expBar_fill;
    [SerializeField] GameObject lvlMaxPanel;

    [SerializeField] CharaDetailUI detailUI;
    [SerializeField] InfoText infoText;
    [SerializeField] MouseOverUI mouseOver;
    [SerializeField] Inventory inventory;
    [SerializeField] GuideMessage guideMessage;
    [SerializeField] LVLUpManager lvlUpManager;

    Character character;

    public void OpenPanel()
    {
        panel.SetActive(true);

        SetValue();
    }
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void GainEXP()
    {
        if (!lvlUpManager.GetInLVLUp())
        {
            if (inventory.GetExp() > 0)
            {
                inventory.AddExp(-1, false);
                character.GainEXP(1);
                SetValue(false);
            }
            else
            {
                guideMessage.SetWaringText("経験のオーブが足りない");
            }
        }
    }

    public void SetValue(bool fromOrigin = true)
    {
        expText.text = string.Format("x{0}", inventory.GetExp());
        if (detailUI.GetDisplayingChara())
        {
            character = detailUI.GetDisplayingChara();
            Character.CharacterStatus status = character.GetCharacterStatus();
            currentLVLText.text = status.level.ToString();
            nextLVLText.text = string.Format("次のLVLまで：{0}/{1}", status.exp, status.GetNextExp());

            lvlMaxPanel.SetActive(status.level == 10);

            if (fromOrigin) { expBar_fill.fillAmount = 0; }
            float to = status.exp / (1f * status.GetNextExp());
            DOTween.To(() => expBar_fill.fillAmount, (x) => expBar_fill.fillAmount = x, to, 0.1f);
        }
    }

    public void ResetValue()
    {
        lvlMaxPanel.SetActive(false);
        expText.text = "";
        if (detailUI.GetDisplayingChara())
        {
            character = null;
            currentLVLText.text = "";
            nextLVLText.text = "";

            expBar_fill.fillAmount = 0;
        }
    }

    //public void SetChara(Character chara)
    //{

    //}
}
