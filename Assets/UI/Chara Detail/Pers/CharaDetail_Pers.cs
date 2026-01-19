using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.TextCore.Text;

public class CharaDetail_Pers : MonoBehaviour
{
    public static CharaDetail_Pers inst;

    void Awake()
    {
        if (inst == null) inst = this;
    }
    [SerializeField] GameObject perButton;
    [SerializeField] GameObject perTypeText;
    [SerializeField] Transform persP;
    [SerializeField] ScrollRect scroll;
    [SerializeField] TextMeshProUGUI charaName;
    [SerializeField] Image charaIcon;
    Character current;

    List<PA_Personality.PersonalityStatus.PersonalityType> goodPers = new List<PA_Personality.PersonalityStatus.PersonalityType>() { PA_Personality.PersonalityStatus.PersonalityType.awoken
        , PA_Personality.PersonalityStatus.PersonalityType.good };
    List<PA_Personality.PersonalityStatus.PersonalityType> otherPers = new List<PA_Personality.PersonalityStatus.PersonalityType>() { PA_Personality.PersonalityStatus.PersonalityType.neutral
        , PA_Personality.PersonalityStatus.PersonalityType.mutation,PA_Personality.PersonalityStatus.PersonalityType.affricted };

    public void SetChara(Character chara)
    {
        current = chara;
        for (int i = 0; i < persP.childCount; i++) { Destroy(persP.GetChild(i).gameObject); }
        List<PA_Personality> list = new List<PA_Personality>();

        list = current.GetPers(PA_Personality.PersonalityStatus.PersonalityType.unique);
        if(list.Count > 0)
        {
            Instantiate(perTypeText, persP).GetComponentInChildren<TextMeshProUGUI>().text = "==ŒÅ—L“Á«==";
            list.ForEach(p =>
            {
                var b = Instantiate(perButton, persP);
                b.GetComponent<CharaDetail_PerButton>().Init(p);
                b.GetComponent<ScrollManager>().SetScroll(scroll);
            });
        }

        list = current.GetPers(goodPers);
        if (list.Count > 0)
        {
            Instantiate(perTypeText, persP).GetComponentInChildren<TextMeshProUGUI>().text = $"=={"—Ç‚¢“Á«".ColorStr(Definer.colorRef.personalityColors[3])} {list.Count}/{GameManager.gameParams.maxPer_good}==";
            list.ForEach(p =>
            {
                var b = Instantiate(perButton, persP);
                b.GetComponent<CharaDetail_PerButton>().Init(p);
                b.GetComponent<ScrollManager>().SetScroll(scroll);
            });
        }


        list = current.GetPers(PA_Personality.PersonalityStatus.PersonalityType.bad);
        if (list.Count > 0)
        {
            Instantiate(perTypeText, persP).GetComponentInChildren<TextMeshProUGUI>().text = $"=={"ˆ«‚¢“Á«".ColorStr(Definer.colorRef.personalityColors[4])} {list.Count}/{GameManager.gameParams.maxPer_bad}==";
            list.ForEach(p =>
            {
                var b = Instantiate(perButton, persP);
                b.GetComponent<CharaDetail_PerButton>().Init(p);
                b.GetComponent<ScrollManager>().SetScroll(scroll);
            });
        }

        list = current.GetPers(otherPers);
        if(list.Count > 0)
        {
            Instantiate(perTypeText, persP).GetComponentInChildren<TextMeshProUGUI>().text = "==‚»‚Ì‘¼“Á«==";
            list.ForEach(p =>
            {
                var b = Instantiate(perButton, persP);
                b.GetComponent<CharaDetail_PerButton>().Init(p);
                b.GetComponent<ScrollManager>().SetScroll(scroll);
            });
        }

        charaName.text = current.CharaStatus().charaName;
        charaIcon.sprite = current.CharaStatus().characterData.spriteForUI;
    }
}
