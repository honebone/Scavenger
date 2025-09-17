using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterStatus
    {
        public string fileName;
        public CharacterData characterData;
        public List<CharacterData.CharacterTag> characterTags;
        public string charaName;
        //public int size;
        public bool immovable;

        public bool notChara;
        public bool player;
        public bool playable;
        //public bool obstacle;
        /// <summary>0:idle 1:damaged </summary>
        public GameObject[] variableSprites;
        public Sprite spriteForUI;
        public Ability.AbilityStatus[] abilitiesStatus;

        public List<GameObject> passiveAbilities;
        public List<GameObject> actionMods;

        public CharacterData corpse;

        //public EquipmentType[] equipableTypes;
        //[Header("equipableTypesと要素数を合わせる")]
        //public Equipment[] equipments;

        public int level;
        public int exp;

        public int lifetime;
        public bool surviveFatalWounds;

        public int maxHP;
        public int maxHP_base;
        public int maxHP_baseByLVL;
        public float maxHP_mul;
        /// <summary>乗算はこれに乗らない</summary>
        public int maxHP_int;

        public int maxSAN;
        public int maxSAN_base;
        public float maxSAN_mul;

        public int ATK;
        public int ATK_base;
        public int ATK_baseByLVL;
        public float ATK_mul;
        public int ATK_int;

        public int INT;
        public int INT_base;
        public int INT_baseByLVL;
        public float INT_mul;
        public int INT_int;

        public float exDMG_mul;

        public float CRITC;
        /// <summary>後から代入される </summary>
        public float CRITD_base;
        public float CRITD;

        public float EVD;
        public float ACC;

        public int ACT;
        public int turnPerRound;
        public int exTurn;

        /// <summary>このラウンドで終了した自身のターン数</summary>
        public int spendTurn;

        public float GHeal;
        public float RHeal;

        public List<StEResist> StEResists;
        public List<StEApplyBonus> StEApplyBonus;
        public float debuffChance;

        public float moveRes;
        public float debuffRes;

        public int position;
        public int position_battleStart;

        public int HP;
        public int shield;

        public float PROT;

        public int SAN;

        public bool doesDropItem;

        public int equipmentSlots;
        public List<Definer.Item> equipments;

        public Character summoner;

        //以下バフ
        public int hide;

        //以下デバフ
        public int marked;
        public int focused;
        public int stun;

        public bool dead;
        //ここに状態異常入れれるといいね 

        public string GetInfo()
        {
            string s = "";
            if (player && !playable) { s += "操作不可\n"; }
            bool f = false;
            s += "タグ：[";
            foreach (CharacterData.CharacterTag tag in characterTags)
            {
                if (f) { s += ", "; }
                f = true;
                s += Definer.CharacterTagName[tag];
            }
            s += "]\n";
            if (immovable) { s += "移動不可\n"; }
            if (player)
            {
                s += string.Format("LVL：{0}(次のLVLまで{1}/{2})\n", level, exp, GetNextExp());
            }
            else
            {
                s += string.Format("LVL：{0}\n", level);
            }
            if (lifetime > 0) { s += $"{"寿命".ToLinkKey()}：{lifetime}\n"; }
            string s2 = $"{maxHP_base + maxHP_baseByLVL} {(maxHP_mul - 100).GetValueWithSign()}％ {maxHP_int.GetValueWithSign()}".ColorStr(Color.gray);
            //s += string.Format("HP/maxHP：{0}/{1} {2}\n", HP, maxHP, s2);
            s += $"{"HP".ToSpr_withName()}：{HP}/{maxHP} {s2}\n";
            if (shield > 0) { s += $"{"shield".ToSpr_withLink().ColorStr(Definer.colorRef.shield)}：{shield}\n"; }
            //if (PROT != 0) { s += ValueToStr("PROT", PROT, $" {"(上限75)".ColorStr(Color.gray)}"); }
            if (player) { s += $"{"SAN".ToSpr_withLink()}：{SAN}/{maxSAN}\n"; }
            else { s += "\n"; }

            s2 = $"{ATK_base + ATK_baseByLVL} {(ATK_mul - 100).GetValueWithSign()}％ {ATK_int.GetValueWithSign()}".ColorStr(Color.gray);
            s += $"{"ATK".ToSpr_withLink()}：{ATK} {s2}\n";
            s2 = $"{INT_base + INT_baseByLVL} {(INT_mul - 100).GetValueWithSign()}％ {INT_int.GetValueWithSign()}".ColorStr(Color.gray);
            s += $"{"INT".ToSpr_withLink()}：{INT} {s2}\n";
            s += $"{"CRIT".ToSpr_withLink()}率：{CRITC}％\n";
            s += $"{"CRIT".ToSpr_withLink()}ダメージ：{CRITD}％ {$"{CRITD_base} {(CRITD - CRITD_base).GetValueWithSign()}".ColorStr(Color.gray)}\n";
            //s += string.Format("CRIT：{0}％で{1}％ダメージ\n", CRITC, CRITD);
            s += ValueToStr("与ダメージ", exDMG_mul, "％\n");
            s += "\n";

            if (PROT != 0) { s += $"{"PROT".ToSpr_withLink()}：{PROT} {"(上限75)\n".ColorStr(Color.gray)}"; }
            s += $"{"EVD".ToSpr_withLink()}：{EVD} {"(上限75)".ColorStr(Color.gray)}\n";
            s += $"{"ACC".ToSpr_withLink()}：{ACC}\n\n";

            s += $"{"ACT".ToSpr_withLink()}：{ACT}\n";
            s += string.Format("ラウンド毎ターン数：{0}\n\n", turnPerRound);

            if (GHeal != 0) { s += $"与える回復量：{GHeal.Evaluate()}％\n"; }
            if (RHeal != 0) { s += $"受ける回復量：{RHeal.Evaluate()}％\n"; }

            foreach (StEResist res in StEResists)
            {
                if (res.value != 0)
                {
                    s += string.Format("{0}耐性{1}％\n", res.ResStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().ToLinkKey(), res.value);
                }
            }
            foreach (StEApplyBonus bonus in StEApplyBonus)
            {
                string StEName = bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().ToLinkKey();
                if (bonus.exChance != 0) { s += ValueToStr(string.Format("{0}付与確率", StEName), bonus.exChance, "％"); }
                if (bonus.exStack != 0) { s += ValueToStr(string.Format("{0}付与スタック数", StEName), bonus.exStack, ""); }
                if (bonus.exValue != 0) { s += ValueToStr($"付与する{StEName}の<color=#FFBF69><i>{{効果量}}</i></color>", bonus.exValue, ""); }
            }
            if (debuffChance != 0) { s += $"{"debuff".ToSpr_withName()}付与確率{debuffChance}％\n"; }
            if (moveRes != 0) { s += string.Format("移動耐性{0}％\n", moveRes); }
            if (debuffRes != 0) { s += $"{"debuff".ToSpr_withName()}耐性{debuffRes}％\n"; }

            //foreach (GameObject actionMod in actionMods)
            //{
            //    s += actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo();
            //}
            return s;
        }

        public void Init(CharacterData data)
        {
            fileName = data.fileName;
            characterData = data;
            characterTags = new List<CharacterData.CharacterTag>(data.characterTags);
            charaName = data.charaName;
            //size = data.size;
            immovable = data.immovable;

            notChara = data.notChara;
            player = data.player;
            playable = data.playable;
            //obstacle = characterTags.Contains(CharacterData.CharacterTag.obstacle);
            variableSprites = data.variableSprites;
            spriteForUI = data.spriteForUI;

            abilitiesStatus = new Ability.AbilityStatus[data.abilities.Length];
            //FindObjectOfType<InfoText>().AddDebugText(abilitiesStatus[0].abilityName);
            for (int i = 0; i < abilitiesStatus.Length; i++) { abilitiesStatus[i] = new Ability.AbilityStatus(data.abilities[i], i); }
            passiveAbilities = new List<GameObject>(data.passiveAbilities);

            actionMods = new List<GameObject>(data.actionMods);

            corpse = data.corpse;

            level = 1;

            lifetime = data.lifetime;
            surviveFatalWounds = data.surviveFatalWounds;
            maxHP_base = data.maxHP;
            maxHP_mul = 100f;
            maxHP = data.maxHP;
            maxSAN_base = data.maxSAN;
            maxSAN_mul = 100f;
            maxSAN = data.maxSAN;

            ATK_base = data.ATK;
            ATK_mul = 100f;
            ATK = data.ATK;

            INT_base = data.INT;
            INT_mul = 100f;
            INT = data.INT;

            CRITC = data.CRITC;
            CRITD_base = data.CRITD;
            CRITD = data.CRITD;

            EVD = data.EVD;
            ACC = data.ACC;

            ACT = data.ACT;
            turnPerRound = data.turnPerRound;

            GHeal = data.GHeal;
            RHeal = data.RHeal;

            debuffRes = data.debuffRes;

            StEResists = new List<StEResist>(data.StEResists);
            StEApplyBonus = new List<StEApplyBonus>(data.StEApplyBonus);

            moveRes = data.moveRes;

            equipmentSlots = 4;

            equipments = new List<Definer.Item>();
        }
        public Vector2Int posIntToVector() { return new Vector2Int(position % 3, Mathf.FloorToInt(position / 3)); }
        public float GetStERes(PA_StatusEffect.StatusEffectParams StEParams)
        {
            float resist = 0;
            foreach (StEResist res in StEResists)
            {
                if (res.ResStE == StEParams.applyStE) { resist = res.value; }
            }

            if (StEParams.GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff)
            {
                resist += debuffRes;
            }

            return resist;
        }
        public bool CheckHasStEApplyBonus(GameObject StE)
        {
            string StEName = StE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName;
            //Debug.Log($"{charaName}の{StE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName}探索開始");
            foreach (StEApplyBonus bonus in StEApplyBonus)
            {
                //Debug.Log($"{bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName}");
                if (bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StEName) { return true; }
            }
            //Debug.Log("見つからず");
            return false;
        }

        /// <summary>絶対にCheckHasStEApplyBonusでチェックしてから呼ぶこと！！！</summary>

        public StEApplyBonus GetStEApplyBonus(GameObject StE)
        {
            string StEName = StE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName;
            foreach (StEApplyBonus bonus in StEApplyBonus)
            {
                if (bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StEName) { return bonus; }
            }
            return null;//ここが呼ばれることはない
        }
        public string ValueToStr(string start, float value, string end)
        {
            if (value == 0) { return ""; }
            string s = start;
            if (value < 0) { s += value.ToString(); }
            else { s += "+" + value.ToString(); }
            s += end + "\n";
            return s;
        }
        /// <summary>％表記で返す</summary>
        public int BaseHP() { return maxHP_base + maxHP_baseByLVL; }
        public int BaseATK() { return ATK_base + ATK_baseByLVL; }
        public int BaseINT() { return INT_base + INT_baseByLVL; }
        /// <summary>%で返す</summary>
        public float GetHPPercent() { return HP * 100f / maxHP; }
        public int GetNextExp() { return level; }

        public bool Obstacle() { return characterTags.Contains(CharacterData.CharacterTag.obstacle); }
    }
    [System.Serializable]
    public struct CharaStatusMod
    {
        public int LVL;

        public float maxHP_mul;
        public int maxHP_int;

        public float maxSAN_mul;

        public float PROT;

        public float ATK_mul;
        public int ATK_int;

        public float INT_mul;
        public int INT_int;

        public float exDMG_mul;

        public float CRITC;
        public float CRITD;

        public float EVD;
        public float ACC;

        public int ACT;
        public int turnPerRound;

        public float GHeal;
        public float RHeal;

        //リスト追加時は絶対にInitに追記しろ！！
        public List<StEResist> StEResists;
        public List<StEApplyBonus> StEApplyBonus;

        public void Init()
        {
            StEResists = new List<StEResist>();
            StEApplyBonus = new List<StEApplyBonus>();
        }
        public float debuffChance;
        public float moveRes;
        public float debuffRes;
        public string GetInfo()
        {
            string info = "";
            bool f = false;
            info += ValueToStr("maxHP".ToSpr_withName(), maxHP_mul, "％");
            info += ValueToStr("maxHP".ToSpr_withName(), maxHP_int, "");
            info += ValueToStr("SAN".ToSpr_withName("maxSAN"), maxSAN_mul, "％");
            info += ValueToStr("PROT".ToSpr_withLink(), PROT, "");
            info += ValueToStr("ATK".ToSpr_withLink(), ATK_mul, "％");
            info += ValueToStr("ATK".ToSpr_withLink(), ATK_int, "");
            info += ValueToStr("INT".ToSpr_withLink(), INT_mul, "％");
            info += ValueToStr("INT".ToSpr_withLink(), INT_int, "");
            info += ValueToStr("与ダメージ", exDMG_mul, "％");
            info += ValueToStr($"{"CRIT".ToSpr_withLink()}率", CRITC, "％");
            info += ValueToStr($"{"CRIT".ToSpr_withLink()}ダメージ", CRITD, "％");
            info += ValueToStr("EVD".ToSpr_withLink(), EVD, "");
            info += ValueToStr("ACC".ToSpr_withLink(), ACC, "");
            info += ValueToStr("ACT".ToSpr_withLink(), ACT, "");
            info += ValueToStr("ラウンド毎ターン数", turnPerRound, "");
            info += ValueToStr("与える回復量", GHeal, "％");
            info += ValueToStr("受ける回復量", RHeal, "％");
            foreach (StEResist res in StEResists)
            {
                info += ValueToStr(string.Format("{0}耐性", res.ResStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().ToLinkKey()), res.value, "％");
            }
            foreach (StEApplyBonus bonus in StEApplyBonus)
            {
                string StEName = bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().ToLinkKey();
                if (bonus.exChance != 0) { info += ValueToStr(string.Format("{0}付与確率", StEName), bonus.exChance, "％"); }
                if (bonus.exStack != 0) { info += ValueToStr(string.Format("{0}付与スタック数", StEName), bonus.exStack, ""); }
                if (bonus.exValue != 0) { info += ValueToStr($"付与する{StEName}の<color=#FFBF69><i>{{効果量}}</i></color>", bonus.exValue, ""); }
                if (bonus.exDMGPerTurn != 0) { info += ValueToStr(string.Format("{0}のHP減少量", StEName), bonus.exDMGPerTurn, "/ターン"); }
            }
            info += ValueToStr($"{"debuff".ToSpr_withName()}付与確率", debuffChance, "％");
            info += ValueToStr("移動耐性", moveRes, "％");
            info += ValueToStr($"{"debuff".ToSpr_withName()}耐性", debuffRes, "％");

            return info;

            string ValueToStr(string start, float value, string end,bool invert=false)
            {
                if (value == 0) { return ""; }
                string s = f ? "\n" : "";
                f = true;
                s += start;
                s += value.Evaluate(invert);
                s += end;
                return s;
            }
        }
    }

    public class SummonCharaStatusParams
    {
        public Character summoner;
        public int LVL = 1;
        public List<CharaStatusMod> statusMods = new List<CharaStatusMod>();
        public List<GameObject> PAs = new List<GameObject>();
        //public SummonCharaStatusParams()
        //{
        //    statusMods = new List<CharaStatusMod>();
        //    PAs = new List<GameObject>();
        //}
    }

    public class PersonalBattleReport
    {
        public int ATKDMG;
        public int INTDMG;
        public int decreaseHP;

        public int RDMG;
        public int RShieldDMG;

        public int GHeal;
        public int GShield;

        public string Report()
        {
            string s = "";

            s += $"与ダメージ：{ATKDMG + INTDMG + decreaseHP}({"ATK".ToSpr()}{ATKDMG.ColorStr(Definer.colorRef.damage)}+{"INT".ToSpr()}{INTDMG.ColorStr(Definer.colorRef.INTDamage)}" +
                $"+{decreaseHP.ColorStr(Definer.colorRef.decreaseHP)})";
            s += $"\n被ダメージ：{RDMG + RShieldDMG}({RDMG.ColorStr(Definer.colorRef.damage)}+{"shieldDMG".ToSpr()}{RShieldDMG.ColorStr(Definer.colorRef.shieldDecrease)})";
            s += $"\n与えた回復/シールド：{GHeal + GShield}({"HP".ToSpr()}{GHeal.ColorStr(Definer.colorRef.heal)},{"shield".ToSpr()}{GShield.ColorStr(Definer.colorRef.shield)})";

            return s;
        }
    }

    protected CharacterStatus charaStatus;
    PersonalBattleReport battleReport = new PersonalBattleReport();

    //[SerializeField]
    //protected Action.ActionStatus[] actionsStatusTest;
    public CharacterStatus CharaStatus() { return charaStatus; }
    public PersonalBattleReport GetBattleReport() { return battleReport; }
    public void ResetBattleReport() { battleReport = new PersonalBattleReport(); }

    Character_Object charaObj;
    Character_TargetButton targetButton;
    public Character_Object GetCharacter_Object() { return charaObj; }
    public Character_TargetButton GetTargetButton() { return targetButton; }

    //protected List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    protected List<PA_StatusEffect> PA_StE = new List<PA_StatusEffect>();
    protected List<PassiveAbility> PA_Per = new List<PassiveAbility>();
    protected List<PassiveAbility> PA_Eq = new List<PassiveAbility>();
    //List<PassiveAbility> deletePAs = new List<PassiveAbility>();

    ActionQueueManager actionQueue;
    BattleManager battleManager;
    protected InfoText infoText;
    protected CharactersManager charactersManager;
    SoundManager soundManager;
    LootPanel loot;
    Definer definer;
    CameraManager cameraManager;
    ExpeditionManager expeditionManager;
    TutorialManager tutorialManager;

    public void Init(SpawnCharaParams spawnParams,Character_Object obj)//CharacterStatus status, Character_Object obj, Character_TargetButton tb, bool dropItem, SummonCharaStatusParams summonCharaParams
    {
        actionQueue = ExpeditionRef.actionQueue;
        battleManager = ExpeditionRef.battleManager;
        infoText = ExpeditionRef.infoText;
        charactersManager = ExpeditionRef.charactersManager;
        soundManager = ExpeditionRef.soundManager;
        loot = ExpeditionRef.loot;
        definer = ExpeditionRef.definer;
        cameraManager = ExpeditionRef.cameraManager;
        expeditionManager = ExpeditionRef.expeditionManager;
        tutorialManager = ExpeditionRef.tutorialManager;

        charaStatus = spawnParams.generatedCharaStatus;
        charaObj = obj;
        targetButton = spawnParams.targetButton;

        charaStatus.HP = charaStatus.maxHP;
        charaStatus.SAN = charaStatus.maxSAN;

        charaStatus.doesDropItem = spawnParams.dropItem;

        if (spawnParams.summonCharaParams != null)
        {
            charaStatus.summoner = spawnParams.summonCharaParams.summoner;
            if (spawnParams.summonCharaParams.LVL > 1)
            {
                StatusGrowth SG = (charaStatus.position.IsPlayerPos()) ? ExpeditionManager.inst.playerStatusGrowth : ExpeditionManager.inst.enemyStatusGrowth;
                StatusMod_ByLVL mod = SG.GetStatusMod(spawnParams.summonCharaParams.LVL);
                mod.SetStatus(charaStatus.maxHP_base, charaStatus.ATK_base, charaStatus.INT_base);

                charaStatus.level = spawnParams.summonCharaParams.LVL;
                charaStatus.maxHP_baseByLVL += mod.maxHP;
                charaStatus.ATK_baseByLVL += mod.ATK;
                charaStatus.INT_baseByLVL += mod.INT;
                AddMaxHP(0, 0, true);
                AddATK(0, 0);
                AddINT(0, 0);

                AddACT(mod.ACT);
                AddEVD(mod.EVD);
                AddACC(mod.ACC);
            }
            foreach (CharaStatusMod mod in spawnParams.summonCharaParams.statusMods)
            {
                ModifyStatus(mod, true, true);
            }
            if (spawnParams.summonCharaParams.PAs.Count > 0) { charaStatus.passiveAbilities.AddRange(new List<GameObject>(spawnParams.summonCharaParams.PAs)); }
        }

        foreach (GameObject pa in charaStatus.passiveAbilities) { AddPA_Personality(pa, false); }

        charaStatus.HP = charaStatus.maxHP;

        charaObj.SetCharaSprite(charaStatus.variableSprites[0]);
        if (!charaStatus.player) { charaObj.DisableSANBar(); }
        charaObj.SetHPandShieldBar();
        charaObj.SetSANBar();

        targetButton.SetCharacter(this);

        //if (charaStatus.position >= 9) { ModifyStatus(expeditionManager.GetEnemyStatusMod(), true, true); }

        if (!charaStatus.playable)
        {
            targetButton.SetDamageText("出現", Definer.colorRef.abilityColors[5]);
            infoText.AddLogText(string.Format("{0}が現れた", charaStatus.charaName));
        }

        bool hasPass = false;
        foreach (Ability.AbilityStatus abilityStatus in charaStatus.abilitiesStatus)
        {
            if (abilityStatus.abilityType == AbilityData.AbilityType.pass)
            {
                hasPass = true;
                break;
            }
        }
        if (!hasPass) { infoText.AddWarningText($"{charaStatus.fileName}にはパスを行うアビリティがありません"); }
        //TurnIconはラウンド開始時にセット
    }
    public List<PA_Equipment> GetEquipments()
    {
        List<PA_Equipment> equipments = new List<PA_Equipment>();
        foreach (PassiveAbility PA in PA_Eq)
        {
            equipments.Add(PA.GetComponent<PA_Equipment>());
        }
        return equipments;
    }
    public List<PassiveAbility> GetPassiveAbilities()
    {
        List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
        passiveAbilities.AddRange(PA_StE);
        passiveAbilities.AddRange(PA_Per);
        passiveAbilities.AddRange(PA_Eq);
        return passiveAbilities;
    }

    public List<PA_StatusEffect> GetStEs() { return new List<PA_StatusEffect>(PA_StE); }

    public List<Eq_Magic> GetMagics(List<Eq_Magic> excludeList=null)
    {
        if (excludeList == null) excludeList = new List<Eq_Magic>();
        return GetPassiveAbilities()
            .OfType<Eq_Magic>()
            .Where(m => !excludeList.Contains(m))
            .ToList();
    }

    public void AddPA_Personality(GameObject paObj, bool note)
    {
        var p = Instantiate(paObj, transform);
        PA_Per.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this, 1, infoText);
        if (note)
        {
            PA_Personality.PersonalityStatus personality = p.GetComponent<PA_Personality>().GetPersonalityStatus();
            targetButton.SetDamageText(string.Format("+特性：{0}", personality.personalityName), Definer.colorRef.personalityColors[(int)personality.personalityType]);
            infoText.AddLogText(string.Format("{0}は新たな特性{1}を得た", charaStatus.charaName, personality.GetName()));
        }
    }
    public void EquipItem(Definer.Item item)
    {
        var p = Instantiate(item.data.manager, transform);
        PA_Eq.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this, 2, infoText);
        item.createdManager = p;
        charaStatus.equipments.Add(item);
    }
    public void UnequipItem(Definer.Item remove, bool returnToInventory = true)
    {
        charaStatus.equipments.Remove(remove);
        //PA_Eq.Remove(remove.createdManager.GetComponent<PassiveAbility>());
        //Destroy(remove.createdManager);
        remove.createdManager.GetComponent<PassiveAbility>().Disable();
        if (returnToInventory)
        {
            FindObjectOfType<Inventory>().AddItem(remove, 1, false);
        }
    }
    public bool CheckSameEquipment(ItemData eq)
    {
        foreach (Definer.Item i in charaStatus.equipments)
        {
            if (i.data == eq) { return true; }
        }
        return false;
    }
    public void RemovePA(PassiveAbility passiveAbility)
    {
        switch (passiveAbility.GetPAType())
        {
            case 0:
                PA_StE.Remove((PA_StatusEffect)passiveAbility);
                break;
            case 1:
                PA_Per.Remove(passiveAbility);
                break;
            case 2:
                PA_Eq.Remove(passiveAbility);
                break;
        }
        // deletePAs.Add(passiveAbility);
    }
    void RemovePA_Execute()
    {
        //foreach (PassiveAbility deletePA in deletePAs)
        //{
        //    switch (deletePA.GetPAType())
        //    {
        //        case 0:
        //            PA_StE.Remove(deletePA);
        //            break;
        //        case 1:
        //            PA_Per.Remove(deletePA);
        //            break;
        //        case 2:
        //            PA_Eq.Remove(deletePA);
        //            break;
        //    }
        //}
        //deletePAs.Clear();
    }
    public void ApplyStE(PA_StatusEffect.StatusEffectParams StEParams, int finalStack, int finalValue, Character applyer)
    {
        bool alreadyExist = false;
        PA_StatusEffect StE = StEParams.applyStE.GetComponent<PA_StatusEffect>();
        PA_StatusEffect.StatusEffectStatus StEStatus = StE.GetStatusEffectStatus();
        if (StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().merge)
        {
            foreach (PA_StatusEffect pa in new List<PA_StatusEffect>(PA_StE))
            {
                if (pa.GetPAType() == 0 && pa.GetStatusEffectStatus().StEName == StE.GetStatusEffectStatus().StEName)//同種のStEがすでにあるならそのスタックを増加
                {
                    pa.AddStack(finalStack);

                    //charaObj.SetDamageText(string.Format("+{0}", StEStatus.StEName), StEStatus.StEType.ToColor());//refvalueとmergeは共存しないので、ここでrefvalueのことを考える必要はない
                    infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, StE.GetPAName()));

                    soundManager.PlaySE(Definer.soundRef.ApplyStE[(int)StE.GetStatusEffectStatus().StEType]);
                    alreadyExist = true;
                }
            }
        }
        if (!alreadyExist)
        {
            var s = Instantiate(StEParams.applyStE, transform);
            PA_StatusEffect pa = s.GetComponent<PA_StatusEffect>();
            PA_StE.Add(pa);
           
            pa.Init_StE(finalStack, finalValue, StEParams.DMGPerTurn, charaObj.SetStEIcon().GetComponent<StEIcon>(), applyer);
            pa.Init(this, 0, infoText);

            if (StEStatus.refValue)
            {
                targetButton.SetDamageText(string.Format("+{0}{1}", StEStatus.StEName, finalValue), StEStatus.ToColor());
                infoText.AddLogText(string.Format("{0}は{1}{2}を付与された", charaStatus.charaName, pa.GetPAName(), finalValue.ToString().ColorStr(StEStatus.ToColor())));
            }
            else
            {
                 targetButton.SetDamageText(string.Format("+{0}", StEStatus.StEName), StEStatus.ToColor());
                 infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, pa.GetPAName()));
                //infoText.AddLogText(string.Format("{0}は{1}を付与された", "test","ok"));
            }

            soundManager.PlaySE(Definer.soundRef.ApplyStE[(int)pa.GetStatusEffectStatus().StEType]);
        }
      
        
    }
    public void RemoveStE(ActionData.RemoveStE removeStE)
    {
        PA_StatusEffect.StatusEffectStatus StE = removeStE.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in GetPassiveAbilities())
        {
            if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.StEName)
            {
                if (removeStE.removeAll) { pa.GetComponent<PA_StatusEffect>().Disable(); }
                else { pa.GetComponent<PA_StatusEffect>().AddStack(removeStE.addAmount); }
            }
        }
        //メッセージ
    }
    public void RemoveStE_ByType(PA_StatusEffect.StatusEffectStatus.StatusEffectType type, int amount)
    {
        if (amount > 0)
        {
            List<PA_StatusEffect> pool = new List<PA_StatusEffect>();
            foreach (PassiveAbility pa in GetPassiveAbilities())
            {
                if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEType == type)
                {
                    if (!pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().undeletable) { pool.Add(pa.GetComponent<PA_StatusEffect>()); }
                }
            }
            if (pool.Count > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    if (pool.Count == 0) { break; }
                    int index = pool.Count.RandIndex();
                    pool[index].Disable();
                    pool.Remove(pool[index]);
                }
            }

        }

    }
    public void ConsumeFocus()
    {
        foreach (PassiveAbility pa in GetPassiveAbilities())
        {
            if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus)
            {
                pa.GetComponent<PA_StatusEffect>().AddStack(-1, false);
                targetButton.SetDamageText("☆フォーカス!!☆", Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus]);
                soundManager.PlaySE(Definer.soundRef.consumeFocus);
            }
        }
    }
    ///// <summary>StE自身がこれを呼んでスタック消費or消去する</summary>
    //public void RemoveStE_BySelf(ActionData.RemoveStE removeStE)
    //{
    //    foreach (PassiveAbility pa in GetPassiveAbilities())
    //    {
    //        if (pa.gameObject == removeStE.removeStE)
    //        {
    //            PA_StatusEffect.StatusEffectStatus StEStatus = pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
    //            if (removeStE.removeAll)
    //            {
    //                pa.GetComponent<PA_StatusEffect>().Disable();
    //                charaObj.SetDamageText(string.Format("-{0}", StEStatus.StEName), StEStatus.StEType.ToColor());
    //                infoText.AddLogText(string.Format("{0}の{1}が消去された", charaStatus.charaName, pa.GetComponent<PA_StatusEffect>().GetPAName()));
    //            }
    //            else
    //            {
    //                pa.GetComponent<PA_StatusEffect>().AddStack(removeStE.addAmount);
    //                charaObj.SetDamageText(string.Format("{0}{1}", StEStatus.StEName, removeStE.addAmount.GetValueWithSign()), Definer.colorRef.failed_unavailable);
    //                infoText.AddLogText(string.Format("{0}の{1}のスタック{2}", charaStatus.charaName, pa.GetComponent<PA_StatusEffect>().GetPAName(), removeStE.addAmount.GetValueWithSign()));
    //            }
    //        }
    //    }
    //    //メッセージ
    //}
    /// <summary>各スタックのリストを返す</summary>
    public List<int> GetStEStacks(GameObject StEObj)
    {
        List<int> stacks = new List<int>();
        PA_StatusEffect.StatusEffectStatus StE = StEObj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in PA_StE)
        {
            if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.StEName)
            {
                stacks.Add(pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().stack);
            }
        }
        return stacks;
    }
    /// <summary>スタックの合計を返す</summary>
    public int GetStEStack_Sum(GameObject StEObj)
    {
        int sum = 0;
        foreach (int stack in GetStEStacks(StEObj)) { sum += stack; }
        return sum;
    }

    public int GetDoTDMG(GameObject StEObj, bool total)
    {
        int DMG = 0;
        PA_StatusEffect.StatusEffectStatus StE = StEObj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in PA_StE)
        {
            if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.StEName)
            {
                int DMGPerTurn = pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().DMGPerTurn;
                DMG += total ? DMGPerTurn * pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().stack : DMGPerTurn;
            }
        }
        return DMG;
    }

    public void AddStEStack(GameObject StEObj, int add)
    {
        PA_StatusEffect.StatusEffectStatus StE = StEObj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in GetPassiveAbilities())
        {
            if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.StEName)
            {
                pa.GetComponent<PA_StatusEffect>().AddStack(add);
            }
        }
    }

    public bool CheckHasStE(GameObject StEObj)
    {
        PA_StatusEffect.StatusEffectStatus StE = StEObj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in PA_StE)
        {
            if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.StEName) { return true; }
            //if (pa.FileName() == fileName) { return true; }
        }
        return false;
    }
    public bool CheckHasPE(GameObject PEObj)
    {
        return targetButton.GetPositionManager().CheckHasPE(PEObj);
    }

    public void DisplayInfo()
    {
        string charaName = charaStatus.charaName;
        if (CheckAffricted()) { charaName = charaName.ColorStr(Definer.colorRef.affricted); }
        infoText.SetCharaInfo(charaName, GetInfo(false), GetInfo(true));
        FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons(charaStatus.abilitiesStatus, this);
        //charaObj.SetSelectedIcon(true);
        targetButton.SetSelectedIcon(true);
    }

    public string GetPACurrentStateInfo()
    {
        string s = "";
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities())
        {
            string info = passiveAbility.GetCurrentStateInfo();
            if (info != "")
            {
                s += $"<{passiveAbility.GetPAName()}>\n{info}\n";
            }
        }
        return s;
    }

    public string GetInfo(bool simple)
    {
        string info = charaStatus.GetInfo();
        info += "\n◇◇状態異常◇◇\n";
        foreach (PassiveAbility pa in PA_StE)
        {
            if (pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().refValue)
            {
                info += string.Format("<{0}>\n{1}\n", pa.GetComponent<PA_StatusEffect>().GetPANameWithValue(), pa.GetPAInfo(simple));
            }
            else
            {
                info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo(simple));
            }

        }

        info += "\n◇◇特性◇◇\n";
        foreach (PassiveAbility pa in PA_Per)
        {
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo(simple));
        }

        if (charaStatus.player)
        {
            info += "\n◇◇装備品◇◇\n";
            foreach (PassiveAbility pa in PA_Eq)
            {
                info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo(simple));
            }
        }

        info += "\n" + targetButton.GetPositionManager().GetPEInfo();

        return info;
    }
    public void ResetCharaSprite()
    {
        if (!charaStatus.dead) { charaObj.SetCharaSprite(charaStatus.variableSprites[0]); }
    }
    public void SetCharaSprite(GameObject sprite)
    {
        charaObj.SetCharaSprite(sprite);
    }
    public void ActAnim() { charaObj.ActAnim(); }

    public void SpawnVisualEffect(GameObject VE)
    {
        Vector2 VEPos = charactersManager.GetCharacterWorldPos(charaStatus.position);
        Vector2 VEOffset = VE.GetComponent<VisualEffect>().GetOffset();
        if (charaStatus.position < 9) { VEOffset.x *= -1; }
        var v = Instantiate(VE, VEPos + VEOffset, VE.transform.rotation);
        if (charaStatus.position < 9) { v.transform.Rotate(new Vector3(0, 180, 0), Space.World); }//プレイヤーの時左右反転
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionStatus"></param>
    /// <param name="setTargets">actionTargetに第3引数を代入するか</param>
    /// <param name="actionTargets">setTargetsがtrueの時、これを対象群として改めてactionstatusに代入する</param>
    public bool Enqueue(Action.ActionStatus actionStatus, bool setTargets, List<Character> actionTargets, int targetCount = 0, bool nullOwner = false)
    {
        if (!nullOwner) { actionStatus.actionOwner = this; }
        else { actionStatus.actionOwner = null; }
        if (setTargets) { actionStatus.actionTargets = actionTargets; }

        if ((actionStatus.actionTargets != null && actionStatus.actionTargets.Count > 0) || (actionStatus.actionTargetsInt != null && actionStatus.actionTargetsInt.Count > 0))
        {
            actionQueue.Enqueue(actionStatus, targetCount);
            return true;
        }
        return false;
    }

    public void SetTurnIcon() { charaObj.SetTurnIcons(charaStatus.turnPerRound+ charaStatus.exTurn); }
    public void SetActionInvolvedIcon(bool owner) { targetButton.SetActionInvolvedIcon(owner); }

    //===================================================<<ターン処理>>========================================================
    bool continueTurn;
    public void MyTurnStart()
    {
        charaObj.SetTurnIcon_CurentTurn();
        infoText.AddLogText(string.Format("\n=={0}のターン==", charaStatus.charaName));
        for (int i = 0; i < charaStatus.abilitiesStatus.Length; i++)
        {
            if (charaStatus.abilitiesStatus[i].cooldown > 0)
            {
                charaStatus.abilitiesStatus[i].AddCoolDown(-1);
            }
        }
        battleManager.Trigger_TurnStart();
    }

    public virtual void MainPhase(bool StEFlag=false)
    {
        if (StEFlag)
        {
            foreach (PassiveAbility StE in PA_StE)
            {
                StE.StE_ApplyFlag();
            }
        }
        if (CheckAlive())
        {
            if (charaStatus.stun > 0)//行動不能
            {
                StartCoroutine(Stun());
            }
            else
            {
                if (charaStatus.playable)
                {
                    if (BattleManager.displayInfoOnTS) DisplayInfo();
                    battleManager.SetSelectingAbility(true);
                    battleManager.StartTutorial_Ability();
                }
                else { StartCoroutine(Test()); }
            }
        }
        else
        {
            StartCoroutine(DeleyOnDeath());
        }

    }
    IEnumerator Stun()
    {
        soundManager.PlaySE(Definer.soundRef.stun);
        targetButton.SetDamageText("行動不能!!", Definer.colorRef.failed_unavailable);
        infoText.AddLogText(string.Format("{0}は行動できない!", charaStatus.charaName).ColorStr(Definer.colorRef.failed_unavailable));
        yield return new WaitForSeconds(1f);
        EndPhase();
    }
    IEnumerator Test()
    {
        yield return new WaitForSeconds(0.5f);

        battleManager.SetSelectedAbility(SelectAbility_Random(), this);
        //charaStatus.omenSet = false;
        //charaStatus.omen = new Ability.AbilityStatus();
        battleManager.GetSelectedAbility().StartSelectTarget();
    }
    IEnumerator DeleyOnDeath()
    {
        yield return new WaitForSeconds(0.2f);
        infoText.AddDebugText("死亡につきターンスキップ");
        battleManager.TurnEnd(1);
    }
    public void EndPhase()
    {
        if (CheckAlive())
        {
            if (continueTurn)
            {
                continueTurn = false;
                MainPhase();
            }
            else
            {
                charaObj.SetTurnIcon_End();
                battleManager.TurnEnd(0);
            }
        }
        else { battleManager.TurnEnd(2); }
    }
    public void ContinueTurn() { continueTurn = true; }


    //======================================================<<アクションによって呼ばれる関数>>=================================================

    public void SetDamageText(string str, Color color)
    {
        targetButton.SetDamageText(str, color);
    }
    public void Kill(Character attacker)
    {
        Die(0, attacker);
    }
    public void DecreaseHP(int value)
    {
        if (!charaStatus.dead)
        {
            charaStatus.HP -= value;
            charaObj.SetHPandShieldBar();
            targetButton.SetDamageText(value.ToString(), Definer.colorRef.decreaseHP);
            infoText.AddLogText(string.Format("{0}はHPを{1}失った", charaStatus.charaName, value.ToString().ColorStr(Definer.colorRef.decreaseHP)));
            soundManager.PlaySE(Definer.soundRef.damage);
            if (charaStatus.HP <= 0)
            {
                if (charaStatus.surviveFatalWounds)//瀕死で耐えるキャラは、HP減少によって死なない
                {
                    charaStatus.HP = 0;
                    targetButton.SetDamageText("瀕死!", Definer.colorRef.damage);
                    infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, "瀕死".ColorStr(Definer.colorRef.damage)));
                    soundManager.PlaySE(Definer.soundRef.dying);
                    tutorialManager.Tutorial_dethsDoor();
                    charaObj.SetHPandShieldBar();
                    OnDecreasedHP(value);
                }
                else
                {
                    charaStatus.HP = 0;
                    Die(0, null);
                }
            }
            else
            {
                OnDecreasedHP(value);
            }
        }
    }

    /// <summary>return:殺害したか</summary>
    public bool Damage(Action.OnDamageParams onDamageParams)
    {
        charaStatus.shield -= onDamageParams.shieldDMG;//シールド減少 
        OnDecreasedShield(onDamageParams.shieldDMG);
        if (onDamageParams.shieldDMG > 0)
        {
            targetButton.SetDamageText($"{"shieldDMGL".ToSpr()}{onDamageParams.shieldDMG}", Definer.colorRef.shieldDecrease);
            infoText.AddLogText(string.Format("{0}はシールドを{1}{2}失った", charaStatus.charaName, "shieldDMG".ToSpr(), onDamageParams.shieldDMG.ToString().ColorStr(Definer.colorRef.shieldDecrease)));
            soundManager.PlaySE(Definer.soundRef.shieldDMG);
        }

        SpawnVisualEffect(Definer.VERef.damage);
        if (onDamageParams.CRIT)//テキストの表示
        {
            targetButton.SetDamageText("Critical!!", Definer.colorRef.CRIT);
            infoText.AddLogText($"{"CRIT".ToSpr()}{"Critical!!".ColorStr(Definer.colorRef.CRIT)}");

            if (onDamageParams.ATK)
            {
                targetButton.SetDamageText($"{"CRITL".ToSpr()}{"ATKL".ToSpr()}{onDamageParams.ATKDMG}", Definer.colorRef.CRIT);
                infoText.AddLogText($"{charaStatus.charaName}は{"ATK".ToSpr()}{onDamageParams.ATKDMG.ToString().ColorStr(Definer.colorRef.CRIT)}ダメージを受けた");
            }
            if (onDamageParams.INT)
            {
                targetButton.SetDamageText($"{"CRITL".ToSpr()}{"INTL".ToSpr()}{onDamageParams.INTDMG}", Definer.colorRef.CRIT);
                infoText.AddLogText($"{charaStatus.charaName}は{"INT".ToSpr()}{onDamageParams.INTDMG.ToString().ColorStr(Definer.colorRef.CRIT)}ダメージを受けた");
            }
            soundManager.PlaySE(Definer.soundRef.CRIT);
        }
        else
        {
            if (onDamageParams.ATK)
            {
                targetButton.SetDamageText($"{"ATKL".ToSpr()}{onDamageParams.ATKDMG}", Definer.colorRef.damage);
                infoText.AddLogText($"{charaStatus.charaName}は{"ATK".ToSpr()}{onDamageParams.ATKDMG.ToString().ColorStr(Definer.colorRef.damage)}ダメージを受けた");
            }
            if (onDamageParams.INT)
            {
                targetButton.SetDamageText($"{"INTL".ToSpr()}{onDamageParams.INTDMG}", Definer.colorRef.INTDamage);
                infoText.AddLogText($"{charaStatus.charaName}は{"INT".ToSpr()}{onDamageParams.INTDMG.ToString().ColorStr(Definer.colorRef.INTDamage)}ダメージを受けた");
            }
            if (onDamageParams.totalDMG > 0) soundManager.PlaySE(Definer.soundRef.damage);
        }

        if (charaStatus.HP == 0)//瀕死の状態で1以上のダメージを受けたら死亡する
        {
            if (onDamageParams.totalDMG > 0)
            {
                if (charaStatus.surviveFatalWounds)
                {
                    charaStatus.HP = 0;
                    Die(0, onDamageParams.ap.owner);
                }
                else { print("瀕死で耐えるキャラ出ないのにHP0で生き続けています"); }
            }
            else//0ダメージの時
            {
                charaStatus.HP = 0;
                targetButton.SetDamageText("瀕死!", Definer.colorRef.damage);
                infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, "瀕死".ColorStr(Definer.colorRef.damage)));
                soundManager.PlaySE(Definer.soundRef.dying);
                tutorialManager.Tutorial_dethsDoor();
            }
        }
        else//瀕死でないなら
        {
            charaStatus.HP -= onDamageParams.totalDMG;
            if (charaStatus.HP <= 0)
            {
                if (charaStatus.surviveFatalWounds)//瀕死で耐えるキャラは、瀕死でない状態で致命傷を受けても死なな
                {
                    charaStatus.HP = 0;
                    targetButton.SetDamageText("瀕死!", Definer.colorRef.damage);
                    infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, "瀕死".ColorStr(Definer.colorRef.damage)));
                    soundManager.PlaySE(Definer.soundRef.dying);
                }
                else
                {

                    charaStatus.HP = 0;
                    Die(0, onDamageParams.ap.owner);
                }
            }
        }

        charaObj.SetHPandShieldBar();//HPバーに反映
        if (CheckAlive())
        {
            OnDamaged(onDamageParams);
            if (onDamageParams.totalDMG > 0) OnDecreasedHP(onDamageParams.totalDMG);
            //カウンター
        }
        return !CheckAlive();
    }
    public void Heal(int value, Character healer)
    {
        charaStatus.HP = Mathf.Min(charaStatus.HP + value, charaStatus.maxHP);
        targetButton.SetDamageText($"{"HPL".ToSpr()}{value}", Definer.colorRef.heal);
        infoText.AddLogText(string.Format("{0}はHPを{1}{2}回復した", charaStatus.charaName, "HP".ToSpr(), value.ToString().ColorStr(Definer.colorRef.heal)));
        soundManager.PlaySE(Definer.soundRef.heal);
        charaObj.SetHPandShieldBar();
        SpawnVisualEffect(Definer.VERef.heal);
    }
    public void SANHeal(int value)
    {
        charaStatus.SAN = Mathf.Min(charaStatus.SAN + value, charaStatus.maxSAN);
        targetButton.SetDamageText($"{"SANL".ToSpr()}{value}", Definer.colorRef.SANHeal);
        infoText.AddLogText(string.Format("{0}は正気度を{1}{2}回復した", charaStatus.charaName, "SAN".ToSpr(), value.ToString().ColorStr(Definer.colorRef.SANHeal)));
        soundManager.PlaySE(Definer.soundRef.SANHeal);
        charaObj.SetSANBar();
    }
    public void SANDamage(int value)
    {
        if (charaStatus.player)
        {
            charaStatus.SAN -= value;
            targetButton.SetDamageText($"{"SANDMGL".ToSpr()}{value}", Definer.colorRef.SANDecrease);
            infoText.AddLogText(string.Format("{0}は正気度を{1}{2}失った", charaStatus.charaName, "SANDMG".ToSpr(), value.ToString().ColorStr(Definer.colorRef.SANDecrease)));
            soundManager.PlaySE(Definer.soundRef.SANDecrease);
            charaObj.SetSANBar();
            if (charaStatus.SAN <= 0)
            {
                //if (!CheckAffricted())
                //{
                //    targetButton.SetDamageText("精神崩壊", Definer.colorRef.affricted);
                //    infoText.AddLogText(string.Format("{0}は精神崩壊した!", charaStatus.charaName).ColorStr(Definer.colorRef.affricted));
                //    expeditionManager.AddMadness(1);
                //    //AddPA_Personality(definer.GetAffrictionDataBase().Choice(), true);
                //    expeditionManager.SetPersonality(this, definer.GetAffrictionDataBase().Choice());

                //    charaStatus.SAN = charaStatus.maxSAN;
                //    charaObj.Affrict();
                //    charaObj.SetSANBar();
                //}
                //else
                //{
                //    targetButton.SetDamageText("精神崩壊", Definer.colorRef.affricted);
                //    infoText.AddLogText(string.Format("{0}は精神崩壊した!", charaStatus.charaName).ColorStr(Definer.colorRef.affricted));
                //    expeditionManager.AddMadness(1);
                //    charaStatus.SAN = charaStatus.maxSAN;
                //}

                targetButton.SetDamageText("精神崩壊", Definer.colorRef.affricted);
                infoText.AddLogText(string.Format("{0}は精神崩壊した!", charaStatus.charaName).ColorStr(Definer.colorRef.affricted));
                expeditionManager.AddMadness(1);

                DecreaseHP((charaStatus.maxHP * expeditionManager.gameParams.HPDecOnAffrict / 100f).ToInt());

                charaStatus.SAN = charaStatus.maxSAN;
                charaObj.SetSANBar();

                TutorialManager.inst.SetTutorial("affrict");
            }
        }
    }

    public void AddTurn(int turns)
    {
        targetButton.SetDamageText($"{"ACT".ToSpr(true)}追加ターン {turns}", Definer.colorRef.emphasize);
        infoText.AddLogText($"{charaStatus.charaName}は{"ACT".ToSpr()}追加ターンを{turns}得た");
        if (BattleManager.inRound)
        {
            battleManager.AddTurn(this, false, turns);
            charaObj.AddTurnIcons(turns);
        }
        else
        {
            charaStatus.exTurn += turns;
        }
    }

    //==================================================<<ステータス変更系>>===========================================================
    public void ModifyStatus(CharaStatusMod mod, bool set, bool heal = false)
    {
        int n = 1;
        if (!set) { n = -1; }
        if (mod.LVL != 0) { charaStatus.level += mod.LVL * n; }
        //if (mod.maxHP_mul != 0) { AddMaxHP(0, mod.maxHP_mul * n, heal); }
        AddMaxHP(0, mod.maxHP_mul * n, heal, mod.maxHP_int * n);
        if (mod.maxSAN_mul != 0) { AddMaxSAN(0, mod.maxSAN_mul * n, heal); }
        if (mod.PROT != 0) { AddPROT(mod.PROT * n); }
        //if (mod.ATK_mul != 0) { AddATK(0, mod.ATK_mul * n); }
        AddATK(0, mod.ATK_mul * n, mod.ATK_int * n);
        //if (mod.INT_mul != 0) { AddINT(0, mod.INT_mul * n); }
        AddINT(0, mod.INT_mul * n, mod.INT_int * n);
        if (mod.exDMG_mul != 0) { AddExDMG_Mul(mod.exDMG_mul * n); }
        if (mod.CRITC != 0) { AddCRITC(mod.CRITC * n); }
        if (mod.CRITD != 0) { AddCRITD(mod.CRITD * n); }
        if (mod.EVD != 0) { AddEVD(mod.EVD * n); }
        if (mod.ACC != 0) { AddACC(mod.ACC * n); }
        if (mod.ACT != 0) { AddACT(mod.ACT * n); }
        if (mod.turnPerRound != 0) { AddTurnPerRound(mod.turnPerRound * n); }
        if (mod.GHeal != 0) { AddGHeal(mod.GHeal * n); }
        if (mod.RHeal != 0) { AddRHeal(mod.RHeal * n); }
        foreach (StEResist res in mod.StEResists)
        {
            AddStERes(res, set);
        }
        foreach (StEApplyBonus bonus in mod.StEApplyBonus)
        {
            AddStEBonus(bonus, set);
        }
        if (mod.debuffChance != 0) { AddDebuffChance(mod.debuffChance * n); }
        if (mod.moveRes != 0) { AddMoveRes(mod.moveRes * n); }
        if (mod.debuffRes != 0) { AddDebuffRes(mod.debuffRes * n); }
    }
    public void AddMaxHP(int value_base, float value_mul, bool heal, int value_int = 0)
    {
        int oldMaxHP = charaStatus.maxHP;

        charaStatus.maxHP_base += value_base;
        charaStatus.maxHP_mul += value_mul;
        charaStatus.maxHP_int += value_int;
        charaStatus.maxHP = Mathf.Max(1, Mathf.RoundToInt((charaStatus.maxHP_base + charaStatus.maxHP_baseByLVL) * charaStatus.maxHP_mul / 100f) + charaStatus.maxHP_int);
        if (charaStatus.maxHP > oldMaxHP && heal)//差分を回復
        {
            charaStatus.HP += charaStatus.maxHP - oldMaxHP;
        }
        if (charaStatus.HP > charaStatus.maxHP) { charaStatus.HP = charaStatus.maxHP; }
        charaObj.SetHPandShieldBar();
    }
    public void AddMaxSAN(int value_base, float value_mul, bool heal)
    {
        int oldMaxSAN = charaStatus.maxSAN;

        charaStatus.maxSAN_base += value_base;
        charaStatus.maxSAN_mul += value_mul;
        charaStatus.maxSAN = Mathf.Max(1, Mathf.RoundToInt(charaStatus.maxSAN_base * charaStatus.maxSAN_mul / 100f));
        if (charaStatus.maxSAN > oldMaxSAN && heal)//差分を回復
        {
            charaStatus.SAN += charaStatus.maxSAN - oldMaxSAN;
        }
        if (charaStatus.SAN > charaStatus.maxSAN) { charaStatus.SAN = charaStatus.maxSAN; }
        charaObj.SetSANBar();
    }

    public void AddPROT(float value) { charaStatus.PROT += value; }
    public void AddATKINT(int value_base, float value_mul)
    {
        AddATK(value_base, value_mul);
        AddINT(value_base, value_mul);
    }

    public void AddATK(int value_base, float value_mul, int value_int = 0)
    {
        charaStatus.ATK_base += value_base;
        charaStatus.ATK_mul += value_mul;
        charaStatus.ATK_int += value_int;
        charaStatus.ATK = Mathf.Max(0, Mathf.RoundToInt((charaStatus.ATK_base + charaStatus.ATK_baseByLVL) * charaStatus.ATK_mul / 100f) + charaStatus.ATK_int);
    }

    public void AddINT(int value_base, float value_mul, int value_int = 0)
    {
        charaStatus.INT_base += value_base;
        charaStatus.INT_mul += value_mul;
        charaStatus.INT_int += value_int;
        charaStatus.INT = Mathf.Max(0, Mathf.RoundToInt((charaStatus.INT_base + charaStatus.INT_baseByLVL) * charaStatus.INT_mul / 100f) + charaStatus.INT_int);
    }
    public void AddCRITC(float value) { charaStatus.CRITC += value; }
    public void AddCRITD(float value) { charaStatus.CRITD += value; }
    public void AddExDMG_Mul(float value) { charaStatus.exDMG_mul += value; }
    public void AddEVD(float value) { charaStatus.EVD += value; }
    public void AddACC(float value) { charaStatus.ACC += value; }
    public void AddACT(int value) { charaStatus.ACT += value; }
    public void AddTurnPerRound(int value) { charaStatus.turnPerRound += value; }
    public void AddGHeal(float value) { charaStatus.GHeal += value; }
    public void AddRHeal(float value) { charaStatus.RHeal += value; }
    public void AddShield(int value)
    {
        charaStatus.shield += value;
        targetButton.SetDamageText($"{"shieldL".ToSpr()}{value}", Definer.colorRef.shield);
        infoText.AddLogText(string.Format("{0}はシールドを{1}{2}得た", charaStatus.charaName, "shield".ToSpr(), value.ToString().ColorStr(Definer.colorRef.shield)));
        soundManager.PlaySE(Definer.soundRef.shield);
        charaObj.SetHPandShieldBar();
    }
    public void RemoveShield(bool removeAll, int value)
    {
        int remove;
        if (charaStatus.shield > 0)
        {
            if (removeAll)
            {
                remove = charaStatus.shield;
                targetButton.SetDamageText($"{"shieldDMG".ToSpr()}シールドブレイク!", Definer.colorRef.shieldDecrease);
                charaStatus.shield = 0;
                infoText.AddLogText(string.Format("{0}は{1}シールドを全て失った", charaStatus.charaName, "shieldDMG".ToSpr()).ColorStr(Definer.colorRef.shieldDecrease));
            }
            else
            {
                remove = Mathf.Min(value, charaStatus.shield);
                targetButton.SetDamageText($"{"shieldDMG".ToSpr()}{remove}", Definer.colorRef.shieldDecrease);
                charaStatus.shield -= remove;
                infoText.AddLogText(string.Format("{0}はシールドを{1}{2}失った", charaStatus.charaName, "shieldDMG".ToSpr(), value.ToString().ColorStr(Definer.colorRef.shieldDecrease)));

            }
            charaObj.SetHPandShieldBar();

            OnDecreasedShield(remove);
        }
    }

    public void AddMarked(bool apply)
    {
        if (apply) { charaStatus.marked++; }
        else { charaStatus.marked--; }
    }
    public void AddHide(bool apply)
    {
        if (apply) { charaStatus.hide++; }
        else { charaStatus.hide--; }
    }
    public void AddFocused(bool apply)
    {
        if (apply) { charaStatus.focused++; }
        else { charaStatus.focused--; }
    }
    public void AddStun(bool apply)
    {
        if (apply) { charaStatus.stun++; }
        else { charaStatus.stun--; }
    }

    public void AddStERes(StEResist resist, bool set)
    {
        foreach (StEResist res in charaStatus.StEResists)
        {
            if (res.ResStE == resist.ResStE)
            {
                if (set) { res.value += resist.value; }
                else { res.value -= resist.value; }
                return;
            }
        }
        if (set) { charaStatus.StEResists.Add(resist); }
        else { infoText.AddErrorText("error"); }
    }
    public void AddStEBonus(StEApplyBonus bonus, bool set)
    {
        for (int i = 0; i < charaStatus.StEApplyBonus.Count; i++)
        {
            if (charaStatus.StEApplyBonus[i].applyStE == bonus.applyStE)
            {
                charaStatus.StEApplyBonus[i].AddBonus(bonus, set);
                return;
            }
        }
        if (set) { charaStatus.StEApplyBonus.Add(bonus); }
        else { infoText.AddErrorText("error"); }
    }
    public void AddDebuffChance(float value)
    {
        charaStatus.debuffChance += value;
    }
    public void AddMoveRes(float value)
    {
        charaStatus.moveRes += value;
    }
    public void AddDebuffRes(float value)
    {
        charaStatus.debuffRes += value;
    }
    public void AddActionMod(GameObject mod, bool set)
    {
        if (set) { charaStatus.actionMods.Add(mod); }
        else { charaStatus.actionMods.Remove(mod); }

    }
    public void Ability_AddRemain(int value, int index) { charaStatus.abilitiesStatus[index].AddRemain(value); }
    public void Ability_SetRemain(int value, int index) { charaStatus.abilitiesStatus[index].SetRemain(value); }
    public void Ability_StartCoolDown(int index) { charaStatus.abilitiesStatus[index].CoolDown_OnUse(); }
    public void Ability_AddCoolDown(int value, int index) { charaStatus.abilitiesStatus[index].AddCoolDown(value); }

    public void ChangePos(int moveTo)
    {
        infoText.AddLogText(string.Format("{0}はポジション{1}から{2}へ移動した", charaStatus.charaName, charaStatus.position.PosIntToStr(), moveTo.PosIntToStr()));
        //charaObj.StopMove(charaStatus.position);
        charaStatus.position = moveTo;
        targetButton = charactersManager.GetTargetButton(charaStatus.position);
        targetButton.SetCharacter(this);
        charactersManager.SortExistingCharacters();

        charaObj.MoveStart(charaStatus.position);
    }

    public void ResetPos()
    {
        charaStatus.position = charaStatus.position_battleStart;
        charaStatus.position_battleStart = -1;

        targetButton = charactersManager.GetTargetButton(charaStatus.position);
        targetButton.SetCharacter(this);

        charaObj.MoveStart(charaStatus.position);
    }

    public void AbilityRemain(ActionData.AbilityRemainControll remainControll)
    {
        foreach (Ability.AbilityStatus ability in charaStatus.abilitiesStatus)
        {
            bool f = false;
            if (ability.abilityData == remainControll.abilityData) { f = true; }
            if (remainControll.abilityData.upgradeAbility != null && ability.abilityData == remainControll.abilityData.upgradeAbility) { f = true; }
            if (f)
            {
                if (remainControll.set) { ability.remain = remainControll.value; }
                else
                {
                    int maxRemain = ability.maxRemain;
                    if (ability.maxRemain == 0) { maxRemain = 999; }
                    ability.remain = Mathf.Clamp(ability.remain + remainControll.value, 0, maxRemain);
                }
                infoText.AddLogText(string.Format("{0}の<{1}>の使用回数が{2}になった", charaStatus.charaName, ability.abilityName.ColorStr(ability.abilityType.ToColor()), ability.remain));
            }
        }
        //for(int i=0; i<charaStatus.abilitiesStatus.Length;i++)
        //{
        //    if (charaStatus.abilitiesStatus[i].abilityData == remainControll.abilityData)
        //    {
        //        infoText.AddDebugText("ok");
        //        if (remainControll.set) { charaStatus.abilitiesStatus[i].remain = remainControll.value; }
        //        else
        //        {
        //            charaStatus.abilitiesStatus[i].remain = Mathf.Clamp(charaStatus.abilitiesStatus[i].remain + remainControll.value, 0, charaStatus.abilitiesStatus[i].maxRemain);
        //        }
        //    }
        //}
    }

    public void GainEXP(int amount)
    {
        if (charaStatus.level < 10)
        {
            charaStatus.exp += amount;
            if (charaStatus.exp >= charaStatus.GetNextExp())
            {
                FindObjectOfType<LVLUpManager>().LVLUp(this);
            }
        }
    }
    public void LVLUp()
    {
        charaStatus.exp -= charaStatus.GetNextExp();
        StatusGrowth SG = (charaStatus.position.IsPlayerPos()) ? ExpeditionManager.inst.playerStatusGrowth : ExpeditionManager.inst.enemyStatusGrowth;
        int LVL = charaStatus.level;

        StatusMod_ByLVL prev = SG.GetStatusMod(LVL);
        StatusMod_ByLVL next = SG.GetStatusMod(LVL + 1);
        prev.SetStatus(charaStatus.maxHP_base, charaStatus.ATK_base, charaStatus.INT_base);
        next.SetStatus(charaStatus.maxHP_base, charaStatus.ATK_base, charaStatus.INT_base);
        next.DeltaMode(prev);

        charaStatus.maxHP_baseByLVL += next.maxHP;
        charaStatus.ATK_baseByLVL += next.ATK;
        charaStatus.INT_baseByLVL += next.INT;
        AddMaxHP(0, 0, true);
        AddATK(0, 0);
        AddINT(0, 0);

        AddACT(next.ACT);
        AddEVD(next.EVD);
        AddACC(next.ACC);

        //List<int> unlockEqSlotLVL = new List<int> { 4, 6, 8, 10 };
        if (GameManager.gameParams.unlockEqSlotLVL.Contains(LVL + 1))
        {
            charaStatus.equipmentSlots++;
        }

        charaStatus.level++;
        if (charaStatus.exp >= charaStatus.GetNextExp())
        {
            FindObjectOfType<LVLUpManager>().LVLUp(this);
        }
    }

    //ここまでアクションによって呼ばれる関数

    public void Debug_UnlockEqSlots()
    {
        charaStatus.equipmentSlots = 8;
    }

    public void UpgradeAbility(AbilityData upgrade)
    {
        for (int i = 0; i < charaStatus.abilitiesStatus.Length; i++)
        {
            if (charaStatus.abilitiesStatus[i].abilityData.upgradeAbility == upgrade)
            {
                Destroy(charaStatus.abilitiesStatus[i].instantiatedManager.gameObject);
                charaStatus.abilitiesStatus[i] = new Ability.AbilityStatus(upgrade, i);
                charaObj.SetAbilityManager(charaStatus.abilitiesStatus[i]);
            }
        }
    }

    public void UnlockAbility_All()
    {
        foreach (Ability.AbilityStatus abilityStatus in charaStatus.abilitiesStatus)
        {
            if (abilityStatus.locked)
            {
                abilityStatus.Unlock();
            }
        }
    }
    public void UpgradeAbility_All()
    {
        foreach (Ability.AbilityStatus abilityStatus in charaStatus.abilitiesStatus)
        {
            if (abilityStatus.abilityData.upgradeAbility != null)
            {
                UpgradeAbility(abilityStatus.abilityData.upgradeAbility);
            }
        }
    }

    public bool CheckAlive() { return !charaStatus.dead; }
    /// <summary>player側かどうかを返す</summary>
    public bool PlayerPos() { return charaStatus.position.IsPlayerPos(); }
    /// <summary>playerかどうかを返す(player側のキャラかではない)</summary>
    public bool IsPlayer() { return charaStatus.playable; }

    public List<Character> GetNeigbor(List<Vector2Int> neigbor)
    {
        return new List<Character>(charactersManager.GetCharactersWithPos(charaStatus.position.RelPosToAbs(neigbor)));
    }

    /// <summary>0:HP0 1:SAN0</summary>
    void Die(int cause, Character killer)
    {
        charaStatus.dead = true;

        Instantiate(Definer.VERef.die, charactersManager.GetCharacterWorldPos(charaStatus.position), Quaternion.identity);
        cameraManager.ShakeCamera(1);

        soundManager.PlaySE(Definer.soundRef.die1);
        soundManager.PlaySE(Definer.soundRef.die2);
        if (cause == 0)
        {
            targetButton.SetDamageText("死亡", Definer.colorRef.damage);
            infoText.AddLogText(string.Format("{0}は死亡した", charaStatus.charaName).ColorStr(Definer.colorRef.damage));
        }
        else if (cause == 1)
        {
            targetButton.SetDamageText("発狂", Definer.colorRef.damage);
            infoText.AddLogText(string.Format("{0}は発狂して死亡した", charaStatus.charaName).ColorStr(Definer.colorRef.damage));
        }

        charactersManager.RemoveExistingCharacter(this);
        battleManager.RemoveTurn(this);

        loot.DropItem_Loot(charaStatus.characterData.loot);
        foreach(var eq in charaStatus.equipments)
        {
            loot.AddItem(eq.data, 1);
        }

        OnDie(killer);
        battleManager.Trigger_OnSomeoneDied(this);

        targetButton.ResetCharacter();
        charaObj.HideCharacterObj();
        foreach (PassiveAbility pa in PA_StE)
        {
            pa.GetComponent<PA_StatusEffect>().DestroyIcon();
        }

        if (charaStatus.player)
        {
            tutorialManager.SetTutorial("playerDeath");
        }

        if (charaStatus.corpse != null)
        {
            if (charaStatus.position < 9) { charactersManager.SpawnPlayer(charaStatus.corpse, charaStatus.position, charaStatus.level); }
            else { charactersManager.SpawnEnemy(charaStatus.corpse, charaStatus.position, false, charaStatus.level); }
            battleManager.StartTutorial_Corpse();
        }
    }
    public void Retreat()
    {
        charaStatus.dead = true;
        charactersManager.RemoveExistingCharacter(this);
        battleManager.RemoveTurn(this);

        targetButton.ResetCharacter();
        charaObj.HideCharacterObj();
    }


    /// <summary>操作不可キャラがアビリティの選択をする際に呼ばれる
    /// 発動可能なアビリティのうち、優先度が最も高いアビリティの重みを考慮して選ぶ</summary>
    public virtual Ability.AbilityStatus SelectAbility_Random()
    {
        int priority = -999;
        foreach (Ability.AbilityStatus ability in charaStatus.abilitiesStatus)
        {
            if (!ability.excludeRandomPool && ability.instantiatedManager.CheckAvailable())
            {
                if (ability.priority >= priority) { priority = ability.priority; }
            }
        }
        //infoText.AddDebugText($"priority:{priority}");

        List<Ability.AbilityStatus> list = new List<Ability.AbilityStatus>();
        foreach (Ability.AbilityStatus ability in charaStatus.abilitiesStatus)
        {
            if (!ability.excludeRandomPool && ability.instantiatedManager.CheckAvailable() && ability.priority == priority) { list.Add(ability); }
        }
        return ChoiceAbilityWithWeight(list);
    }
    public Ability.AbilityStatus ChoiceAbilityWithWeight(List<Ability.AbilityStatus> abilitiesStatus)
    {
        if (abilitiesStatus.Count == 0) { infoText.AddErrorText("アビリティの選択肢がありません"); }
        List<int> weight = new List<int>();
        foreach (Ability.AbilityStatus ability in abilitiesStatus) { weight.Add(ability.selectWeight); }
        return abilitiesStatus[weight.ChoiceWithWeight()];
    }
    public List<Ability.AbilityStatus> GetAvailableAbilitiesStatus(bool onlyIncludeRandamPool)
    {
        List<Ability.AbilityStatus> availables = new List<Ability.AbilityStatus>();
        foreach (Ability.AbilityStatus ability in charaStatus.abilitiesStatus)
        {
            if (!(ability.excludeRandomPool && onlyIncludeRandamPool) && ability.instantiatedManager.CheckAvailable()) { availables.Add(ability); }
        }
        return availables;
    }
    public void OnBattleStart()
    {
        charaStatus.position_battleStart = charaStatus.position;
        for (int i = 0; i < charaStatus.abilitiesStatus.Length; i++)
        {
            //Ability_AddRemain(charaStatus.abilitiesStatus[i].remainOnBattleStart, i);
            charaStatus.abilitiesStatus[i].CoolDown_OnBattleStart();
            charaStatus.abilitiesStatus[i].SetRemain(charaStatus.abilitiesStatus[i].remainOnBattleStart);
        }
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnBattleStart(); }
        RemovePA_Execute();
    }
    public void OnRoundStart()
    {
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnRoundStart(); }
        RemovePA_Execute();
    }
    public void OnTurnOrderDecide()
    {
        charaStatus.exTurn = 0;//追加ターンリセット
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnTurnOrderDecide(); }
        RemovePA_Execute();
    }
    public void OnTurnStart(bool myTurn, int turnCount)
    {
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnTurnStart(myTurn, turnCount); }
        RemovePA_Execute();
    }
    public void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara)
    {
        if (charaStatus.shield > 0 && battleManager.GetCurrntTurnChara().CharaStatus().position.IsPlayerPos() != charaStatus.position.IsPlayerPos())
        {
            RemoveShield(false, Mathf.CeilToInt(charaStatus.shield * 0.25f));
        }
        if (myTurn)
        {
            charaStatus.spendTurn++;
        }
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnTurnEnd(myTurn, turnCount, deadTurnChara); }
        //targetButton.GetPositionManager().OnTurnEnd();
        RemovePA_Execute();
    }
    public void OnRoundEnd()
    {
        if (charaStatus.lifetime > 0)
        {

            DecreaseHP(Mathf.CeilToInt(1f * charaStatus.BaseHP() / charaStatus.lifetime));
            tutorialManager.SetTutorial("lifetime");
        }
        charaStatus.spendTurn = 0;
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnRoundEnd(); }
        RemovePA_Execute();
    }
    public void OnBattleEnd()
    {
        charaStatus.shield = 0;//シールド量リセット
        charaStatus.exTurn = 0;//追加ターンリセット
        continueTurn = false;
        charaObj.SetHPandShieldBar();
        charaStatus.spendTurn = 0;
        for (int i = 0; i < charaStatus.abilitiesStatus.Length; i++)
        {
            charaStatus.abilitiesStatus[i].CoolDown_OnBattleStart();
            charaStatus.abilitiesStatus[i].SetRemain(charaStatus.abilitiesStatus[i].remainOnBattleStart);
        }

        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnBattleEnd(); }
        RemovePA_Execute();
    }

    public Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities())
            {
                actionsStatus = passiveAbility.ModifyAction(statusRef, actionsStatus, forCalcDMG);
            }
            RemovePA_Execute();
        }

        return actionsStatus;
    }

    public Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef, bool forCalcDMG)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities())
            {
                statusRef = passiveAbility.ModifyAction_Targeted(statusRef, forCalcDMG);
            }
            RemovePA_Execute();
        }

        return statusRef;
    }

    public void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnActivateAbility(actionResultsList); }
            RemovePA_Execute();
        }
    }
    /// <summary>攻撃時、命中したかに関わらず誘発</summary>
    public void OnAttack(List<Action.OnAttackParams> onAttackParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnAttack(onAttackParamsList); }
            RemovePA_Execute();
        }
    }
    public void OnDecreasedHP(int value)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDecreasedHP(value); }
            RemovePA_Execute();
        }
    }
    public void OnDecreasedShield(int value)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDecreasedShield(value); }
            RemovePA_Execute();
        }
    }
    /// <summary>攻撃命中時</summary>
    public void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDamage(onDamageParamsList); }
            RemovePA_Execute();
        }
    }
    public void OnCRIT(int ID) { }
    public void Onkill(List<Action.OnKillParams> onKillParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnKill(onKillParamsList); }
            RemovePA_Execute();
        }
    }
    public void OnMiss(int ID) { }
    public void OnHeal(List<Action.OnHealParams> onHealParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnHeal(onHealParamsList); }
            RemovePA_Execute();
        }
    }
    //public virtual void OnApplyStE() { }
    //public virtual void OnRemoveStE() { }

    public virtual void OnCast(Eq_Magic cast)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnCast(cast); }
            RemovePA_Execute();
        }
    }

    public void BecomeAbilityTarget(Character actor)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.BecomeAbilityTarget(actor); }
            RemovePA_Execute();
        }
    }
    public void OnAttacked(Character attacker, bool evaded, bool missed)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnAttacked(attacker, evaded, missed); }
            RemovePA_Execute();
        }
    }
    public void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDamaged(onDamageParams); }
            RemovePA_Execute();
            battleManager.Trigger_OnSomeoneDamaged(onDamageParams);
        }
    }
    public void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnMoved(onMoveParams); }
            RemovePA_Execute();
            battleManager.Trigger_OnSomeoneMove(onMoveParams);
        }
    }
    public void OnFocus(List<Action.OnFocusParams> focusParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnFocus(focusParamsList); }
            RemovePA_Execute();
            battleManager.Trigger_OnSomeoneFocus(focusParamsList);
        }
    }
    public void OnDie(Character killer)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDie(killer); }
            RemovePA_Execute();
        }
    }
    public void OnHealed(Character healer, Action.OnHealParams onHealParams)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnHealed(healer, onHealParams); }
            RemovePA_Execute();
        }
    }
    public virtual void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnApplyStE(onApplyStEParamsList); }
            RemovePA_Execute();
        }
    }
    public void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnApplyedStE(onApplyStEParams); }
            RemovePA_Execute();
        }
    }
    public void OnSummon(List<Action.OnSummonParams> onSummonParamsList)
    {
        //infoText.AddDebugText(charaStatus.position.ToString());
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSummon(onSummonParamsList); }
            RemovePA_Execute();
        }
    }
    public void OnSummoned(Action.OnSummonParams onSummonParams)
    {
        //infoText.AddDebugText(charaStatus.position.ToString());
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSummoned(onSummonParams); }
            RemovePA_Execute();
        }
    }

    public void OnSomeoneDamaged(Action.OnDamageParams onDamageParams)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSomeoneDamaged(onDamageParams); }
            RemovePA_Execute();
        }
    }

    public void OnSomeoneMove(Action.OnMoveParams onMoveParams)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSomeoneMove(onMoveParams); }
            RemovePA_Execute();
        }
    }
    public void OnSomeoneFocus(List<Action.OnFocusParams> focusParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSomeoneFocus(focusParamsList); }
            RemovePA_Execute();
        }
    }
    public void OnSomeoneSummoned(Character summoner, List<Action.OnSummonParams> onSummonParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSomeoneSummoned(summoner, onSummonParamsList); }
            RemovePA_Execute();
        }
    }

    public void OnSomeoneDied(Character died)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSomeoneDied(died); }
            RemovePA_Execute();
        }
    }
    public void OnSomeoneApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSomeoneApplyedStE(onApplyStEParamsList); }
            RemovePA_Execute();
        }
    }
    //public virtual void OnRemoveedStE() { }

    public bool CheckAffricted()
    {
        foreach (PassiveAbility pa in PA_Per)
        {
            if (pa.GetComponent<PA_Personality>().GetPersonalityStatus().personalityType == PA_Personality.PersonalityStatus.PersonalityType.affricted)
            {
                return true;
            }
        }
        return false;
    }
    public PositionManager GetPositionManager() { return GetTargetButton().GetPositionManager(); }
}

