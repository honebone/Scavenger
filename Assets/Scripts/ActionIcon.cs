using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour
{
    [SerializeField]
    Image image;
    public void Init(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void DestroyIcon() { Destroy(gameObject); }
}
