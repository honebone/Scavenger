using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text infoText;

    publicÅ@void SetText(string name,string info)
    {
        nameText.text = name;
        infoText.text = info;
    }
}
