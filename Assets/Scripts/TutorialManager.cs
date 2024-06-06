using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] bool skipTutorial;
    [SerializeField] GameObject panel;
    [SerializeField] Transform guideObjP;

    [SerializeField] TutorialText left;
    [SerializeField] TutorialText right;
    List<TutorialData> unlockedTutorial = new List<TutorialData>();

    TutorialText displayingText;
    TutorialData displayingTutorial;
    int count;

    private void Start()
    {
        skipTutorial = !FindObjectOfType<GameManager>().CheckIfTutorialArea();
    }

    public void StartTutorial(TutorialData tutorial)
    {
        if (!skipTutorial && !unlockedTutorial.Contains(tutorial))
        {
            Time.timeScale = 0;
            panel.SetActive(true);

            displayingTutorial = tutorial;
            unlockedTutorial.Add(tutorial);

            count = 0;
            DisplayTutorial();
        }
    }

    public void DisplayTutorial()
    {
        TutorialData.Tutorial tutorial = displayingTutorial.tutorials[count];
        if (tutorial.left) { displayingText = left; }
        else { displayingText = right; }
        right.ResetText();
        left.ResetText();

        if (guideObjP.childCount != 0) { for (int i = 0; i < guideObjP.childCount; i++) { Destroy(guideObjP.GetChild(i).gameObject); } }
        if (tutorial.guideObj != null) { Instantiate(tutorial.guideObj, guideObjP); }

        displayingText.SetText(tutorial);
    }
    public void Resume()
    {
        count++;
        if (displayingTutorial.tutorials.Count == count) { EndTutorial(); }
        else { DisplayTutorial(); }
    }

    public void EndTutorial()
    {
        right.ResetText();
        left.ResetText();

        if (guideObjP.childCount != 0) { for (int i = 0; i < guideObjP.childCount; i++) { Destroy(guideObjP.GetChild(i).gameObject); } }

        panel.SetActive(false);

        displayingTutorial = null;

        Time.timeScale = 1;
        count = 0;
    }

    public bool CheckUnlocked(TutorialData tutorial) { return skipTutorial || unlockedTutorial.Contains(tutorial); }
}
