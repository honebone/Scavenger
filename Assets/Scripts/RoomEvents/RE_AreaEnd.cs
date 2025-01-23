using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_AreaEnd : RoomEvent
{
  
    [SerializeField] REOptionParams endExpedition;
    [SerializeField] REOptionParams skipDeploy;
    [SerializeField] List<AreaData> nextAreas;

    protected int choice = 0;
    /// <summary> 0:deploy 1:selectArea </summary>
    int phase;
    //GameObject eventManager;
    List<CharacterData> deployPool = new List<CharacterData>();


    public override void OnEndREInfo()
    {
        if (characterManager.GetExistingCharacters_All().Count < 4)
        {
            phase = 0;
            StartDeploy();
        }
        else
        {
            phase = 1;
            StartSelecrNextArea();
        }
    }
    void StartDeploy()
    {
        deployPool = ExpeditionRef.definer.GetPlayerDataBase();
        deployPool.RemoveList(expeditionManager.deployedChara);
        List<REOptionParams> list = new List<REOptionParams>();
        foreach (CharacterData data in deployPool)
        {
            REOptionParams option = new REOptionParams();
            option.optionName = data.charaName;
            option.optionInfo = data.GetInfo();
            list.Add(option);
        }
        list = new List<REOptionParams>(list.Sample(4));
        list.Add(skipDeploy);
        expeditionManager.SetREOptionButtons(list);
        ExpeditionRef.tutorialManager.Tutorial_Redeploy();
    }
    void StartSelecrNextArea()
    {
        List<REOptionParams> list = new List<REOptionParams>();
        foreach (AreaData area in nextAreas)
        {
            REOptionParams option = new REOptionParams();
            option.optionName = string.Format("{0}に移動", area.areaName);
            option.optionInfo = string.Format("次のエリア「{0}」に移動する\n\n{1}", area.areaName, area.areaInfo.ColorStr(Color.gray));
            list.Add(option);
        }
        list.Add(endExpedition);
        expeditionManager.SetREOptionButtons(list);
    }
    public override void OnRClick(int index)
    {
        if (phase == 0 && index < deployPool.Count)
        {
            Character.CharacterStatus status = new Character.CharacterStatus();
            status.Init(deployPool[index]);
            AbilityButtonPanel.instance.SetAbilityButtons_Deploy(status.abilitiesStatus);
        }
    }
    public override void SelectOption(int index)
    {
        choice = index;

        if (phase == 0)
        {
            if (choice < deployPool.Count)
            {
                List<int> playerPos = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                characterManager.SpawnPlayer(deployPool[index], characterManager.GetEmptyPos(playerPos)[0], 1);
                expeditionManager.deployedChara.Add(deployPool[index]);
            }

            phase = 1;
            StartSelecrNextArea();
        }
        else if(phase == 1)
        {
            if (choice == nextAreas.Count) { expeditionManager.EndExpediton(); }
            else
            {
                expeditionManager.NextArea(nextAreas[choice]);
            }
            Destroy(this.gameObject);
        }        
    }
}
