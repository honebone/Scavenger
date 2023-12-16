using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu( menuName = "ScriptableObjects/ItemData")]


public class ItemData : ScriptableObject
{
    public enum ItemType { material,  equipment,tool }
    public ItemType itemType;
   

    public string itemName;
    [TextArea(3, 10)]
    public string info;
    public int amountPerStack = 1;
    public enum Rarity { common, uncommon, rare, epic, legendary }
    public Rarity rarity;
    public Sprite sprite;

    //material
    public enum MaterialTag { other, valuables, slay, ore, food, plant, processed }
    public MaterialTag[] materialTags=new MaterialTag[1];
    public int price;//Šî–{‚Æ‚È‚é”ƒ’l

    //equipment
    public enum EquipmentTag { none, weapon, armor }
    public EquipmentTag equipmentTag;

    [Header("tool,equipment—p")]
    public GameObject manager;
   
}
