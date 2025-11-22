using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingManager : MonoBehaviour
{
    [SerializeField] GameObject panel;

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
    [Header("Settings_[key] となる")] public List<SettingParams> settings;

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
        BattleManager.inst.Settings_MO_BuffKinds(settings[5].setting.GetValue() == 1);
        BattleManager.inst.Settings_MO_DebuffKinds(settings[6].setting.GetValue() == 1);
    }

    public void SettingsButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText_Old("設定", "設定画面を開く");
        }
        if (Input.GetMouseButtonDown(0))
        {
            panel.SetActive(!panel.activeSelf);
            if (!panel.activeSelf) { PlayerPrefs.Save(); }
        }
    }

    /// <summary>valueはsliderでなく実際の値</summary>
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
        else if (key == settings[5].key)//バフの種類数表示
        {
            setting = settings[5];
            BattleManager.inst.Settings_MO_BuffKinds(value == 1);
        }
        else if (key == settings[6].key)//デバフの種類数表示
        {
            setting = settings[6];
            BattleManager.inst.Settings_MO_DebuffKinds(value == 1);
        }

        else
        {
            infoText.AddErrorText("設定のkeyが間違っています");
            return;
        }

        PlayerPrefs.SetFloat($"Settings_{setting.key}", value);
    }
}
