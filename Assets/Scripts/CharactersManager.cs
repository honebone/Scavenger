using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [SerializeField] Transform CharactersP;

    [SerializeField]//test
    List<Character> generatedCharacters;
    List<Character> existingCharacters;

    ExpeditionManager expeditionManager;
    InfoText infoText;
    Utility util;

    public static CharactersManager inst;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        expeditionManager = ExpeditionRef.expeditionManager;
        generatedCharacters = new List<Character>();

        existingCharacters = new List<Character>();

        infoText = FindObjectOfType<InfoText>();
        util = FindObjectOfType<Utility>();

        for (int i = 0; i < targetButtons_size1.Length; i++)
        {
            targetButtons_size1[i].SetPosition(i);
        }
    }

    public void AddCharacter(Character character)
    {
        generatedCharacters.Add(character);
        existingCharacters.Add(character);
        SortExistingCharacters();
    }
    public void SortExistingCharacters()
    {
        existingCharacters.Sort((a, b) => a.CharaStatus().position - b.CharaStatus().position);
    }
    /// <summary>生存している全てのキャラを返します </summary>
    public List<Character> GetExistingCharacters_All() { return existingCharacters; }
    public List<Character> GetExistingCharacters(List<int> positions, bool includeEmpty)
    {
        List<Character> characters = new List<Character>();
        foreach (int pos in positions) { if (CheckCharaExist(pos) || includeEmpty) { characters.Add(GetCharacterWithPos(pos)); } }
        return characters;
    }

    public List<Character.CharacterStatus> GetExistingCharactersStatus()
    {
        List<Character.CharacterStatus> charactersStatus = new List<Character.CharacterStatus>();
        foreach (Character existingCharacter in existingCharacters) { charactersStatus.Add(existingCharacter.CharaStatus()); }
        return charactersStatus;
    }


    /// <summary>移動処理に使用　進行方向にいるキャラを取得</summary>
    /// <param name="ranges">0:right 1:upper 2:lower 3:left</param>
    /// <returns></returns>
    public List<Character> GetTravelingDirCharas(int pos, int dir, int range)
    {
        List<Character> c = new List<Character>();
        for (int i = 1; i <= range; i++)
        {
            if (CheckCharaExist(util.GetMoveToPos(pos, dir, i)) && !c.Contains(GetCharacterWithPos(util.GetMoveToPos(pos, dir, i))))
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

    public Character_TargetButton GetTargetButton(int pos) { return targetButtons_size1[pos]; }
    public PositionManager GetPositionManager(int pos) { return positionManagers[pos]; }
    public PositionManager[] GetPositionManagers() { return positionManagers; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="targetEmpty">空きスペースを対象とするか</param>
    /// <param name="size">空きスペースを対象とする場合、そのサイズ</param>
    /// <param name="targetGroup"></param>
    public void SetTargetIcon(int pos, List<int> targetGroup)//,bool targetEmpty
    {
        //if (targetEmpty)
        //{
        //    GetTargetButton(pos).SetTargetIcon(targetGroup);
        //}
        //else
        //{
        //    if (CheckCharaExist(pos))
        //    {
        //        GetTargetButton( GetCharacterWithPos(pos).GetCharacterStatus().position).SetTargetIcon(targetGroup);
        //    }
        //}
        GetTargetButton(pos).SetTargetIcon(targetGroup);
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
        public bool onlyNonplayable;
        [Space]
        public bool front;
        public bool mid;
        public bool back;

        [Header("\n\n\n<検索条件の指定>")]
        public bool excludeObstacle;
        public List<CharacterData> characterInclude;
        public List<CharacterData.CharacterTag> characterTags;
        public List<CharacterData.CharacterTag> characterTagsExclude;

        [System.Serializable]
        public class StECondition
        {
            public GameObject StE;
            [Header("同数も許可")] public int stack = 1;
            public bool stack_lessThan;
        }
        public List<StECondition> StEConditions;
        [Header("これは旧式のStE条件です")] public List<GameObject> StE;
        public List<GameObject> StEExclude;
        public List<GameObject> PE;
        public List<GameObject> PEExclude;
        public bool canEcho;
        [Header("反響の対象となるStE(なしでも可)")]
        public List<GameObject> echoTargets;

        public float HPPercent;
        public bool HP_lessThan;
        public bool HP_excludeEqual;

        [Header("\n\n\n<参照のキャラとの比較>")]
        public List<Vector2Int> neighbor;
        public bool sameColmn;
        public bool sameRow;
    }

    public bool ExamineCharacter(Character character, SearchCharaCondition condition ,Character refChara=null)
    {
        if (condition.searchAsPos)
        {
            infoText.AddErrorText("ポジションを検査するためにキャラクター検索を行っています!!");
            return false;
        }
        Character.CharacterStatus status = character.CharaStatus();
        if (!condition.player && status.position < 9) { return false; }
        if (!condition.enemy && status.position >= 9) { return false; }
        if (condition.onlyPlayable && !status.playable) { return false; }
        if (condition.onlyNonplayable && status.playable) { return false; }
        if (!condition.front && status.position.GetColumn() == 0) { return false; }
        if (!condition.mid && status.position.GetColumn() == 1) { return false; }
        if (!condition.back && status.position.GetColumn() == 2) { return false; }
        if (condition.excludeObstacle && status.Obstacle()) { return false; }

        bool matched = condition.characterTags.Count == 0;
        foreach (CharacterData.CharacterTag tag in condition.characterTags)
        {
            if (status.characterTags.Contains(tag))
            {
                matched = true;
                break;
            }
        }
        if (!matched) { return false; }



        matched = condition.characterInclude.Count == 0 || condition.characterInclude.Contains(status.characterData);
        if (!matched) { return false; }

        foreach (CharacterData.CharacterTag tag in condition.characterTagsExclude)
        {
            if (status.characterTags.Contains(tag))
            {
                return false;
            }
        }

        matched = condition.StEConditions.Count == 0;
        foreach (SearchCharaCondition.StECondition cond in condition.StEConditions)
        {
            int stack = character.GetStEStack_Sum(cond.StE);
            if (cond.stack_lessThan && stack <= cond.stack)
            {
                matched = true;
                break;
            }
            else if (!cond.stack_lessThan && stack >= cond.stack)
            {
                matched = true;
                break;
            }
        }
        if (!matched) { return false; }

        matched = condition.StE.Count == 0;
        foreach (GameObject s in condition.StE)
        {
            if (character.CheckHasStE(s))
            {
                matched = true;
                break;
            }
        }
        if (!matched) { return false; }



        matched = true;
        foreach (GameObject s in condition.StEExclude)
        {
            if (character.CheckHasStE(s))
            {
                matched = false;
                break;
            }
        }
        if (!matched) { return false; }



        matched = condition.PE.Count == 0;
        foreach (GameObject s in condition.PE)
        {
            if (character.CheckHasPE(s))
            {
                matched = true;
                break;
            }
        }
        if (!matched) { return false; }



        matched = true;
        foreach (GameObject s in condition.PEExclude)
        {
            if (character.CheckHasPE(s))
            {
                matched = false;
                break;
            }
        }
        if (!matched) { return false; }

        if (condition.canEcho && !character.CanEcho(condition.echoTargets)) { return false; }

        float HPPercent = status.HP.GetPercent(status.maxHP);
        if (condition.HP_lessThan)
        {
            if (condition.HP_excludeEqual && HPPercent >= condition.HPPercent) { return false; }//HP% < 条件値 のみ通す
            if (!condition.HP_excludeEqual && HPPercent > condition.HPPercent) { return false; }//HP% <= 条件値 のみ通す
        }
        else
        {
            if (condition.HP_excludeEqual && HPPercent <= condition.HPPercent) { return false; }//HP% > 条件値 のみ通す
            if (!condition.HP_excludeEqual && HPPercent < condition.HPPercent) { return false; }//HP% >= 条件値 のみ通す
        }

        if (refChara != null)
        {
            Character.CharacterStatus refStat = refChara.CharaStatus();

            matched = condition.neighbor.Count == 0 || condition.neighbor.Any(x => refStat.position.PosIntToVector() + x == status.position.PosIntToVector());
            if (!matched) { return false; }

            if (condition.sameColmn && refStat.position.GetColumn() != status.position.GetColumn()) { return false; }
            if (condition.sameRow && refStat.position.GetRow() != status.position.GetRow()) { return false; }
        }

        return true;
    }

    public List<Character> SearchCharaWithCondition(SearchCharaCondition condition, Character refChara = null)
    {
        if (condition.searchAsPos)
        {
            infoText.AddErrorText("ポジションを検索するためにキャラクター検索を行っています!!");
            return null;
        }
        List<Character> list = new List<Character>();
        foreach (Character character in existingCharacters)
        {
            if (ExamineCharacter(character, condition, refChara)) list.Add(character);

        }

        return list;
    }

    //=============================================================================[キャラ検索便利系]=====================================================================
    /// <summary>
    /// 最もHP(またはHP割合)の小さいキャラ
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    public List<Character> SearchChara_Weakest(SearchCharaCondition condition, bool percent)
    {
        List<Character> pool = new List<Character>(SearchCharaWithCondition(condition));
        float minValue = 0;
        for (int i = 0; i < pool.Count; i++)
        {
            if (percent)
            {
                if (i == 0 || pool[i].CharaStatus().GetHPPercent() <= minValue) { minValue = pool[i].CharaStatus().GetHPPercent(); }
            }
            else
            {
                if (i == 0 || pool[i].CharaStatus().HP <= minValue) { minValue = pool[i].CharaStatus().HP; }
            }
        }

        List<Character> list = new List<Character>();

        foreach (Character chara in pool)
        {
            if (percent)
            {
                if (chara.CharaStatus().GetHPPercent() == minValue) { list.Add(chara); }
            }
            else
            {
                if (chara.CharaStatus().HP == minValue) { list.Add(chara); }
            }
        }

        return list;
    }

    /// <summary>
    /// 最もHP(またはHP割合)の大きいキャラ
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    public List<Character> SearchChara_Strongest(SearchCharaCondition condition, bool percent)
    {
        List<Character> pool = new List<Character>(SearchCharaWithCondition(condition));
        float maxValue = 0;
        for (int i = 0; i < pool.Count; i++)
        {
            if (percent)
            {
                if (i == 0 || pool[i].CharaStatus().GetHPPercent() >= maxValue) { maxValue = pool[i].CharaStatus().GetHPPercent(); }
            }
            else
            {
                if (i == 0 || pool[i].CharaStatus().HP >= maxValue) { maxValue = pool[i].CharaStatus().HP; }
            }
        }

        List<Character> list = new List<Character>();

        foreach (Character chara in pool)
        {
            if (percent)
            {
                if (chara.CharaStatus().GetHPPercent() == maxValue) { list.Add(chara); }
            }
            else
            {
                if (chara.CharaStatus().HP == maxValue) { list.Add(chara); }
            }
        }

        return list;
    }

    /// <summary>いずれかの陣営のキャラすべてを返す</summary>
    public List<Character> SearchChara_AllInOneSide(bool player)
    {
        List<Character> list = new List<Character>();
        foreach (Character chara in GetExistingCharacters_All())
        {
            if (chara.PlayerPos() == player) list.Add(chara);
        }
        return list;
    }

    public List<int> SearchPosWithCondition(SearchCharaCondition condition)
    {
        if (!condition.searchAsPos)
        {
            infoText.AddErrorText("キャラクターを検索するためにポジション検索を行っています!!");
            return null;
        }
        List<int> list = new List<int>();
        for (int i = 0; i < 18; i++)
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

            matched = true;
            foreach (GameObject s in condition.PEExclude)
            {
                if (GetPositionManager(i).CheckHasPE(s))
                {
                    matched = false;
                    break;
                }
            }
            if (!matched) { continue; }


            list.Add(i);

        }
        //infoText.AddDebugText(list.Count.ToString());
        return list;
    }

    public Character GetCharacterWithPos(int pos)
    {
        foreach (Character character in GetExistingCharacters_All())
        {
            Character.CharacterStatus characterStatus = character.CharaStatus();
            if (characterStatus.position == pos) { return character; }
        }
        infoText.AddDebugText(string.Format("error:ポジション{0}にキャラクターは存在していません", pos));
        return null;
    }
    public List<Character> GetCharactersWithPos(List<int> posList)
    {
        List<Character> list = new List<Character>();
        foreach (int posInt in posList)
        {
            if (CheckCharaExist(posInt)) { list.Add(GetCharacterWithPos(posInt)); }
        }
        return list;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveValue">0:right 1:upper 2:lower 3:left</param>
    /// <returns></returns>
    public List<int> GetMoveTargets(int pos, List<int> moveValue)
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
        foreach (int pos in range)
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
            if (chara.CharaStatus().position >= 9 && !chara.CharaStatus().Obstacle() && !chara.CharaStatus().minion) { return false; }//敵側に障害物でないキャラがいるなら勝利してない
        }
        return true;
    }
    public bool CheckDefeat()
    {
        foreach (Character chara in existingCharacters)
        {
            if (chara.CharaStatus().player) { return false; }//playerがいるなら敗北してない
        }
        return true;
    }

    public void ResetPlayerPos()
    {
        foreach (Character chara in existingCharacters)
        {
            if (!chara.CharaStatus().player) { infoText.AddErrorText("戦闘終了時にプレイヤー以外のキャラがいます"); }
        }
        foreach (Character character in GetExistingCharacters_All()) { character.GetTargetButton().ResetCharacter(); }
        foreach (Character character in GetExistingCharacters_All()) { character.ResetPos(); }

        SortExistingCharacters();
    }



    public void ResetAllTargetIcons()
    {
        foreach (Character_TargetButton targetButton in targetButtons_size1) { targetButton.ResetTargetIcon(); }
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

    /// <summary>
    /// プレイヤーを復活させ、それ以外は破壊
    /// </summary>
    public void DestroyOrRespawnDead()
    {
        List<Character> remove = new List<Character>();
        List<Character> respawn = new List<Character>();
        foreach (Character c in generatedCharacters)
        {
            if (c.CharaStatus().dead)
            {
                if (c.IsPlayer())//プレイヤーの場合は復活
                {
                    respawn.Add(c);
                }
                else//プレイヤーでない場合は削除予定に追加
                {
                    remove.Add(c);
                }
            }
        }

        foreach (Character c in remove)
        {
            generatedCharacters.Remove(c);
            Destroy(c.GetCharacter_Object().gameObject);
        }

        List<int> playerPos = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        List<int> emptyPlayerPos = GetEmptyPos(playerPos);
        respawn.ForEach(c =>
        {
            generatedCharacters.Remove(c);
            if (emptyPlayerPos.Count == 0) infoText.AddErrorText("プレイヤーの空きスペースがありません");
            int pos = emptyPlayerPos[0];
            emptyPlayerPos.Remove(pos);
            c.Respawn(pos);
            if (GameManager.gameParams.SANPenaltyOnDie > 0) c.SANDamage(GameManager.gameParams.SANPenaltyOnDie);
        });
    }

    public Character SpawnPlayer(CharacterData characterData, int pos, int LVL, Character.SummonCharaStatusParams summonCharaParams = null)
    {
        if (pos >= 9) { print("プレイヤーを召喚するのに、指定した位置がエネミー側です!"); }
        Character.SummonCharaStatusParams statusParams = (summonCharaParams == null) ? new Character.SummonCharaStatusParams() : summonCharaParams;
        statusParams.LVL = LVL;

        SpawnCharaParams spawnParams = new SpawnCharaParams();

        Character.CharacterStatus generatedCharaStatus = new Character.CharacterStatus();

        generatedCharaStatus.Init(characterData);
        generatedCharaStatus.position = pos;
        spawnParams.generatedCharaStatus = generatedCharaStatus;
        spawnParams.manager = characterData.manager;

        Vector2 worldPos = charactersWorldPos_Size1[pos];
        Character_TargetButton tb = targetButtons_size1[pos];

        spawnParams.targetButton = tb;
        spawnParams.dropItem = false;
        spawnParams.summonCharaParams = statusParams;

        var co = Instantiate(characterObject, worldPos, Quaternion.identity, CharactersP);
        return co.GetComponent<Character_Object>().Init(spawnParams);
    }

    public Character SpawnEnemy(CharacterData characterData, int pos, bool dropItem, int LVL, Character.SummonCharaStatusParams summonCharaParams = null)
    {
        if (pos < 9) { print("エネミーを召喚するのに、指定した位置がプレイヤー側です!"); }
        Character.SummonCharaStatusParams statusParams = (summonCharaParams == null) ? new Character.SummonCharaStatusParams() : summonCharaParams;
        statusParams.LVL = LVL;

        SpawnCharaParams spawnParams = new SpawnCharaParams();

        GameParams gp = GameManager.gameParams;
        if (!characterData.boss&&!characterData.characterTags.Contains(CharacterData.CharacterTag.obstacle))
        {
            foreach (GameObject PA in expeditionManager.GetMadnessPA().Shuffle())
            {
                if (gp.madnessSpawnChance.Dice())
                {
                    statusParams.PAs.Add(PA);
                    statusParams.statusMods.Add(gp.madnessStatMod);
                    spawnParams.madness = true;
                    break;
                }
            }
        }


        Character.CharacterStatus generatedCharaStatus = new Character.CharacterStatus();

        generatedCharaStatus.Init(characterData);
        generatedCharaStatus.position = pos;
        spawnParams.generatedCharaStatus = generatedCharaStatus;
        spawnParams.manager = characterData.manager;

        Vector2 worldPos = charactersWorldPos_Size1[pos];
        Character_TargetButton tb = targetButtons_size1[pos];

        spawnParams.targetButton = tb;
        spawnParams.dropItem = dropItem;
        spawnParams.summonCharaParams = statusParams;

        var co = Instantiate(characterObject, worldPos, Quaternion.identity, CharactersP);
        return co.GetComponent<Character_Object>().Init(spawnParams);
    }
}

public class SpawnCharaParams
{
    public Character.CharacterStatus generatedCharaStatus;
    public GameObject manager;
    public Character_TargetButton targetButton;
    public bool dropItem;
    public bool madness;
    public Character.SummonCharaStatusParams summonCharaParams = null;
}
