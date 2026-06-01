using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TtileSceneManager : MonoBehaviour
{
    [SerializeField] CanvasGroup textMask;
    [SerializeField] Light2D sunLight;
    [SerializeField] AudioClip BGM;
    [SerializeField] GameObject settingsPanel;

    [SerializeField] UIPanel gameStartCanvas;
    [SerializeField] UIPanel gameModeCanvas;

    [SerializeField] TextMeshProUGUI difficultyInfo;
    [SerializeField] List<GameModeSelectButton> gameModeSelectButtons;
    //[SerializeField] Toggle skipTutorial;

    public static bool skipTutorial_bool;

    [Header("Settings_[key] Ç∆Ç»ÇÈ")] public List<SettingManager.SettingParams> settings;

    GameManager gameManager;
    SoundManager soundManager;

    bool canStart;

    public void CanStart()
    {
        canStart = true;
        gameStartCanvas.SetPanelActive(true);
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = SoundManager.instance;
        //skipTutorial_bool = PlayerPrefs.GetInt("Setting_skipTutorial", 0) == 1;
        //skipTutorial.isOn = skipTutorial_bool;

        foreach (var setting in settings)
        {
            setting.setting.Init(setting.key, SetValue);
        }

        soundManager.SetBGMVolume(settings[0].setting.GetValue());
        soundManager.SetSEVolume(settings[1].setting.GetValue());
        skipTutorial_bool = settings[2].setting.GetValue() == 1;
        Debug.Log($"skipTutorial is {skipTutorial_bool}");

        soundManager.SetBGM_Normal(BGM);
    }

    public void SetValue(float value, string key)
    {
        SettingManager.SettingParams setting = new SettingManager.SettingParams();
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
        else if (key == settings[2].key)//skipTutorial
        {
            setting = settings[2];
            skipTutorial_bool = value == 1;
            Debug.Log($"skipTutorial is {skipTutorial_bool}");
        }


        else
        {
            Debug.Log("ê›íËÇÃkeyÇ™ä‘à·Ç¡ÇƒÇ¢Ç‹Ç∑");
            return;
        }

        PlayerPrefs.SetFloat($"Settings_{setting.key}", value);
    }

    IEnumerator StartExpedition()
    {
        FindObjectOfType<FadeOutUI>().FadeOut_SetDuration(1);
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            sunLight.intensity -=0.126f;
        }
        yield return new WaitForSeconds(1f);
        gameManager.GoToExpeditionScene(!skipTutorial_bool);
    }

    //public void ToggleTutorial() { PlayerPrefs.SetInt("Setting_skipTutorial", (skipTutorial.isOn) ? 1 : 0); }
    public void StartGame()
    {
        if (canStart)
        {
            settingsPanel.SetActive(false);
            soundManager.StopBGMs();
            PlayerPrefs.Save();
            canStart = false;
            StartCoroutine(StartExpedition());
        }
    }

    public void ToDifficultySelect()
    {
        if (canStart)
        {
            gameStartCanvas.SetPanelActive(false);
            gameModeCanvas.SetPanelActive(true);

            SetDifficulty(0);
        }
    }

    public void SetDifficulty(int value)
    {
        SoundManager.instance.PlaySE_Select();
        difficultyInfo.text = gameManager.SetDifficulty((Difficulty)value).info;

        gameModeSelectButtons.ForEach(b=>b.Deactivate());
        gameModeSelectButtons[value].Activate();
    }
}
