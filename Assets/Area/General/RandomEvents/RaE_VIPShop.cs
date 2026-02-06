using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_VIPShop : RE_RandomEvents
{
    [SerializeField] ItemData memberPlate;
    [SerializeField] int price;
    [SerializeField] REOptionParams option_pay;
    [SerializeField] REOptionParams option_member;
    [SerializeField] ShopParams shopParams;
    public override void StartRandomEvent()
    {
        REOptionParams pay=new REOptionParams(option_pay);
        REOptionParams member=new REOptionParams(option_member);

        pay.available=Inventory.inst.CheckCoin(price);
        member.available = Inventory.inst.CheckEq(memberPlate, true);

        expeditionManager.SetREOptionButtons(new List<REOptionParams>() { pay, member, option_exit });
    }
    public override void SelectOption(int index)
    {
        if (index == 0)
        {
            Inventory.inst.RemoveCoin(price);
            Shop.inst.StartShop(shopParams);
        }
        else if (index == 1) 
        {
            Shop.inst.StartShop(shopParams);
        }
        else
        {
            EndRoomEvent();
        }
    }
}
