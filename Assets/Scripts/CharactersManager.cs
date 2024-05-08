using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharactersManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int[] charactersWorldPos_Size1;

    [SerializeField]
    Character_TargetButton[] targetButtons_size1;
    [SerializeField]
    PositionManager[] positionManagers;


    [SerializeField]
    GameObject characterObject;

    [SerializeField]//test
    List<Character> generatedCharacters;
    List<Character> existingCharacters;

    InfoText infoText;
    Utility util;
    public void AddCharacter(Character character)
    {
        generatedCharacters.Add(character);
        existingCharacters.Add(character);
        SortExistingCharacters();
        //if (existingCharacters.Count == 0) { existingCharacters.Add(character); }
        //else
        //{
        //    for (int i = 0; i < existingCharacters.Count; i++)
        //    {
        //        if (existingCharacters[i].GetCharacterStatus().positon > character.GetCharacterStatus().positon)
        //        {
        //            existingCharacters.Insert(i, character);
        //            break;
        //        }
        //    }
        //}         
    }
    public void SortExistingCharacters()
    {
        existingCharacters.Sort((a, b) => a.GetCharacterStatus().position - b.GetCharacterStatus().position);
    }
    /// <summary>生存している全てのキャラを返します </summary>
    public List<Character> GetExistingCharacters_All() { return existingCharacters; }
    public List<Character> GetExistingCharacters(List<int> positions,bool includeEmpty)
    {
        List<Character> characters = new List<Character>();
        foreach (int pos in positions) { if (CheckCharaExist(pos)||includeEmpty) { characters.Add(GetCharacterWithPos(pos)); } }
        return characters;
    }
    public List<int> GetExistingCharactersPos(List<int> targetPool)
    {
        List<int> ints = new List<int>();
        List<Character> characterList = new List<Character>();
        foreach(int target in targetPool)
        {
            if (CheckCharaExist(target) && !characterList.Contains(GetCharacterWithPos(target)))
            {
                ints.Add(target);
                characterList.Add(GetCharacterWithPos(target));
            }
        }
        return ints;
    }
    public List<Character.CharacterStatus> GetExistingCharactersStatus()
    {
        List<Character.CharacterStatus> charactersStatus = new List<Character.CharacterStatus>();
        foreach (Character existingCharacter in existingCharacters) { charactersStatus.Add(existingCharacter.GetCharacterStatus()); }
        return charactersStatus;
    }


    /// <summary>移動処理に使用　進行方向にいるキャラを取得</summary>
    /// <param name="ranges">0:right 1:upper 2:lower 3:left</param>
    /// <returns></returns>
    public List<Character> GetTravelingDirCharas(int pos,int dir,int range)
    {
        List<Character> c = new List<Character>();
        for(int i = 1; i <= range; i++)
        {
            if(CheckCharaExist(util.GetMoveToPos(pos, dir, i))&&!c.Contains(GetCharacterWithPos(util.GetMoveToPos(pos, dir, i))))
            {
                c.Add(GetCharacterWithPos(util.GetMoveToPos(pos, dir, i)));
            }
        }

        return c;
    }

   

    public void RemoveExistingCharacter(Character chara)
    {
        if (existingCharacters.Contains(chara)) { existingCharacters.Remove(chara); }
        else { infoText.AddErrorText("今死亡したキャラは、そもそも存在していません"); }
    }

    public Character_TargetButton GetTargetButton(int pos)  { return targetButtons_size1[pos];}
    public PositionManager GetPositionManager(int pos) { return positionManagers[pos]; }
    public PositionManager[] GetPositionManagers() { return positionManagers; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="targetEmpty">空きスペースを対象とするか</param>
    /// <param name="size">空きスペースを対象とする場合、そのサイズ</param>
    /// <param name="targetGroup"></param>
    public void SetTargetIcon(int pos,bool targetEmpty,List<int> targetGroup)
    {
        if (targetEmpty)
        {
            GetTargetButton(pos).SetTargetIcon(targetGroup);
        }
        else
        {
            if (CheckCharaExist(pos))
            {
                GetTargetButton( GetCharacterWithPos(pos).GetCharacterStatus().position).SetTargetIcon(targetGroup);
            }
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="checkPos"></param>
    /// <returns></returns>
    public bool CheckCharaExist(int checkPos)
    {
        foreach (Character.CharacterStatus characterStatus in GetExistingCharactersStatus())
        {
            if (characterStatus.position == checkPos) { return true; }
        }
        return false;
    }
    public bool CheckCharaExistAtLeastOne(List<int> checkRange)
    {
        foreach (int checkPos in checkRange)
        {
            if (CheckCharaExist(checkPos)) { return true; }
        }
        return false;
    }
    [System.Serializable]
    public struct SearchCharaCondition
    {
        [Header("<ポジション(int)を検索する>")]
        public bool searchAsPos;
        public bool onlyEmpty;
        [Header("<検索範囲の指定>")]
        public bool player;
        public bool enemy;
        public bool onlyPlayable;
        [Space]
        public bool front;
        public bool mid;
        public bool back;

        [Header("\n\n\n<検索条件の指定>")]
        public List<CharacterData.CharacterTag> characterTags;
        public List<GameObject> StE;
        public List<GameObject> PE;
        public float HPPercent;
        public bool HP_lessThan;
        public bool HP_excludeEqual;

    }
    public List<Character> SearchCharaWithCondition(SearchCharaCondition condition)
    {
        if (condition.searchAsPos)
        {
            infoText.AddErrorText("ポジションを検索するためにキャラクター検索を行っています!!");
            return null;
        }
        List<Character> list = new List<Character>();
        foreach(Character character in existingCharacters)
        {
            Character.CharacterStatus status = character.GetCharacterStatus();
            if (!condition.player && status.position < 9) { continue; }
            if (!condition.enemy && status.position >= 9) { continue; }
            if (condition.onlyPlayable && !status.playable) { continue; }
            if(!condition.front && status.position.GetColumn()==0) { continue; }
            if (!condition.mid && status.position.GetColumn() == 1) { continue; }
            if (!condition.back && status.position.GetColumn() == 2) { continue; }
            bool matched = condition.characterTags.Count == 0;
            foreach (CharacterData.CharacterTag tag in condition.characterTags)
            {
                if (status.characterTags.Contains(tag))
                {
                    matched = true;
                    break;
                }
            }
            if (!matched) { continue; }
            matched = condition.StE.Count == 0;

            foreach (GameObject s in condition.StE)
            {
                if (character.CheckHasStE(s))
                {
                    matched = true;
                    break;
                }
            }
            if (!matched) { continue; }
            matched = condition.PE.Count == 0; 

            foreach (GameObject s in condition.PE)
            {
                if (character.CheckHasPE(s))
                {
                    matched = true;
                    break;
                }
            }
            if (!matched) { continue; }

            float HPPercent = status.HP.GetPercent(status.maxHP);
            if (condition.HP_lessThan)
            {
                if (condition.HP_excludeEqual && HPPercent >= condition.HPPercent) { continue; }//HP% < 条件値 のみ通す
                if(!condition.HP_excludeEqual && HPPercent > condition.HPPercent) { continue; }//HP% <= 条件値 のみ通す
            }
            else
            {
                if(condition.HP_excludeEqual && HPPercent <= condition.HPPercent) { continue; }//HP% > 条件値 のみ通す
                if (!condition.HP_excludeEqual && HPPercent < condition.HPPercent) { continue; }//HP% >= 条件値 のみ通す
            }

            list.Add(character);

        }

        return list;
    }
    public bool CheckIfMatchCondition(Character character, SearchCharaCondition condition)
    {
        return SearchCharaWithCondition(condition).Contains(character);
    }

    public List<int> SearchPosWithCondition(SearchCharaCondition condition)
    {
        if (!condition.searchAsPos)
        {
            infoText.AddErrorText("キャラクターを検索するためにポジション検索を行っています!!");
            return null;
        }
        List<int> list = new List<int>();
        for (int i =0;i<18;i++)
        {
            if (!condition.player && i < 9) { continue; }
            if (!condition.enemy && i >= 9) { continue; }
            if (!condition.front && i.GetColumn() == 0) { continue; }
            if (!condition.mid && i.GetColumn() == 1) { continue; }
            if (!condition.back && i.GetColumn() == 2) { continue; }
            if (condition.onlyEmpty && CheckCharaExist(i)) { continue; }
            bool matched = condition.PE.Count == 0;

            foreach (GameObject s in condition.PE)
            {
                if (GetPositionManager(i).CheckHasPE(s))
                {
                    matched = true;
                    break;
                }
            }
            if (!matched) { continue; }
            list.Add(i);

        }
        return list;
    }

    public Character GetCharacterWithPos(int pos)
    {
        foreach (Character character in GetExistingCharacters_All())
        {
            Character.CharacterStatus characterStatus = character.GetCharacterStatus();
            if (characterStatus.position == pos) { return character; }
        }
        infoText.AddDebugText(string.Format("error:ポジション{0}にキャラクターは存在していません", pos));
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveValue">0:right 1:upper 2:lower 3:left</param>
    /// <returns></returns>
    public List<int> GetMoveTargets(int pos,List<int> moveValue)
    {
        int minPos = 0;
        int maxPos = 8;
        List<int> targets = new List<int>();

        if (pos >= 9)
        {
            minPos = 9;
            maxPos = 17;
        }
        if (moveValue[0] > 0)
        {
            for (int i = 1; i <= moveValue[0]; i++)
            {
                if (pos + (3 * i) <= maxPos) { targets.Add(pos + (i * 3)); }
                else { break; }
            }
        }
        if (moveValue[1] > 0)
        {
            for (int i = 1; i <= moveValue[1]; i++)
            {
                if (Mathf.FloorToInt(pos / 3f) == Mathf.FloorToInt((pos + i) / 3f)) { targets.Add(pos + i); }
                else { break; }
            }
        }
        if (moveValue[2] > 0)
        {
            for (int i = 1; i <= moveValue[2]; i++)
            {
                if (Mathf.FloorToInt(pos / 3f) == Mathf.FloorToInt((pos - i) / 3f))
                {
                    targets.Add(pos - i);
                }
                else { break; }
            }
        }
        if (moveValue[3] > 0)
        {
            for (int i = 1; i <= moveValue[3]; i++)
            {
                if (pos - (3 * i) >= minPos) { targets.Add(pos - (i * 3)); }
                else { break; }
            }
        }

        return targets;
    }
    public List<int> GetEmptyPos(List<int> range)
    {
        List<int> empty = new List<int>();
        foreach(int pos in range)
        {
            if (!CheckCharaExist(pos)) { empty.Add(pos); }
        }
        return empty;
    }
    public Vector2 GetCharacterWorldPos(int pos)
    {
        return charactersWorldPos_Size1[pos];
    }

    public bool CheckVictory()
    {
        foreach (Character chara in existingCharacters)
        {
            if(chara.GetCharacterStatus().position >= 9&&!chara.GetCharacterStatus().obstacle) { return false; }//敵側に障害物でないキャラがいるなら勝利してない
        }
        return true;
    }



    public void ResetAllTargetIcons()
    {
        foreach(Character_TargetButton targetButton in targetButtons_size1) { targetButton.ResetTargetIcon(); }
        //foreach (Character_TargetButton targetButton in targetButtons_size2) { if (targetButton != null) { targetButton.ResetTargetIcon(); }; }
        //foreach (Character_TargetButton targetButton in targetButtons_size3) { if (targetButton != null) { targetButton.ResetTargetIcon(); }; }
    }
    public void ResetAllActionInvolvedIcons()
    {
        foreach (Character_TargetButton targetButton in targetButtons_size1) { targetButton.ResetActionInvolvedIcon(); }
        //foreach (Character_TargetButton targetButton in targetButtons_size2) { if (targetButton != null) { targetButton.ResetActionInvolvedIcon(); }; }
        //foreach (Character_TargetButton targetButton in targetButtons_size3) { if (targetButton != null) { targetButton.ResetActionInvolvedIcon(); }; }
    }
    public void ReseAlltSelectedIcons()
    {
        foreach (Character_TargetButton targetButton in targetButtons_size1) { targetButton.SetSelectedIcon(false); }
    }
    private void Start()
    {
        generatedCharacters=new List<Character>();

        existingCharacters = new List<Character>();

        infoText = FindObjectOfType<InfoText>();
        util = FindObjectOfType<Utility>();
    }


    public void SpawnPlayer(CharacterData characterData,int pos)
    {
        if (pos >= 9) { print("プレイヤーを召喚するのに、指定した位置がエネミー側です!"); }
        Character.CharacterStatus generatedCharaStatus = new Character.CharacterStatus();

        generatedCharaStatus.Init(characterData, generatedCharacters.Count);
        generatedCharaStatus.position = pos;

        Vector2 worldPos = charactersWorldPos_Size1[pos];
        Character_TargetButton tb = targetButtons_size1[pos];

        var co = Instantiate(characterObject, worldPos, Quaternion.identity);
        co.GetComponent<Character_Object>().Init(generatedCharaStatus, characterData.manager,tb,false);
    }

    public void SpawnEnemy(CharacterData characterData, int pos,bool dropItem)
    {
        if (pos < 9) { print("エネミーを召喚するのに、指定した位置がプレイヤー側です!"); }
        Character.CharacterStatus generatedCharaStatus = new Character.CharacterStatus();

        generatedCharaStatus.Init(characterData, generatedCharacters.Count);
        generatedCharaStatus.position = pos;

        Vector2 worldPos = charactersWorldPos_Size1[pos];
        Character_TargetButton tb = targetButtons_size1[pos];

        var co = Instantiate(characterObject, worldPos, Quaternion.identity);
        co.GetComponent<Character_Object>().Init(generatedCharaStatus, characterData.manager, tb, dropItem);
    }

}
