using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Object : MonoBehaviour
{
    [SerializeField]
    Transform characterManagerParent;
   public void Init(Character.CharacterStatus characterStatus,GameObject charaManager)
    {
        var c = Instantiate(charaManager, characterManagerParent);
        c.GetComponent<Character>().Init(characterStatus);
    }
}
