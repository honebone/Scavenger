using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Shop : RoomEvent
{
    [SerializeField] ShopParams shopParams;
    public override void OnEndREInfo()
    {
        Shop.inst.StartShop(shopParams);
    }
}
