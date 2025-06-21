using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    //[SerializeField,Header("\n\n")] float SEVolume_default;
    //[SerializeField] Settings_Slider SEVolume;
    //[SerializeField, Header("\n\n")] float BGMVolume_default;
    //[SerializeField] Settings_Slider BGMVolume;
    //[SerializeField, Header("\n\n")] float resolveSpeed_default;
    //[SerializeField] Settings_Slider resolveSpeed;
    //[SerializeField, Header("\n\n1:true 0:false")] int infoOnMouseover_default;
    //[SerializeField] Setting_Toggle toggle_infoOnMouseover;

    [SerializeField] ActionQueueManager actionQueue;

    SoundManager soundManager;
    InfoText infoText;
    ExpeditionManager expeditionManager;

    [System.Serializable]
    public class SettingParams
    {
        public string key;
        public SettingsOption setting;
    }
    [Header("Settings_[key] Ç∆Ç»ÇÈ")] public List<SettingParams> settings;

    public static bool infoOnMouseover;
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        infoText = InfoText.inst;
        expeditionManager = ExpeditionManager.inst;

        //SEVolume.SetValue(PlayerPrefs.GetFloat("Settings_SEVolume", SEVolume_default));
        //ApplySEVolume();

        //BGMVolume.SetValue(PlayerPrefs.GetFloat("Settings_BGMVolume", BGMVolume_default));
        //ApplyBGMVolume();

        //resolveSpeed.SetValue(PlayerPrefs.GetFloat("Settings_resolveSpeed", resolveSpeed_default));
        //ApplyResolveSpeed();
        foreach (var setting in settings)
        {
            setting.setting.Init(setting.key,SetValue);
        }

        soundManager.SetBGMVolume(settings[0].setting.GetValue());
        soundManager.SetSEVolume(settings[1].setting.GetValue());
        actionQueue.SetResolveSpeed(settings[2].setting.GetValue());
        expeditionManager.Setting_ResetPos(settings[3].setting.GetValue() == 1);
        BattleManager.inst.Settings_DisplayInfoOnTS(settings[4].setting.GetValue() == 1);
    }

    public void SettingsButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText_Old("ê›íË", "ê›íËâÊñ ÇäJÇ≠");
        }
        if (Input.GetMouseButtonDown(0))
        {
            panel.SetActive(!panel.activeSelf);
            if (!panel.activeSelf) { PlayerPrefs.Save(); }
        }
    }

    //public void ApplySEVolume()
    //{
    //    soundManager.SetSEVolume(SEVolume.GetValue());
    //    SEVolume.SetValueText();
    //    PlayerPrefs.SetFloat("Settings_SEVolume", SEVolume.GetValue());
    //}
    //public void ApplyBGMVolume()
    //{
    //    soundManager.SetBGMVolume(BGMVolume.GetValue());
    //    BGMVolume.SetValueText();
    //    PlayerPrefs.SetFloat("Settings_BGMVolume", BGMVolume.GetValue());
    //    PlayerPrefs.Save();
    //}
    //public void ApplyResolveSpeed()
    //{
    //    actionQueue.SetResolveSpeed(resolveSpeed.GetValue());
    //    resolveSpeed.SetValueText();
    //    PlayerPrefs.SetFloat("Settings_resolveSpeed", resolveSpeed.GetValue());
    //    PlayerPrefs.Save();
    //}
    //public void ApplyInfoOnMouseOver()
    //{
    //    infoOnMouseover = toggle_infoOnMouseover.GetValue();
    //    PlayerPrefs.SetInt("Settings_infoOnMouseover", (infoOnMouseover) ? 1 : 0);
    //    PlayerPrefs.Save();
    //}

    /// <summary>valueÇÕsliderÇ≈Ç»Ç≠é¿ç€ÇÃíl</summary>
    public void SetValue(float value, string key)
    {
        SettingParams setting = new SettingParams();
        if (key == settings[0].key)//BGMVolume
        {
            setting = settings[0];
            soundManager.SetBGMVolume(value);
        }
        else if (key == settings[1].key)//SEVolume
        {
            setting = settings[1];
            soundManager.SetSEVolume(value);
        }
        else if (key == settings[2].key)//resolveSpeed
        {
            setting = settings[2];
            actionQueue.SetResolveSpeed(value);
        }
        else if (key == settings[3].key)//resetPos
        {
            setting = settings[3];
            expeditionManager.Setting_ResetPos(value == 1);
        }
        else if (key == settings[4].key)//displayInfoOnTS
        {
            setting = settings[4];
            BattleManager.inst.Settings_DisplayInfoOnTS(value == 1);
        }


        else
        {
            infoText.AddErrorText("ê›íËÇÃkeyÇ™ä‘à·Ç¡ÇƒÇ¢Ç‹Ç∑");
            return;
        }

        PlayerPrefs.SetFloat($"Settings_{setting.key}", value);
    }
}
