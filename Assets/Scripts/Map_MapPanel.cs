using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_MapPanel : MonoBehaviour
{
    [SerializeField]
    GameObject mapPanel;

    [SerializeField] GameObject content;
    [SerializeField] ScrollRect mapScroll;
    [SerializeField] GameObject layerPanel;

    [SerializeField] TutorialData tutorial_map;
    [SerializeField] TutorialData tutorial_expedition;

    List<Map_LayerPanel> layers;

    ExpeditionManager expeditionManager;
    InfoText infoText;
    TutorialManager tutorialManager;
    MouseOverUI mouseOver;
    private void Start()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        infoText = FindObjectOfType<InfoText>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        mouseOver = FindObjectOfType<MouseOverUI>();
    }

    public void MapButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText_Old("マップ","マップを開く");
        }
        if (Input.GetMouseButtonDown(0))
        {
            ToggleMap();
        }
    }
    public void ToggleMap()
    {
        if (!mapPanel.activeSelf) { OpenMap(); }
        else { CloseMap(); }
    }
    public void OpenMap()
    {
        if (!mapPanel.activeSelf) {
            SoundManager.instance.PlaySE_Select();
            mapPanel.SetActive(true);
            if (tutorialManager.CheckUnlocked(tutorial_expedition))
            {
                tutorialManager.SetTutorial(tutorial_map);
            }
        }
    }
    public void CloseMap()
    {
        if (mapPanel.activeSelf)
        {
            SoundManager.instance.PlaySE_Select();
            mapPanel.SetActive(false);
        }
    }

    public void ResetMap()
    {
        for (int i = 0; i < content.transform.childCount; i++) { Destroy(content.transform.GetChild(i).gameObject); }
        layers = new List<Map_LayerPanel>();
    }

    public void SetLayerPanel(ExpeditionManager.Room[] l, int lc)
    {
        var lp = Instantiate(layerPanel, content.transform);
        lp.GetComponent<Map_LayerPanel>().Init(l, lc, expeditionManager, infoText, mapScroll, mouseOver);
        layers.Add(lp.GetComponent<Map_LayerPanel>());
    }
    public void EndGenerateMap()
    {
        expeditionManager.SetLayers(layers);
        mapScroll.horizontalNormalizedPosition = 0;
    }
}
