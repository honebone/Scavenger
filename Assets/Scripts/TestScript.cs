using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    float speed;
    bool[] spinning =new bool[3];//各リールが回っているか
    [SerializeField]
    Transform[] ReelsTF;

    bool f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < 3; i++)
        {
            if (spinning[i])
            {
                ReelsTF[i].Translate(Vector3.down * speed);
            }
            if (ReelsTF[i].position.y <= -2200)
            {
                Vector3 pos = ReelsTF[i].position;
                pos.y = 2400;
                ReelsTF[i].position = pos;
            }
        }
        if (f)
        {
            f = false;
            
        }
    }
    public void SpinReel()
    {
        f = true;

    }
    /// <summary>ボタンを押したときの処理</summary>
    /// <param name="order">押されたボタンの識別</param>
    public void StopReel(int order)
    {
        spinning[order] = false;
    }
}
