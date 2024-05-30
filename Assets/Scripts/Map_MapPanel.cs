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
    List<Map_LayerPanel> layers;

    ExpeditionManager expeditionManager;
    InfoText infoText;
    private void Start()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        infoText = FindObjectOfType<InfoText>();
    }
    public void ToggleMap()
    {
        mapPanel.SetActive(!mapPanel.activeSelf);
    }
    public void OpenMap()
    {
        if (!mapPanel.activeSelf) { mapPanel.SetActive(true); }
    }
    public void CloseMap()
    {
        if (mapPanel.activeSelf) { mapPanel.SetActive(false); }
    }

    public void ResetMap()
    {
        for (int i = 0; i < content.transform.childCount; i++) { Destroy(content.transform.GetChild(i).gameObject); }
        layers = new List<Map_LayerPanel>();
    }

    public void SetLayerPanel(ExpeditionManager.Room[] l, int lc)
    {
        var lp = Instantiate(layerPanel, content.transform);
        lp.GetComponent<Map_LayerPanel>().Init(l, lc, expeditionManager, infoText, mapScroll);
        layers.Add(lp.GetComponent<Map_LayerPanel>());
    }
    public void EndGenerateMap()
    {
        expeditionManager.SetLayers(layers);
    }
}
