using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        /// <summary>勝敗に関係ないか</summary>
        public bool obstacle;
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

        public bool surviveFatalWounds;
        public int maxHP;
        public int maxHP_base;
        public float maxHP_mul;
        public int maxSAN;
        public int maxSAN_base;
        public float maxSAN_mul;

        public int ATK;
        public int ATK_base;
        public float ATK_mul;

        public int INT;
        public int INT_base;
        public float INT_mul;

        public float CRITC;
        public float CRITD;
        
        public float EVD;
        public float ACC;

        public int ACT;
        public int turnPerRound;

        public float GHeal;
        public float RHeal;

        public StatusGrowth statusGrowth;

        public List<StEResist> StEResists;
        public List<StEApplyBonus> StEApplyBonus;

        public float moveRes;
        public float debuffRes;

        ///<summary>今んとこ使ってないっす</summary>
        public int instanceID;
        public int position;

        /// <summary>自身をかばっているキャラのinstanceID</summary>
        public int protectedBy;

        //public bool omenSet;
        //public Ability.AbilityStatus omen;

        public int HP;
        public int shield;

        public float PROT;

        public int SAN;

        //public int exATK;

        public bool doesDropItem;

        public int equipmentSlots;
        public List<Definer.Item> equipments;

        //以下バフ
        public int hide;

        //以下デバフ
        public int marked;
        public int focused;
        public int stun;
        //public int bleed;//被ダメージ時この値分HP減少
        //public int poison;//行動時この値分HP減少
        //public int burn;//ターン終了時にこの値分HPが減少

        public bool dead;
        //ここに状態異常入れれるといいね 

        public string GetInfo()
        {
            string s = "";
            if (player && !playable) { s += "操作不可\n"; }
            bool f = false;
            s += "[";
            foreach(CharacterData.CharacterTag tag in characterTags)
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
            s += string.Format("HP/maxHP：{0}/{1}({2}％)\n", HP, maxHP, HP.GetPercent(maxHP).ToString("0.0"));
            if (shield > 0) { s += string.Format("シールド：{0}\n", shield); }
            if (PROT != 0) { s += ValueToStr("被ダメージ", PROT * -1, "％"); }
            if (player) { s += string.Format("SAN/maxSAN：{0}/{1}\n\n", SAN, maxSAN); }
            else { s += "\n"; }

            s += string.Format("ATK：{0}\n", ATK);
            s += string.Format("INT：{0}\n", INT);
            s += string.Format("CRIT：{0}％で{1}％ダメージ\n\n", CRITC, CRITD);

            s += string.Format("EVD：{0}\n", EVD);
            s += string.Format("ACC：{0}\n\n", ACC);

            s += string.Format("ACT：{0}\n", ACT);
            s += string.Format("ラウンド毎ターン数：{0}\n\n", turnPerRound);

            if (GHeal != 100) { s += string.Format("与える回復量：{0}％\n", GHeal); }
            if (RHeal != 100) { s += string.Format("受ける回復量：{0}％\n", RHeal); }

            foreach (StEResist res in StEResists)
            {
                if (res.value != 0)
                {
                    s += string.Format("{0}耐性{1}％\n", res.ResStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName, res.value);
                }
            }
            foreach(StEApplyBonus bonus in StEApplyBonus)
            {
                string StEName = bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName;
                if (bonus.exChance != 0) { s += ValueToStr(string.Format("{0}付与確率", StEName), bonus.exChance, "％"); }
                if (bonus.exStack != 0) { s += ValueToStr(string.Format("{0}付与スタック数", StEName), bonus.exStack, ""); }
                if (bonus.exValue != 0) { s += ValueToStr(string.Format("付与する{0}の効果量", StEName), bonus.exValue, ""); }
            }
            if (moveRes != 0) { s += string.Format("移動耐性{0}％\n", moveRes); }
            if (debuffRes != 0) { s += string.Format("デバフ耐性{0}％\n", debuffRes); }

            //foreach (GameObject actionMod in actionMods)
            //{
            //    s += actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo();
            //}
            return s;
        }

        public void Init(CharacterData data,int ID)
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
            obstacle = data.obstacle;
            variableSprites = data.variableSprites;
            spriteForUI = data.spriteForUI;

            abilitiesStatus = new Ability.AbilityStatus[data.abilities.Length];
            //FindObjectOfType<InfoText>().AddDebugText(abilitiesStatus[0].abilityName);
            for (int i = 0; i < abilitiesStatus.Length; i++) { abilitiesStatus[i]=new Ability.AbilityStatus(data.abilities[i],i); }
            passiveAbilities = new List<GameObject>(data.passiveAbilities);

            actionMods = new List<GameObject>(data.actionMods);

            corpse = data.corpse;

            level = 1;

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
            CRITD = data.CRITD;

            EVD = data.EVD;
            ACC = data.ACC;

            ACT = data.ACT;
            turnPerRound = data.turnPerRound;

            GHeal = data.GHeal;
            RHeal = data.RHeal;

            statusGrowth = data.statusGrowth;

            debuffRes = data.debuffRes;

            StEResists = new List<StEResist>(data.StEResists);
            StEApplyBonus = new List<StEApplyBonus>(data.StEApplyBonus);

            moveRes = data.moveRes;

            equipmentSlots = 4;

            instanceID = ID;
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
        public StEApplyBonus? GetStEApplyBonus(GameObject StE)
        {
            foreach (StEApplyBonus bonus in StEApplyBonus)
            {
                if (bonus.applyStE == StE) { return bonus; }
            }
            return null;
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
        public  float GetHPPercent() { return HP * 100f / maxHP; }
        public int GetNextExp() { return level; }
    }
    [System.Serializable]
    public struct CharaStatusMod
    {
        public float maxHP_mul;
        public float maxSAN_mul;

        public float PROT;

        public float ATK_mul;
        public float INT_mul;

        public float CRITC;
        public float CRITD;

        public float EVD;
        public float ACC;

        public int ACT;
        public int turnPerRound;

        public float GHeal;
        public float RHeal;

        public List<StEResist> StEResists;
        public List<StEApplyBonus> StEApplyBonus;

        public float moveRes;
        public float debuffRes;
        public string GetInfo()
        {
            string info = "";
            bool f = false;
            info += ValueToStr("maxHP", maxHP_mul, "％");
            info += ValueToStr("maxSAN", maxSAN_mul, "％");
            info += ValueToStr("被ダメージ", PROT * -1, "％");
            info += ValueToStr("ATK", ATK_mul, "％");
            info += ValueToStr("INT", INT_mul, "％");
            info += ValueToStr("CRIT率", CRITC, "％");
            info += ValueToStr("CRITダメージ", CRITD, "％");
            info += ValueToStr("EVD", EVD, "");
            info += ValueToStr("ACC", ACC, "");
            info += ValueToStr("ACT", ACT, "");
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
                if (bonus.exValue != 0) { info += ValueToStr(string.Format("付与する{0}の値", StEName), bonus.exValue, ""); }
            }
            info += ValueToStr("移動耐性", moveRes, "％");
            info += ValueToStr("デバフ耐性", debuffRes, "％");

            return info;

            string ValueToStr(string start, float value, string end)
            {
                if (value == 0) { return ""; }
                string s = f ? "\n" : "";
                f = true;
                s += start;
                if (value < 0) { s += value.ToString(); }
                else { s += "+" + value.ToString(); }
                s += end;
                return s;
            }
        }
       
    }
   protected CharacterStatus charaStatus;

    //[SerializeField]
    //protected Action.ActionStatus[] actionsStatusTest;
    public CharacterStatus GetCharacterStatus() { return charaStatus; }

    Character_Object charaObj;
    Character_TargetButton targetButton;
    public Character_Object GetCharacter_Object() { return charaObj; }
    public Character_TargetButton GetTargetButton() { return targetButton; }

    //protected List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    protected List<PassiveAbility> PA_StE = new List<PassiveAbility>();
    protected List<PassiveAbility> PA_Per = new List<PassiveAbility>();
    protected List<PassiveAbility> PA_Eq = new List<PassiveAbility>();
    List<PassiveAbility> deletePAs = new List<PassiveAbility>();

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

    public void Init(CharacterStatus status, Character_Object obj, Character_TargetButton tb, bool dropItem )
    {
        charaStatus = status;
        charaObj = obj;
        targetButton = tb;

        charaStatus.HP = charaStatus.maxHP;
        charaStatus.SAN = charaStatus.maxSAN;

        charaStatus.doesDropItem = dropItem;

        charaObj.SetCharaSprite(charaStatus.variableSprites[0]);
        if (!charaStatus.player) { charaObj.DisableSANBar(); }
        charaObj.SetHPandShieldBar();
        charaObj.SetSANBar();
        foreach (GameObject pa in charaStatus.passiveAbilities) { AddPA_Personality(pa,false); }

        targetButton.SetCharacter(this);

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

        if (charaStatus.position >= 9) { ModifyStatus(expeditionManager.GetEnemyStatusMod(), true, true); }

        if (!charaStatus.playable)
        {
            targetButton.SetDamageText("出現", Definer.colorRef.abilityColors[5]);
            infoText.AddLogText(string.Format("{0}が現れた", charaStatus.charaName));
        }
        //TurnIconはラウンド開始時にセット
    }
    public List<PA_Equipment> GetEquipments()
    {
        List<PA_Equipment> equipments = new List<PA_Equipment>();
        foreach(PassiveAbility PA in PA_Eq)
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
    public void AddPA_Personality(GameObject paObj,bool note)
    {
        var p = Instantiate(paObj, transform);
        PA_Per.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this, 1, infoText);
        if (note)
        {
            PA_Personality.PersonalityStatus personality = p.GetComponent<PA_Personality>().GetPersonalityStatus();
            targetButton.SetDamageText(string.Format("+特性：{0}", personality.personalityName), Definer.colorRef.personalityColors[(int)personality.personalityType]);
            infoText.AddLogText(string.Format("{0}は新たな特性{1}を得た", charaStatus.charaName,personality.GetName()));
        }
    }
    public void EquipItem(Definer.Item item)
    {
        var p = Instantiate(item.data.manager, transform);
        PA_Eq.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this,2,infoText);
        item.createdManager = p;
        charaStatus.equipments.Add(item);
    }
    public void UnequipItem(Definer.Item remove,bool returnToInventory=true)
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
        foreach(Definer.Item i in charaStatus.equipments)
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
                PA_StE.Remove(passiveAbility);
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
        foreach (PassiveAbility deletePA in deletePAs) {
            switch (deletePA.GetPAType())
            {
                case 0:
                    PA_StE.Remove(deletePA);
                    break;
                case 1:
                    PA_Per.Remove(deletePA);
                    break;
                case 2:
                    PA_Eq.Remove(deletePA);
                    break;
            }
        }
        deletePAs.Clear();
    }
    public void ApplyStE(PA_StatusEffect.StatusEffectParams StEParams,int finalStack,int finalValue)
    {
        bool alreadyExist = false;
        PA_StatusEffect StE = StEParams.applyStE.GetComponent<PA_StatusEffect>();
        PA_StatusEffect.StatusEffectStatus StEStatus = StE.GetStatusEffectStatus();
        if (StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().merge)
        {
            foreach (PassiveAbility pa in new List<PassiveAbility>(PA_StE))
            {
                if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.GetStatusEffectStatus().StEName)//同種のStEがすでにあるならそのスタックを増加
                {
                    pa.GetComponent<PA_StatusEffect>().AddStack(finalStack);

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
            PA_StE.Add(s.GetComponent<PassiveAbility>());
            //sort
            s.GetComponent<PA_StatusEffect>().Init(finalStack,finalValue, charaObj.SetStEIcon().GetComponent<StEIcon>());
            s.GetComponent<PassiveAbility>().Init(this, 0,infoText);
            if (StEStatus.refValue)
            {
                targetButton.SetDamageText(string.Format("+{0}{1}", StEStatus.StEName, finalValue), StEStatus.StEType.ToColor());
                infoText.AddLogText(string.Format("{0}は{1}{2}を付与された", charaStatus.charaName, s.GetComponent<PA_StatusEffect>().GetPAName(), finalValue.ToString().ColorStr(StEStatus.StEType.ToColor())));
            }
            else
            {
                targetButton.SetDamageText(string.Format("+{0}", StEStatus.StEName), StEStatus.StEType.ToColor());
                infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, s.GetComponent<PA_StatusEffect>().GetPAName()));
            }
            soundManager.PlaySE(Definer.soundRef.ApplyStE[(int)StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEType]);
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
                for(int i = 0; i < amount; i++)
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
        foreach(int stack in GetStEStacks(StEObj)) { sum += stack; }
        return sum;
    }

    public void AddStEStack(GameObject StEObj,int add)
    {
        PA_StatusEffect.StatusEffectStatus StE = StEObj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in PA_StE)
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
        infoText.SetCharaInfo(charaName, GetInfo());
        FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons(charaStatus.abilitiesStatus, this);
        //charaObj.SetSelectedIcon(true);
        targetButton.SetSelectedIcon(true);
    }
    public string GetInfo()
    {
        string info = charaStatus.GetInfo();
        info += "\n◇◇状態異常◇◇\n";
        foreach (PassiveAbility pa in PA_StE)
        {
            if (pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().refValue)
            {
                info += string.Format("<{0}>\n{1}\n", pa.GetComponent<PA_StatusEffect>().GetPANameWithValue(), pa.GetPAInfo());
            }
            else
            {
                info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo());
            }

        }

        info += "\n◇◇特性◇◇\n";
        foreach (PassiveAbility pa in PA_Per)
        {
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo());
        }

        if (charaStatus.player)
        {
            info += "\n◇◇装備品◇◇\n";
            foreach (PassiveAbility pa in PA_Eq)
            {
                info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo());
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
        if (charaStatus.position < 9) { v.transform.Rotate(new Vector3(0, 180, 0),Space.World); }//プレイヤーの時左右反転
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionStatus"></param>
    /// <param name="setTargets">actionTargetに第3引数を代入するか</param>
    /// <param name="actionTargets">setTargetsがtrueの時、これを対象群として改めてactionstatusに代入する</param>
    public void Enqueue(Action.ActionStatus actionStatus, bool setTargets, List<Character> actionTargets, int targetCount = 0, bool nullOwner = false)
    {
        if (!nullOwner) { actionStatus.actionOwner = this; }
        else { actionStatus.actionOwner = null; }
        if (setTargets) { actionStatus.actionTargets = actionTargets; }

        if ((actionStatus.actionTargets != null && actionStatus.actionTargets.Count > 0) || (actionStatus.actionTargetsInt != null && actionStatus.actionTargetsInt.Count > 0))
        {
            actionQueue.Enqueue(actionStatus, targetCount);
        }
    }

    public void SetTurnIcon() { charaObj.SetTurnIcons(charaStatus.turnPerRound); }
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
    public virtual void MainPhase()
    {
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
                    DisplayInfo();
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
                OnTurnEnd();
                charaObj.SetTurnIcon_End();
                actionQueue.StartResolve(4);
            }           
        }
        else { battleManager.TurnEnd(2); }
    }
  public void ContinueTurn() { continueTurn = true; }


    //======================================================<<アクションによって呼ばれる関数>>=================================================
   public void Kill(Character attacker)
    {
        Die(0,attacker);
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
        if (onDamageParams.shieldDMG > 0)
        {
            targetButton.SetDamageText($"{"shieldDMGL".ToSpr()}{onDamageParams.shieldDMG}", Definer.colorRef.shieldDecrease);
            infoText.AddLogText(string.Format("{0}はシールドを{1}{2}失った", charaStatus.charaName,"shieldDMG".ToSpr(), onDamageParams.shieldDMG.ToString().ColorStr(Definer.colorRef.shieldDecrease)));
        }

        SpawnVisualEffect(Definer.VERef.damage);
        if (onDamageParams.CRIT)//テキストの表示
        {
            targetButton.SetDamageText("Critical!!", Definer.colorRef.CRIT);
            infoText.AddLogText($"{"CRIT".ToSpr()}{"Critical!!".ColorStr(Definer.colorRef.CRIT)}");

            if (onDamageParams.ATK)
            {
                targetButton.SetDamageText($"{"CRITL".ToSpr()}{"ATKDMGL".ToSpr()}{onDamageParams.ATKDMG}", Definer.colorRef.CRIT);
                infoText.AddLogText($"{charaStatus.charaName}は{"ATKDMG".ToSpr()}{onDamageParams.ATKDMG.ToString().ColorStr(Definer.colorRef.CRIT)}ダメージを受けた");
            }
            if (onDamageParams.INT)
            {
                targetButton.SetDamageText($"{"CRITL".ToSpr()}{"INTDMGL".ToSpr()}{onDamageParams.INTDMG}", Definer.colorRef.CRIT);
                infoText.AddLogText($"{charaStatus.charaName}は{"INTDMG".ToSpr()}{onDamageParams.INTDMG.ToString().ColorStr(Definer.colorRef.CRIT)}ダメージを受けた");
            }
            soundManager.PlaySE(Definer.soundRef.CRIT);
        }
        else
        {
            if (onDamageParams.ATK)
            {
                targetButton.SetDamageText($"{"ATKDMGL".ToSpr()}{onDamageParams.ATKDMG}", Definer.colorRef.damage);
                infoText.AddLogText($"{charaStatus.charaName}は{"ATKDMG".ToSpr()}{onDamageParams.ATKDMG.ToString().ColorStr(Definer.colorRef.damage)}ダメージを受けた");
            }
            if (onDamageParams.INT)
            {
                targetButton.SetDamageText($"{"INTDMGL".ToSpr()}{onDamageParams.INTDMG}", Definer.colorRef.INTDamage);
                infoText.AddLogText($"{charaStatus.charaName}は{"INTDMG".ToSpr()}{onDamageParams.INTDMG.ToString().ColorStr(Definer.colorRef.INTDamage)}ダメージを受けた");
            }
            soundManager.PlaySE(Definer.soundRef.damage);
        }

        if (charaStatus.HP == 0)//瀕死の状態で1以上のダメージを受けたら死亡する
        {
            if (onDamageParams.totalDMG > 0)
            {
                if (charaStatus.surviveFatalWounds)
                {
                    Die(0, onDamageParams.owner);
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
                    Die(0, onDamageParams.owner);
                }
            }
        }

        charaObj.SetHPandShieldBar();//HPバーに反映
        if (CheckAlive() && onDamageParams.totalDMG > 0)
        {
            OnDamaged(onDamageParams);
            OnDecreasedHP(onDamageParams.totalDMG);
            //カウンター
        }
        return !CheckAlive();
    }
    public void Heal(int value,Character healer)
    {
        charaStatus.HP = Mathf.Min(charaStatus.HP + value, charaStatus.maxHP);
        targetButton.SetDamageText($"{"healL".ToSpr()}{value}", Definer.colorRef.heal);
        infoText.AddLogText(string.Format("{0}はHPを{1}{2}回復した", charaStatus.charaName,"heal".ToSpr(), value.ToString().ColorStr(Definer.colorRef.heal)));
        soundManager.PlaySE(Definer.soundRef.heal);
        charaObj.SetHPandShieldBar();
        SpawnVisualEffect(Definer.VERef.heal);
    }
    public void SANHeal(int value)
    {
        charaStatus.SAN = Mathf.Min(charaStatus.SAN + value, charaStatus.maxSAN);
        targetButton.SetDamageText($"{"SANHealL".ToSpr()}{value}", Definer.colorRef.SANHeal);
        infoText.AddLogText(string.Format("{0}は正気度を{1}{2}回復した", charaStatus.charaName,"SANHeal".ToSpr(), value.ToString().ColorStr(Definer.colorRef.SANHeal)));
        soundManager.PlaySE(Definer.soundRef.SANHeal);
        charaObj.SetSANBar();
    }
    public void SANDamage(int value)
    {
        if (charaStatus.player)
        {
            charaStatus.SAN -= value;
            targetButton.SetDamageText($"{"SANDMGL".ToSpr()}{value}", Definer.colorRef.SANDecrease);
            infoText.AddLogText(string.Format("{0}は正気度を{1}{2}失った", charaStatus.charaName, "SANDMG".ToSpr(),value.ToString().ColorStr(Definer.colorRef.SANDecrease)));
            soundManager.PlaySE(Definer.soundRef.SANDecrease);
            charaObj.SetSANBar();
            if (charaStatus.SAN <= 0) 
            { 
                if (!CheckAffricted())
                {
                    targetButton.SetDamageText("精神崩壊", Definer.colorRef.affricted);
                    infoText.AddLogText(string.Format("{0}は精神崩壊した!", charaStatus.charaName).ColorStr(Definer.colorRef.affricted));
                    AddPA_Personality(definer.GetAffrictionDataBase().Choice(), true);

                    charaStatus.SAN = charaStatus.maxSAN;
                    charaObj.Affrict();
                    charaObj.SetSANBar();
                }
                else
                {
                    Die(1, null);
                }
            }
        }
    }

    //==================================================<<ステータス変更系>>===========================================================
    public void ModifyStatus(CharaStatusMod mod, bool set, bool heal = false)
    {
        int n = 1;
        if (!set) { n = -1; }
        if (mod.maxHP_mul != 0) { AddMaxHP(0, mod.maxHP_mul * n, heal); }
        if (mod.maxSAN_mul != 0) { AddMaxSAN(0, mod.maxSAN_mul * n, heal); }
        if (mod.PROT != 0) { AddPROT(mod.PROT * n); }
        if (mod.ATK_mul != 0) { AddATK(0, mod.ATK_mul * n); }
        if (mod.INT_mul != 0) { AddINT(0, mod.INT_mul * n); }
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
        if (mod.moveRes != 0) { AddMoveRes(mod.moveRes); }
        if (mod.debuffRes != 0) { AddDebuffRes(mod.debuffRes); }
    }
    public void AddMaxHP(int value_base, float value_mul, bool heal)
    {
        int oldMaxHP = charaStatus.maxHP;

        charaStatus.maxHP_base += value_base;
        charaStatus.maxHP_mul += value_mul;
        charaStatus.maxHP = Mathf.Max(1, Mathf.RoundToInt(charaStatus.maxHP_base * charaStatus.maxHP_mul / 100f));
        if (charaStatus.maxHP > oldMaxHP&&heal)//差分を回復
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
        if (charaStatus.maxSAN > oldMaxSAN&&heal)//差分を回復
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

    public void AddATK(int value_base, float value_mul)
    {
        charaStatus.ATK_base += value_base;
        charaStatus.ATK_mul += value_mul;
        charaStatus.ATK = Mathf.Max(0, Mathf.RoundToInt(charaStatus.ATK_base * charaStatus.ATK_mul / 100f));
    }

    public void AddINT(int value_base, float value_mul)
    {
        charaStatus.INT_base += value_base;
        charaStatus.INT_mul += value_mul;
        charaStatus.INT = Mathf.Max(0, Mathf.RoundToInt(charaStatus.INT_base * charaStatus.INT_mul / 100f));
    }
    public void AddCRITC(float value) { charaStatus.CRITC += value; }
    public void AddCRITD(float value) { charaStatus.CRITD += value; }
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
        infoText.AddLogText(string.Format("{0}はシールドを{1}{2}得た", charaStatus.charaName,"shield".ToSpr(), value.ToString().ColorStr(Definer.colorRef.shield)));
        soundManager.PlaySE(Definer.soundRef.shield);
        charaObj.SetHPandShieldBar();
    }
    public void RemoveShield(bool removeAll,int value)
    {
        if(charaStatus.shield > 0)
        {
            if (removeAll)
            {
                targetButton.SetDamageText($"{"shieldDMG".ToSpr()}シールドブレイク!", Definer.colorRef.shieldDecrease);
                charaStatus.shield = 0;
                infoText.AddLogText(string.Format("{0}は{1}シールドを全て失った", charaStatus.charaName, "shieldDMG".ToSpr()).ColorStr(Definer.colorRef.shieldDecrease));
            }
            else
            {
                int remove = Mathf.Min(value, charaStatus.shield);
                targetButton.SetDamageText($"{"shieldDMG".ToSpr()}{remove}", Definer.colorRef.shieldDecrease);
                charaStatus.shield -= remove;
                infoText.AddLogText(string.Format("{0}はシールドを{1}{2}失った", charaStatus.charaName,"shieldDMG".ToSpr(), value.ToString().ColorStr(Definer.colorRef.shieldDecrease)));

            }
            charaObj.SetHPandShieldBar();
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
        foreach(StEResist res in charaStatus.StEResists)
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
    public void AddStEBonus(StEApplyBonus bonus,bool set)
    {
        for(int i=0;i< charaStatus.StEApplyBonus.Count;i++)
        {
            if (charaStatus.StEApplyBonus[i].applyStE == bonus.applyStE)
            {
                charaStatus.StEApplyBonus[i]=charaStatus.StEApplyBonus[i].AddBonus(bonus, true);
                return;
            }
        }
        if (set) { charaStatus.StEApplyBonus.Add(bonus); }
        else { infoText.AddErrorText("error"); }
    }
    public void AddMoveRes(float value)
    {
        charaStatus.moveRes += value;
    }
    public void AddDebuffRes(float value)
    {
        charaStatus.debuffRes += value;
    }
    public void AddActionMod(GameObject mod ,bool set)
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
        infoText.AddLogText(string.Format("{0}はポジション{1}から{2}へ移動した", charaStatus.charaName,charaStatus.position.PosIntToStr(),moveTo.PosIntToStr()));
        charaObj.StopMove(charaStatus.position);
        charaStatus.position = moveTo;
        targetButton = charactersManager.GetTargetButton(charaStatus.position);
        targetButton.SetCharacter(this);
        charactersManager.SortExistingCharacters();

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
        StatusGrowth SG = charaStatus.statusGrowth;
        int LVL = charaStatus.level;

        AddMaxHP(SG.CalcGrowth(LVL, SG.maxHP), 0, true);
        AddATK(SG.CalcGrowth(LVL, SG.ATK), 0);
        AddINT(SG.CalcGrowth(LVL, SG.INT), 0);
        AddCRITC(SG.CalcGrowth(LVL, SG.CRITC));
        AddCRITD(SG.CalcGrowth(LVL, SG.CRITD));
        AddACT(SG.CalcGrowth(LVL, SG.ACT));

        List<int> unlockEqSlotLVL = new List<int> { 4, 6, 8, 10 };
        if (unlockEqSlotLVL.Contains(LVL + 1))
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
    /// <summary>0:HP0 1:SAN0</summary>
    void Die(int cause,Character killer)
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

        OnDie(killer);

        targetButton.ResetCharacter();
        charaObj.HideCharacterObj();
        foreach(PassiveAbility pa in PA_StE)
        {
            pa.GetComponent<PA_StatusEffect>().DestroyIcon();
        }

        if (charaStatus.corpse != null)
        {
            if (charaStatus.position < 9) { charactersManager.SpawnPlayer(charaStatus.corpse, charaStatus.position); }
            else { charactersManager.SpawnEnemy(charaStatus.corpse, charaStatus.position, false); }
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
        for(int i = 0; i < charaStatus.abilitiesStatus.Length; i++)
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
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnTurnOrderDecide(); }
        RemovePA_Execute();
    }
    public void OnTurnStart(bool myTurn,int turnCount)
    {
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnTurnStart(myTurn, turnCount); }
        RemovePA_Execute();
    }
    public void OnTurnEnd()
    {
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnTurnEnd(); }
        targetButton.GetPositionManager().OnTurnEnd();
        RemovePA_Execute();
    }
    public void OnRoundEnd()
    {
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnRoundEnd(); }
        RemovePA_Execute();
    }
    public void OnBattleEnd()
    {
        charaStatus.shield = 0;//シールド量リセット
        charaObj.SetHPandShieldBar();
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
                actionsStatus = passiveAbility.ModifyAction(statusRef, actionsStatus,forCalcDMG);
            }
            RemovePA_Execute();
        }

        return actionsStatus;
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

    public void BecomeAbilityTarget(Character actor)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.BecomeAbilityTarget(actor); }
            RemovePA_Execute();
        }
    }
    public void OnAttacked(Character attacker,bool evaded,bool missed)
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

    public void OnSomeoneMove(Action.OnMoveParams onMoveParams)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnSomeoneMove(onMoveParams); }
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
        foreach(PassiveAbility pa in PA_Per)
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

