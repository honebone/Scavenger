using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    [SerializeField]
    Vector2 offset;
    
    public Vector2 GetOffset() { return offset; }

    public void EndAnimation() { Destroy(gameObject); }
}
