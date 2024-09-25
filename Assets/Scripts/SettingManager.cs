using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField,Header("\n\n")] float SEVolume_default;
    [SerializeField] Settings_Slider SEVolume;
    [SerializeField, Header("\n\n")] float BGMVolume_default;
    [SerializeField] Settings_Slider BGMVolume;
    [SerializeField, Header("\n\n")] float resolveSpeed_default;
    [SerializeField] Settings_Slider resolveSpeed;
    [SerializeField, Header("\n\n1:true 0:false")] int infoOnMouseover_default;
    [SerializeField] Setting_Toggle toggle_infoOnMouseover;

    [SerializeField] ActionQueueManager actionQueue;

    SoundManager soundManager;
    InfoText infoText;

    public static bool infoOnMouseover;
    void Start()
    {
        soundManager=FindObjectOfType<SoundManager>();
        infoText= FindObjectOfType<InfoText>();

        SEVolume.SetValue(PlayerPrefs.GetFloat("Settings_SEVolume", SEVolume_default));
        ApplySEVolume();

        BGMVolume.SetValue(PlayerPrefs.GetFloat("Settings_BGMVolume", BGMVolume_default));
        ApplyBGMVolume();

        resolveSpeed.SetValue(PlayerPrefs.GetFloat("Settings_resolveSpeed", resolveSpeed_default));
        ApplyResolveSpeed();

        toggle_infoOnMouseover.SetValue((PlayerPrefs.GetInt("Settings_infoOnMouseover", infoOnMouseover_default) == 1) ? true : false);
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

    public void ApplySEVolume()
    {
        soundManager.SetSEVolume(SEVolume.GetValue());
        SEVolume.SetValueText();
        PlayerPrefs.SetFloat("Settings_SEVolume", SEVolume.GetValue());
        PlayerPrefs.Save();
    }
    public void ApplyBGMVolume()
    {
        soundManager.SetBGMVolume(BGMVolume.GetValue());
        BGMVolume.SetValueText();
        PlayerPrefs.SetFloat("Settings_BGMVolume", BGMVolume.GetValue());
        PlayerPrefs.Save();
    }
    public void ApplyResolveSpeed()
    {
        actionQueue.SetResolveSpeed(resolveSpeed.GetValue());
        resolveSpeed.SetValueText();
        PlayerPrefs.SetFloat("Settings_resolveSpeed", resolveSpeed.GetValue());
        PlayerPrefs.Save();
    }
    public void ApplyInfoOnMouseOver()
    {
        infoOnMouseover = toggle_infoOnMouseover.GetValue();
        PlayerPrefs.SetInt("Settings_infoOnMouseover", (infoOnMouseover) ? 1 : 0);
        PlayerPrefs.Save();
    }
}
