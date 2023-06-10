using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_MapPanel : MonoBehaviour
{
    [SerializeField]
    GameObject mapPanel;
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
}
