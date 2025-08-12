using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEndLogManager : MonoBehaviour
{
    //public GameObject sideButton;
    //public Transform sideButtonsP;
    public GameObject panel;
    public GameObject prevButton;
    public GameObject nextButton;
    public Transform managerP;

    public GameObject manager_enemyLVLUP;
    public GameObject manager_addPer;
    public GameObject manager_madness;

    ExpeditionManager em;
    List<RoomEndLog> queue = new List<RoomEndLog>();

    int currentIndex;
    int maxIndex;
    bool canMove;

    public static RoomEndLogManager inst;
    private void Awake()
    {
        inst = this;
    }
    void Start()
    {
        em = ExpeditionManager.inst;
    }

    public void Enqueue(GameObject manager)
    {
        var l = Instantiate(manager, managerP);
        l.GetComponent<RoomEndLog>().Init(this);
        queue.Add(l.GetComponent<RoomEndLog>());
        l.SetActive(false);
        //var s = Instantiate
    }
    public void Enqueue_EnemyLVL()
    {
        var l = Instantiate(manager_enemyLVLUP, managerP);
        l.GetComponent<RoomEndLog>().Init(this);
        queue.Add(l.GetComponent<RoomEndLog>());
        l.SetActive(false);
        //var s = Instantiate
    }
    public void Enqueue_AddPer(Character chara,GameObject per)
    {
        var l = Instantiate(manager_addPer, managerP);
        l.GetComponent<RoomEndLog>().Init(this);
        l.GetComponent<REL_AddPer>().Init_AddPer(chara,per);

        queue.Add(l.GetComponent<RoomEndLog>());
        l.SetActive(false);
        //var s = Instantiate
    }

    public void Enqueue_Madness(int m, GameObject PA)
    {
        var l = Instantiate(manager_madness, managerP);
        l.GetComponent<RoomEndLog>().Init(this);
        l.GetComponent<REL_Madness>().Init_Madness(m, PA);

        queue.Add(l.GetComponent<RoomEndLog>());
        l.SetActive(false);
        //var s = Instantiate
    }

    public void StartREL()
    {
        panel.SetActive(true);
        currentIndex = 0;
        maxIndex = -1;
        if (queue.Count > 0) { LogStart(); }
    }

    public void Back()
    {
        if (currentIndex == 0) InfoText.inst.AddErrorText("error!:REL");
        queue[currentIndex].FadeOut();
        currentIndex--;

        if (currentIndex == 0) prevButton.SetActive(false);
        else prevButton.SetActive(true);

        nextButton.SetActive(true);

        LogStart();
    }
    public void Next()
    {
        queue[currentIndex].FadeOut();
        currentIndex++;

        if (currentIndex == queue.Count)//最後のログで次に進んだら終了
        {
            EndREL();
        }
        else
        {
            if (maxIndex >= currentIndex)
            {
                prevButton.SetActive(true);
                nextButton.SetActive(true);
            }
                LogStart();
        }
    }
    public void LogStart()
    {
        if (maxIndex < currentIndex)
        {
            prevButton.SetActive(false);
            nextButton.SetActive(false);

            queue[currentIndex].LogStart(true);
            maxIndex = currentIndex;
        }
        else
        {
            queue[currentIndex].LogStart(false);
        }
    }
    public void LogEnd()
    {
        if (currentIndex > 0) prevButton.SetActive(true);
        nextButton.SetActive(true);
        //ボタンを押せるように 0番目は前に戻れない
    }

    public void EndREL()
    {
        for (int i = 0; i < managerP.childCount; i++) Destroy(managerP.GetChild(i).gameObject);
        panel.SetActive(false);
        currentIndex = 0;
        maxIndex = -1;
        queue.Clear();

        em.SelectNextRoom();
    }

    public bool HasLog() { return queue.Count > 0; }
}
