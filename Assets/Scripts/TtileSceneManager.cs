using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class TtileSceneManager : MonoBehaviour
{
    [SerializeField] CanvasGroup textMask;
    [SerializeField] Light2D sunLight;
    bool canStart;

    public void CanStart()
    {
        canStart = true;
        textMask.alpha = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (canStart&&Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartExpedition());
        }
    }

    IEnumerator StartExpedition()
    {
        FindObjectOfType<FadeOutUI>().FadeOut_SetDuration(1);
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            sunLight.intensity -=0.126f;
        }
        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameManager>().GoToExpeditionScene(false);
    }
}
