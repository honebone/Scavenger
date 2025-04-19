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

    ExpeditionManager em;
    List<RoomEndLog> queue = new List<RoomEndLog>();

    int currentIndex;
    int maxIndex;
    bool canMove;
    // Start is called before the first frame update
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

    public void StartREL()
    {
        panel.SetActive(true);
        currentIndex = 0;
        maxIndex = -1;
        if (queue.Count > 0) { LogStart(); }
    }

    public void Next()
    {
        if(currentIndex == queue.Count)//最後のログで次に進んだら終了
        {

        }
    }
    public void LogStart()
    {
        if (maxIndex < currentIndex)
        {
            queue[currentIndex].LogStart(true);
            maxIndex = currentIndex;
        }
        else { queue[currentIndex].LogStart(false); }
    }
    public void LogEnd()
    {
        //ボタンを押せるように 0番目は前に戻れない
    }

    public bool HasLog() { return queue.Count > 0; }
}
