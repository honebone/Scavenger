using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Settings_Slider SEVolume;
    [SerializeField] Settings_Slider BGMVolume;
    [SerializeField] Settings_Slider resolveSpeed;

    [SerializeField] ActionQueueManager actionQueue;

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
        soundManager.SetSEVolume(SEVolume.GetValue());
        SEVolume.SetValueText();
    }
    public void SetBGMVolume()
    {
        soundManager.SetBGMVolume(BGMVolume.GetValue());
        BGMVolume.SetValueText();
    }
    public void SetResolveSpeed()
    {
        actionQueue.SetResolveSpeed(resolveSpeed.GetValue());
        resolveSpeed.SetValueText();
    }
}
