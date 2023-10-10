using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    //scriptable object‚ÌƒAƒCƒRƒ“‚Ì•Ï‚¦•û‚ª‚í‚©‚Á‚½‚ç”pŽ~‚·‚é—\’è
    [SerializeField]
    ItemData itemData;

    public ItemData GetItemData() { return itemData; }
}
