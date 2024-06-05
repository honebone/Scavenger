using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Transform guideObjP;

    [SerializeField] TutorialText left;
    [SerializeField] TutorialText right;
    List<TutorialData> unlockedTutorial;

    TutorialText displayingText;
    TutorialData displayingTutorial;
    int count;
  
    public void StartTutorial(TutorialData tutorial)
    {
        if (!unlockedTutorial.Contains(tutorial))
        {
            displayingTutorial = tutorial;
            unlockedTutorial.Add(tutorial);

            count = 0;

        }
    }

    public void DisplayTutorial()
    {
        if (displayingTutorial.tutorials[count].left) { displayingText = left; }
        else { displayingText = right; }


    }
}
