using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Deploy_CharaButton : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI charaNameText;
    [SerializeField] Image charaImage;


    InfoText infoText;
    DeployCharacterManager deployCharacterManager;
    MouseOverUI mouseOver;

    Character.CharacterStatus charaStatus;
    public void Init(Character.CharacterStatus status,InfoText it,DeployCharacterManager dc,MouseOverUI mo)
    {
        charaStatus = status;
        infoText = it;
        deployCharacterManager = dc;
        mouseOver = mo;
        //scroll = s;

        //charaNameText.text = charaStatus.charaName;
        charaImage.sprite = charaStatus.spriteForUI;
    }
    public void Anim()
    {
        charaImage.transform.DOLocalMoveX(0, 0.5f);
    }

    public void SetDragChara()
    {
        infoText.SetText(charaStatus.charaName, charaStatus.characterData.GetInfo(false), charaStatus.characterData.GetInfo(true));
        FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons_Deploy(charaStatus.abilitiesStatus);

        if (Input.GetMouseButtonDown(0))
        {
            deployCharacterManager.SetDraggingChara(charaStatus);
        }
    }
    //[SerializeField]
    //float scrollSpeed = 0.1f;
    //ScrollRect scroll;
    //float wheel;
    //bool p;
    private void Update()
    {
        //if (p)
        //{
        //    wheel += Input.mouseScrollDelta.y;
        //    if (wheel != 0)
        //    {
        //        scroll.verticalNormalizedPosition += wheel * scrollSpeed;
        //        wheel = 0;
        //    }
        //}
    }
    public void OnMouseEnter()
    {
        SoundManager.instance.PlaySE_MO();

        // p = true;
        string info = $"<{charaStatus.charaName}>\n\n{charaStatus.characterData.GetRoleInfo()}\n";
        info += string.Format("使用難易度：{0}\n", charaStatus.characterData.difficulty);
        info += $"得意な列：{charaStatus.characterData.GetPreferedPos()}列\n\n";
        info += string.Format("\"{0}\"\n\n", charaStatus.characterData.introduction).ColorStr(Definer.colorRef.emphasize);

        mouseOver.SetUI(info, true);
    }
    public void OnMouseExit()
    {
        //p = false;
        mouseOver.ResetUI();
    }
}
