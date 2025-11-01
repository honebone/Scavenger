using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview
    (
        string assetPath,
        Object[] subAssets,
        int width,
        int height
    )
    {
        var obj = target as ItemData;
        var sprite = obj.sprite;

        if (sprite == null)
        {
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        var preview = AssetPreview.GetAssetPreview(sprite);
        var final = new Texture2D(width, height);
        if (preview == null) { return base.RenderStaticPreview(assetPath, subAssets, width, height); }
        EditorUtility.CopySerialized(preview, final);

        return final;
    }
}

#endif

[CreateAssetMenu( menuName = "ScriptableObjects/ItemData")]


public class ItemData : ScriptableObject
{
    public enum ItemType { material,  equipment,tool }
    public ItemType itemType;
   

    public string itemName;

    public bool specialInfo;
    [TextArea(3, 10),Header("フレーバーテキストとか?")]
    public string info;

    public enum Rarity { common, uncommon, rare, epic, legendary,madness }
    public Rarity rarity;
    public Sprite sprite;

    //material
    public enum MaterialTag { other, valuables, slay, ore, food, plant, processed, junk, sundries, book, holy, weapon,crops }
    [Header("\nMaterial")]
    public int amountPerStack = 1;
    public Vector2Int dropAmountRange;
    public List<MaterialTag> materialTags;
    public int price;//基本となる買値

    ////equipment
    //public enum EquipmentTag { none, weapon, armor }
    //[Header("\nMaterial")]
    //public EquipmentTag equipmentTag;

    [Header("tool,equipment用")]
    public GameObject manager;

    public string GetInfo(bool simple)
    {
        string s = "";

        if (!specialInfo)
        {
            s += string.Format("{0}\n", Definer.rarityName[rarity].ColorStr(rarity.ToColor()));
            bool f = false;

            switch (itemType)
            {
                case ItemData.ItemType.material:
                    s += "<<素材>>\n\n";
                    s += "タグ：";
                    foreach (ItemData.MaterialTag tag in materialTags)
                    {
                        if (f) { s += ", "; }
                        f = true;
                        s += $"[{Definer.materialTagName[tag]}]";
                    }
                    s += "\n";
                    s += "現在素材の使い道は実装されていません...\n".ColorStr(Color.red);
                    s += string.Format("スロットあたりの所持数：{0}\n", amountPerStack.ToString());
                    s += string.Format("価値：{0}G\n", price.ToString());
                    s += string.Format("スロット単価：{0}G\n", (price * amountPerStack).ToString());
                    break;


                case ItemData.ItemType.equipment:
                    s += "<<装備品>>\n";
                    //if (data.equipmentTag != ItemData.EquipmentTag.none)
                    //{
                    //    s += string.Format("[{0}]\n", Definer.equipmentTagName[data.equipmentTag]);
                    //}
                    //s += "\n";
                    s += manager.GetComponent<PassiveAbility>().GetPAInfo(simple);
                    break;


                case ItemData.ItemType.tool:
                    s += "<<道具>>\n\n";
                    s += string.Format("スロットあたりの所持数：{0}\n", amountPerStack.ToString());
                    s += manager.GetComponent<PassiveAbility>().GetPAInfo();
                    break;
            }
            s += info;
        }

        return s;
    }

}
