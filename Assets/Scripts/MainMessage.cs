using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMessage : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI infoText;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetMessage(string message)
    {
        titleText.text = message;
        anim.SetTrigger("set");
    }

    public void ResetMessage()
    {
        anim.SetTrigger("reset");
    }
    public void SetInfo(string info)
    {
        infoText.text = info;
        anim.SetTrigger("info");
    }
    public void ClearText()
    {
        titleText.text = "";
        infoText.text = "";
    }
}
