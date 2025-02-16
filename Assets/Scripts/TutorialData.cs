using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/TutorialData")]
public class TutorialData : ScriptableObject
{
    public string key;
    [System.Serializable]
    public class Tutorial
    {
        public string title;
        [TextArea(3, 10)] public string tutorialText;
        public bool left;
        public GameObject guideObj;
    }

    public List<Tutorial> tutorials;
}
