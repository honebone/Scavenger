using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    [SerializeField] bool checkParticle;
    ParticleSystem particle;
    void Start()
    {
        if (checkParticle) { particle = GetComponent<ParticleSystem>(); }
    }

    void Update()
    {
        if (checkParticle&&particle.isStopped) //パーティクルが終了したか判別
        {
            Destroy(gameObject);//パーティクル用ゲームオブジェクトを削除
        }
    }

    public Vector2 GetOffset() { return offset; }

    public void EndAnimation() { Destroy(gameObject); }
}
