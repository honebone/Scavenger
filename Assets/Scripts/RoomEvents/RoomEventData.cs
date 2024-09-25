using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RE_", menuName = "ScriptableObjects/RoomEventData")]

public class RoomEventData : ScriptableObject
{
    public string eventName;
    [TextArea(3, 10)]
    public string eventInfo;
    public bool randomEvent;
    public bool debug;
    public List<GameObject> roomEventManager;
    public Sprite eventIcon;
}
