using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunction : MonoBehaviour
{
    [SerializeField]
    CharacterData[] characterData;
    [SerializeField]
    int pos;
    [SerializeField]
    List<int> moveValue;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[0], 4);
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[0], 7);

            FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[1], 10);   
            FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[2], 12);
            //FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[3], 13);
            FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[1], 15);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { FindObjectOfType<AreaManager>().GenerateMap(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { FindObjectOfType<ExpeditionManager>().SelectNextRoom(); }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[1], 11);
        }
    }
}
