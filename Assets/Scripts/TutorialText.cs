using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialText : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI tutorialText;
   
    public void SetText(TutorialData.Tutorial tutorial)
    {
        panel.SetActive(true);
        titleText.text = tutorial.title;
        tutorialText.text = tutorial.tutorialText;
    }
    
    public void ResetText()
    {
        titleText.text = "";
        tutorialText.text = "";
        panel.SetActive(false);

    }
}
