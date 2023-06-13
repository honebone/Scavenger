using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionInfoPanel : MonoBehaviour
{
    [SerializeField]
    Text infoText;
    [SerializeField]
    Image frame;
    public void Init(string actionName,string actionInfo)
    {

        infoText.text += string.Format("<{0}>\n{1}", actionName, actionInfo);
    }
}
