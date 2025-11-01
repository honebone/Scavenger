using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Compendium : MonoBehaviour
{
    public CanvasGroup canvas;
    public Transform eqButtonsP;
    public GameObject eqButton;
    public ScrollRect scroll;

    List<ItemData> eqs = new List<ItemData>();
    List<CPD_EqButton> eqButtons = new List<CPD_EqButton>();
    bool active;

    public void ToggleUI()
    {
        if (active)  Close();
        else Open();
    }
    public void Open()
    {
        canvas.alpha = 1.0f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        active = true;
    }
    public void Close()
    {
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        var list = new List<ItemData>(Definer.inst.GetAllEquipments());
        eqs = list.OrderBy(x => x.rarity).ToList();
        eqs.ForEach(x =>
        {
            var b = Instantiate(eqButton, eqButtonsP);
            eqButtons.Add(b.GetComponent<CPD_EqButton>().Init(x, scroll,50.Dice()));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
