using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager inst;

    bool skipTutorial;
    [SerializeField] GameObject panel;
    [SerializeField] Transform guideObjP;

    [SerializeField] TutorialText left;
    [SerializeField] TutorialText right;
    List<TutorialData> unlockedTutorial = new List<TutorialData>();

    public List<TutorialData> tutorials;
    public List<TutorialData> tips;
    [Header("これらがアンロックされると、最低限のチュートリアルが完了したとみなされる")]public List<TutorialData> essentialTutorials;
    [SerializeField] TutorialData T_deathsDoor;
    [SerializeField] TutorialData T_redeploy;

    TutorialText displayingText;
    TutorialData displayingTutorial;
    List<TutorialData> tutorialQueue = new List<TutorialData>();
    GameManager gameManager;

    int count;
    bool inTutorial;

    private void Awake()
    {
        if(inst == null)inst = this;
    }

    private void Start()
    {
        //skipTutorial = !FindObjectOfType<GameManager>().CheckDoesTutorial();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetTutorial(TutorialData tutorial, bool phase2 = false)
    {
        if (phase2 && !CompleteEssentials())
        {
            return;
        }
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
    public void SetTutorial(string key,bool phase2 = false)
    {
        foreach (TutorialData tutorial in tutorials)
        {
            if (tutorial.key == key)
            {
                SetTutorial(tutorial, phase2);
                return;
            }
        }
        InfoText.inst.AddErrorText("tutorialが発見できませんでした");
    }

    public void SetTips()
    {
        List<TutorialData> list = tips.Where(x => !unlockedTutorial.Contains(x)).ToList();
        if (list.Count > 0) SetTutorial(list.Choice());
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
    public void Tutorial_Redeploy() { SetTutorial(T_redeploy); }

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
    public bool CompleteEssentials() { return essentialTutorials.Where(x => !CheckUnlocked(x)).Count() == 0; }
}
