using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunction : MonoBehaviour
{
    [SerializeField]
    CharacterData characterData;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { FindObjectOfType<CharactersManager>().SpawnPlayer(characterData, 7); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { FindObjectOfType<AreaManager>().GenerateMap(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { FindObjectOfType<ExpeditionManager>().SelectNextRoom(); }
    }
}
