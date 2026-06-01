using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaE_MemoryDealer : RE_RandomEvents
{
    [SerializeField] Vector2Int price_good;
    [SerializeField] Vector2Int price_awoken;

    [SerializeField] int lineup_good;

    List<GameObject> perList=new List<GameObject>();
    List<int> priceList=new List<int>();

    GameObject per;
    public override void StartRandomEvent()
    {
        result = SelectAction;

        List<REOptionParams > options = new List<REOptionParams>();
        options.Add(GenShopItem(expeditionManager.GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType.awoken)[0], price_awoken.Range()));

        expeditionManager.GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType.good, lineup_good).ForEach(p =>
        {
            options.Add(GenShopItem(p, price_good.Range()));
        });

        options.Add(option_exit);

        expeditionManager.SetREOptionButtons(options);
    }

    REOptionParams GenShopItem(GameObject obj, int price)
    {
        PA_Personality PA = obj.GetComponent<PA_Personality>();
        REOptionParams option = new REOptionParams();
        option.optionName = $"{"coin".ToSpr_withName()}{price}ÅF{PA.GetPAName()}";
        option.optionInfo = GenPerOptionInfo(PA, $"{"coin".ToSpr_withName()}{price}Çè¡îÔÇµÇƒÅAÉLÉÉÉâ1ëÃÇ™");
        option.available = CheckCoin(price);

        perList.Add(obj);
        priceList.Add(price);

        return option;
    }

    void SelectAction(int index)
    {
        if (index < perList.Count)
        {
            per=perList[index];
            inventory.RemoveCoin(priceList[index]);

            result = SelectPlayer;
            expeditionManager.SetREOptionButtons(GenPlayerSelects());
        }
        else
        {
            EndRoomEvent();
        }
    }

    void SelectPlayer(int index)
    {
        expeditionManager.SetPersonality(players[index], per);

        EndRoomEvent();
    }
}
