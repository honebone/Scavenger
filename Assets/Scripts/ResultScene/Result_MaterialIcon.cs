using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_MaterialIcon : MonoBehaviour
{
    [SerializeField]
    Image materialImage;
    [SerializeField]
    Image frame;
    [SerializeField]
    Text amountText;

    Definer.Item item;
    bool revealed;

    public void Init(Definer.Item i)
    {
        item = i;
    }

    public void Reveal()
    {
        materialImage.enabled = true;
        materialImage.sprite = item.data.sprite;
        frame.enabled = true;
        frame.color = item.data.rarity.ToColor();
        amountText.text = item.amount.ToString();

        revealed = true;
    }

    public bool GetRevealed() { return revealed; }
}
