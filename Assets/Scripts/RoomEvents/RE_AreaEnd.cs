using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_AreaEnd : RoomEvent
{
  
    [SerializeField]
    List<RoomEvent.REOptionParams> options;

    protected int choice = 0;
    //GameObject eventManager;
    
    public override void OnEndREInfo()
    {
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        choice = index;

        expeditionManager.EndExpediton();//test
    }
    //IEnumerator Consequence()
    //{
    //    switch (choice)
    //    {

    //    }
    //}
}
