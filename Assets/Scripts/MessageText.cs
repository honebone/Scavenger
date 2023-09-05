using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageText : MonoBehaviour
{
    [SerializeField]
    Text messageText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetText(string text)
    {
        messageText.text = text;
    }
    public void ResetText() { messageText.text = ""; }
}
