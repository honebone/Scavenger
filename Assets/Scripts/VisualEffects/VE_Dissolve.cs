using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VE_Dissolve : MonoBehaviour
{
    [SerializeField] float dissolveDelay = 0;
    [SerializeField] float dissolveTime = 1f;
    SpriteRenderer sr;
    Material mat;
    void Start()
    {
        sr=GetComponent<SpriteRenderer>();
        mat = sr.material;

        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        if(dissolveDelay > 0)yield return new WaitForSeconds(dissolveDelay);
        float timer = 0f;
        while(timer<dissolveTime)
        {
            timer += Time.deltaTime;
            mat.SetFloat("_Fade", 1-(timer / dissolveTime));

            yield return null;
        }
    }
}
