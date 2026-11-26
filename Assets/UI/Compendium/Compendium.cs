using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Compendium : MonoBehaviour
{
    public static Compendium inst;

    public CanvasGroup canvas;

    [SerializeField] List<CPDElement> elements;
    [SerializeField] CPD_Eq eq;
    [SerializeField] CPD_Per per;
    [SerializeField] TextMeshProUGUI totalUnlocksText;
    [SerializeField] TextMeshProUGUI unlocksText;

    bool active;

    enum Mode { Eq, PA, RaE}
    Mode mode = Mode.Eq;

    CPDElement current => elements[(int)mode];

    private void Awake()
    {
        if (inst == null) inst = this;
    }

    void Start()
    {
        elements.ForEach(x => x.Init());

        RefleshText();
    }

    public void ToggleTab(int index)
    {
        mode = (Mode)index;
        elements.ForEach(e => e.Close());
        current.Open();
        RefleshText();
    }

    public void ToggleUI()
    {
        if (active)  Close();
        else Open();
        SoundManager.instance.PlaySE_Select();
    }
    public void Open()
    {
        canvas.alpha = 1.0f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        active = true;

        RefleshText();
    }
    public void Close()
    {
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        active = false;
    }

    public void UnlockEq(ItemData unlock)
    {
        eq.UnlockEq(unlock);
        RefleshText();
    }

    public void UnlockPer(GameObject unlock)
    {
        per.UnlockPer(unlock);
        RefleshText();
    }

    private void RefleshText()
    {
        Vector2Int unlocks = new Vector2Int();
        elements.ForEach(e => unlocks += e.GetUnlocks());
        totalUnlocksText.text = $"çáåv {unlocks.x}/{unlocks.y}";
        unlocksText.text = current.GetUnlocksStr();
    }
}
