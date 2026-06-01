using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    
    public void SetPanelActive(bool active)
    {
        canvas.alpha = active ? 1 : 0;
        canvas.blocksRaycasts = active;
        canvas.interactable = active;
    }
}
