using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMessage : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI titleText;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetTrigger("set");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
