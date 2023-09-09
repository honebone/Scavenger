using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu( menuName = "ScriptableObjects/ItemData")]


public class ItemData : ScriptableObject
{
    public enum ItemType { material,  temporary }
    public ItemType itemType;
    public enum MaterialTag { other, valuables, slay, ore, food, plant, processed }
    public MaterialTag[] materialTags=new MaterialTag[1];
    public enum TemporaryTag { other, tool, equipment }
    public TemporaryTag temporaryTag;

    public string itemName;
    [TextArea(3, 10)]
    public string info;
    public int amountPerStack;
    public enum Rarity { common, uncommon, rare, epic, legendary }
    public Rarity rarity;
    public Sprite sprite;
    public int price;//Šî–{‚Æ‚È‚é”ƒ’l

    [Header("tool,equipment—p")]
    public GameObject manager;
   
}
