using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
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
        soundManager = SoundManager.instance;
        infoText = InfoText.inst;
        expeditionManager = ExpeditionManager.inst;

        foreach (var setting in settings)
        {
            setting.setting.Init(setting.key,SetValue);
        }

        int i = 0;//設定項目の順番を入れ替えやすいようにするため
        soundManager.SetBGMVolume(settings[i].setting.GetValue());
        i++;
        soundManager.SetSEVolume(settings[i].setting.GetValue());
        i++;
        actionQueue.SetResolveSpeed(settings[i].setting.GetValue());
        i++;
        expeditionManager.Setting_ResetPos(settings[i].setting.GetValue() == 1);
        i++;
        BattleManager.inst.Settings_DisplayInfoOnTS(settings[i].setting.GetValue() == 1);
        i++;
        BattleManager.inst.Settings_MO_BuffKinds(settings[i].setting.GetValue() == 1);
        i++;
        BattleManager.inst.Settings_MO_DebuffKinds(settings[i].setting.GetValue() == 1);
        i++;
        BattleManager.inst.Settings_MO_Personalities(settings[i].setting.GetValue() == 1);
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
        int i = 0;
        if (key == settings[i].key)//BGMVolume
        {
            setting = settings[i];
            soundManager.SetBGMVolume(value);
        }
        i++;
        if (key == settings[i].key)//SEVolume
        {
            setting = settings[i];
            soundManager.SetSEVolume(value);
        }
        i++;
        if (key == settings[i].key)//resolveSpeed
        {
            setting = settings[i];
            actionQueue.SetResolveSpeed(value);
        }
        i++;
        if (key == settings[i].key)//resetPos
        {
            setting = settings[i];
            expeditionManager.Setting_ResetPos(value == 1);
        }
        i++;
        if (key == settings[i].key)//displayInfoOnTS
        {
            setting = settings[i];
            BattleManager.inst.Settings_DisplayInfoOnTS(value == 1);
        }
        i++;
        if (key == settings[i].key)//バフの種類数表示
        {
            setting = settings[i];
            BattleManager.inst.Settings_MO_BuffKinds(value == 1);
        }
        i++;
        if (key == settings[i].key)//デバフの種類数表示
        {
            setting = settings[i];
            BattleManager.inst.Settings_MO_DebuffKinds(value == 1);
        }
        i++;
        if (key == settings[i].key)//特性表示
        {
            setting = settings[i];
            BattleManager.inst.Settings_MO_Personalities(value == 1);
        }



        if (!settings.Any(x => x.key == key))
        {
            infoText.AddErrorText("設定のkeyが間違っています");
            return;
        }

        PlayerPrefs.SetFloat($"Settings_{setting.key}", value);
    }
}
