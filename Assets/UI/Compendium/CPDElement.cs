using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CPDElement : MonoBehaviour
{
    [SerializeField]public UIPanel panel;
    [SerializeField] protected Transform buttonsP;
    [SerializeField] protected GameObject buttonPrefab;
    [SerializeField] protected ScrollRect scroll;

    protected List<int> unlockStates = new List<int>();
    public virtual void Init() { }

    public void Open()
    {
        panel.SetPanelActive(true);
    }

    public void Close()
    {
        panel.SetPanelActive(false);
    }

    public virtual string GetUnlocksStr() { return $"{unlockStates.Count(x => x != 0)}/{unlockStates.Count()}"; }
    public  virtual Vector2Int GetUnlocks() { return new Vector2Int(unlockStates.Count(x => x != 0), unlockStates.Count()); }
}
