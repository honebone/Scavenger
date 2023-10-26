using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunction : MonoBehaviour
{

    [SerializeField]
    CharacterData[] characterData;

    [SerializeField]
    ItemData[] itemData;
    [SerializeField]
    int[] amount;
    Definer.Item[] items;

    private void Start()
    {
        items=new Definer.Item[itemData.Length];
        for(int i = 0; i < itemData.Length; i++)
        {
            items[i].Init(itemData[i]);
            items[i].amount = amount[i];
        }
        //for(int i = 0; i < 18; i++)
        //{
        //    print(string.Format("{0}:{1}",i,i.GetCurrentColumn()));
        //}
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[0], 6);
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[0], 7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[1], 10,true);
            FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[2], 12,true);
            FindObjectOfType<CharactersManager>().SpawnEnemy(characterData[3], 11, true);
            FindObjectOfType<BattleManager>().BattleStart();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { FindObjectOfType<AreaManager>().GenerateMap(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { FindObjectOfType<ExpeditionManager>().SelectNextRoom(); }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach(Definer.Item item in items)
            {
                FindObjectOfType<Inventory>().AddItem(item,item.amount);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            foreach (Definer.Item item in items)
            {
                FindObjectOfType<LootPanel>().AddItem(item, item.amount);
            }

        }
    }
}
