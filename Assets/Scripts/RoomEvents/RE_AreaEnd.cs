using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_AreaEnd : RoomEvent
{
  
    [SerializeField] REOptionParams endExpedition;
    [SerializeField] List<AreaData> nextAreas;

    protected int choice = 0;
    //GameObject eventManager;
    
    public override void OnEndREInfo()
    {
        List<REOptionParams> list = new List<REOptionParams>();
        foreach(AreaData area in nextAreas)
        {
            REOptionParams option = new REOptionParams();
            option.optionName=string.Format("{0}に移動",area.areaName);
            option.optionInfo = string.Format("次のエリア「{0}」に移動する", area.areaName);
            list.Add(option);   
        }
        list.Add(endExpedition);
        expeditionManager.SetREOptionButtons(list);
    }
    public override void SelectOption(int index)
    {
        choice = index;

        if (choice == nextAreas.Count) { expeditionManager.EndExpediton(); }
        else
        {
            expeditionManager.NextArea(nextAreas[choice]);
        }
        Destroy(this.gameObject);
    }
    //IEnumerator Consequence()
    //{
    //    switch (choice)
    //    {

    //    }
    //}
}
