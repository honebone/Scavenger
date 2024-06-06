using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Slider SEVolume;
    [SerializeField] Slider BGMVolume;

    SoundManager soundManager;
    InfoText infoText;
    void Start()
    {
        soundManager=FindObjectOfType<SoundManager>();
        infoText= FindObjectOfType<InfoText>();
    }

    public void SettingsButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText("ê›íË", "ê›íËâÊñ ÇäJÇ≠");
        }
        if (Input.GetMouseButtonDown(0))
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    public void SetSEVolume()
    {
        soundManager.SetSEVolume(SEVolume.value);
    }
    public void SetBGMVolume()
    {
        soundManager.SetBGMVolume(BGMVolume.value);
    }
}
