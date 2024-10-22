using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    bool skipTutorial;
    [SerializeField] GameObject panel;
    [SerializeField] Transform guideObjP;

    [SerializeField] TutorialText left;
    [SerializeField] TutorialText right;
    List<TutorialData> unlockedTutorial = new List<TutorialData>();
    [SerializeField] TutorialData T_deathsDoor;

    TutorialText displayingText;
    TutorialData displayingTutorial;
    List<TutorialData> tutorialQueue = new List<TutorialData>();
    GameManager gameManager;

    int count;
    bool inTutorial;

    private void Start()
    {
        //skipTutorial = !FindObjectOfType<GameManager>().CheckDoesTutorial();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetTutorial(TutorialData tutorial)
    {
        if (gameManager.DoTutorial() && !unlockedTutorial.Contains(tutorial))
        {
            unlockedTutorial.Add(tutorial);
            tutorialQueue.Add(tutorial);

            if (!inTutorial)
            {
                StartTutorial();   
            }
        }
    }

    public void StartTutorial()
    {
        inTutorial = true;
        Time.timeScale = 0;
        panel.SetActive(true);

        displayingTutorial = tutorialQueue[0];
        tutorialQueue.RemoveAt(0);

        count = 0;
        DisplayTutorial();
    }

    public void Tutorial_dethsDoor() { SetTutorial(T_deathsDoor); }

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

        if (tutorialQueue.Count > 0) { StartTutorial(); }
        else
        {
            inTutorial = false;
            panel.SetActive(false);

            displayingTutorial = null;

            Time.timeScale = 1;
            count = 0;
        }
    }

    public bool CheckUnlocked(TutorialData tutorial) { return skipTutorial || unlockedTutorial.Contains(tutorial); }
}
