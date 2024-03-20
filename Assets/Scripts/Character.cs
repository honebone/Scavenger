using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterStatus
    {
        public string fileName;
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
        public List<LootPanel.DropItem> dropItems;

        //public EquipmentType[] equipableTypes;
        //[Header("equipableTypesと要素数を合わせる")]
        //public Equipment[] equipments;

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


        public int instanceID;
        public int position;

        /// <summary>自身をかばっているキャラのinstanceID</summary>
        public int protectedBy;

        //public bool omenSet;
        //public Ability.AbilityStatus omen;

        public int HP;
        public int shield;

        public int SAN;

        public int exATK;

        public bool doesDropItem;

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
            s += string.Format("HP/maxHP：{0}/{1}\n", HP, maxHP);
            if (shield > 0) { s += string.Format("シールド：{0}\n", shield); }
            if (player) { s += string.Format("SAN/maxSAN：{0}/{1}\n\n", SAN, maxSAN); }
            else { s += "\n"; }

            s += string.Format("ATK：{0}\n", ATK);
            s += string.Format("CRIT：{0}％で{1}倍ダメージ\n\n", CRITC, CRITD);

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
                if (bonus.exValue != 0) { s += ValueToStr(string.Format("付与する{0}の値", StEName), bonus.exValue, ""); }
            }
            if (moveRes != 0) { s += string.Format("移動耐性{0}％\n", moveRes); }
            return s;
        }

        public void Init(CharacterData data,int ID)
        {
            fileName = data.fileName;
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
            dropItems = data.dropItems;

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

            CRITC = data.CRITC;
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

            instanceID = ID;
            equipments = new List<Definer.Item>();
        }
        public Vector2Int posIntToVector() { return new Vector2Int(position % 3, Mathf.FloorToInt(position / 3)); }
        public float GetStERes(GameObject StE)
        {
            foreach (StEResist res in StEResists)
            {
                if (res.ResStE == StE) { return res.value; }
            }
            return 0;
        }
        public StEApplyBonus GetStEApplyBonus(GameObject StE)
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
    }
    [System.Serializable]
    public struct CharaStatusMod
    {
        public float maxHP_mul;
        public float maxSAN_mul;

        public float ATK_mul;

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
        public string GetInfo()
        {
            string s = "";
            s += ValueToStr("maxHP", maxHP_mul, "％");
            s += ValueToStr("maxSAN", maxSAN_mul, "％");
            s += ValueToStr("ATK", ATK_mul, "％");
            s += ValueToStr("CRIT率", CRITC, "％(加算)");
            s += ValueToStr("CRITダメージ", CRITD, "倍(加算)");
            s += ValueToStr("EVD", EVD, "");
            s += ValueToStr("ACC", ACC, "");
            s += ValueToStr("ACT", ACT, "");
            s += ValueToStr("ラウンド毎ターン数", turnPerRound, "");
            s += ValueToStr("与える回復量", GHeal, "％");
            s += ValueToStr("受ける回復量", RHeal, "％");
            foreach(StEResist res in StEResists)
            {
                s += ValueToStr(string.Format("{0}耐性", res.ResStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName), res.value, "％");
            }
            foreach(StEApplyBonus bonus in StEApplyBonus)
            {
                string StEName = bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName;
                if (bonus.exChance != 0) { s += ValueToStr(string.Format("{0}付与確率", StEName), bonus.exChance, "％"); }
                if (bonus.exStack != 0) { s += ValueToStr(string.Format("{0}付与スタック数", StEName), bonus.exStack, ""); }
                if (bonus.exValue != 0) { s += ValueToStr(string.Format("付与する{0}の値", StEName), bonus.exValue, ""); }
            }
            
            return s;
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
    }
   protected CharacterStatus charaStatus;

    [SerializeField]
    protected Action.ActionStatus[] actionsStatusTest;
    public CharacterStatus GetCharacterStatus() { return charaStatus; }

    Character_Object charaObj;
    Character_TargetButton targetButton;
    public Character_Object GetCharacter_Object() { return charaObj; }
    public Character_TargetButton GetCharacter_TargetButton() { return targetButton; }

    //protected List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    protected List<PassiveAbility> PA_Pa = new List<PassiveAbility>();
    protected List<PassiveAbility> PA_StE = new List<PassiveAbility>();
    protected List<PassiveAbility> PA_Eq = new List<PassiveAbility>();
    List<PassiveAbility> deletePAs = new List<PassiveAbility>();

    ActionQueueManager actionQueue;
    BattleManager battleManager;
    Utility util;
    protected InfoText infoText;
    protected CharactersManager charactersManager;
    SoundManager soundManager;
    LootPanel loot;

    public void Init(CharacterStatus status, Character_Object obj, Character_TargetButton tb, bool dropItem)
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
        foreach (GameObject pa in charaStatus.passiveAbilities) { AddPA_Personality(pa); }

        targetButton.SetCharacter(this);

        actionQueue = FindObjectOfType<ActionQueueManager>();
        battleManager = FindObjectOfType<BattleManager>();
        util = FindObjectOfType<Utility>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager = FindObjectOfType<CharactersManager>();
        soundManager = FindObjectOfType<SoundManager>();
        loot = FindObjectOfType<LootPanel>();

        if (!charaStatus.playable)
        {
            charaObj.SetDamageText("出現", Definer.colorRef.abilityColors[5]);
            infoText.AddLogText(string.Format("{0}が現れた", charaStatus.charaName));
        }
        //TurnIconはラウンド開始時にセット
    }
    public List<PassiveAbility> GetPassiveAbilities()
    {
        List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
        passiveAbilities.AddRange(PA_Pa);
        passiveAbilities.AddRange(PA_StE);
        passiveAbilities.AddRange(PA_Eq);
        return passiveAbilities;
    }
    public void AddPA_Personality(GameObject paObj)
    {
        var p = Instantiate(paObj, transform);
        PA_Pa.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this,1);
    }
    public void EquipItem(Definer.Item item)
    {
        var p = Instantiate(item.data.manager, transform);
        PA_Eq.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this,2);
        item.createdManager = p;
        charaStatus.equipments.Add(item);
    }
    public void UnequipItem(Definer.Item remove)
    {
        charaStatus.equipments.Remove(remove);
        //PA_Eq.Remove(remove.createdManager.GetComponent<PassiveAbility>());
        //Destroy(remove.createdManager);
        remove.createdManager.GetComponent<PassiveAbility>().Disable();
        FindObjectOfType<Inventory>().AddItem(remove, 1, false);
    }
    public void RemovePA(PassiveAbility passiveAbility)
    {
        switch (passiveAbility.GetPAType())
        {
            case 0:
                PA_StE.Remove(passiveAbility);
                break;
            case 1:
                PA_Pa.Remove(passiveAbility);
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
                    PA_Pa.Remove(deletePA);
                    break;
                case 2:
                    PA_Eq.Remove(deletePA);
                    break;
            }
        }
        deletePAs.Clear();
    }
    public void ApplyStE(PA_StatusEffect.StatusEffectParams StEParams,StEApplyBonus applyBonus)
    {
        bool f = false;
        if (StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().merge)
        {
            PA_StatusEffect StE = StEParams.applyStE.GetComponent<PA_StatusEffect>();
            foreach (PassiveAbility pa in PA_StE)
            {
                if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.GetStatusEffectStatus().StEName)//同種のStEがすでにあるなら
                {
                    if(applyBonus == null) { pa.GetComponent<PA_StatusEffect>().AddStack(StEParams.stack); }
                    else { pa.GetComponent<PA_StatusEffect>().AddStack(StEParams.stack + applyBonus.exStack); }
                    charaObj.SetDamageText(string.Format("付与：{0}", StE.GetPAName()), Color.white);
                    infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, StE.GetPAName()));
                    soundManager.PlaySE(Definer.soundRef.ApplyStE[(int)StE.GetStatusEffectStatus().StEType]);
                    f = true;
                }
            }
        }
        if (!f)
        {
            var s = Instantiate(StEParams.applyStE, transform);
            PA_StE.Add(s.GetComponent<PassiveAbility>());
            //sort
            s.GetComponent<PA_StatusEffect>().Init(StEParams, charaObj.SetStEIcon().GetComponent<StEIcon>(),applyBonus);
            s.GetComponent<PassiveAbility>().Init(this, 0);
            charaObj.SetDamageText(string.Format("付与：{0}", s.GetComponent<PA_StatusEffect>().GetPAName()), Color.white);
            infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, s.GetComponent<PA_StatusEffect>().GetPAName()));
            soundManager.PlaySE(Definer.soundRef.ApplyStE[(int)StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEType]);
        }
    }
    public void RemoveStE(ActionData.RemoveStE removeStE)
    {
        PA_StatusEffect.StatusEffectStatus StE = removeStE.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in GetPassiveAbilities())
        {
            if (pa.GetPAType()==0&&pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.StEName)
            {
                if (removeStE.removeAll) { pa.GetComponent<PA_StatusEffect>().Disable(); }
            }
        }
        //メッセージ
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
        string info = charaStatus.GetInfo();
        info += "\n◇◇特性◇◇\n";
        foreach (PassiveAbility pa in PA_Pa)
        {
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(),pa.GetPAInfo()); 
        } 
        
        info += "\n◇◇状態異常◇◇\n";
        foreach (PassiveAbility pa in PA_StE)
        {
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(),pa.GetPAInfo()); 
        }

        if (charaStatus.player)
        {
            info += "\n◇◇装備品◇◇\n";
            foreach (PassiveAbility pa in PA_Eq)
            {
                info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo());
            }
        }
        
        info+="\n"+targetButton.GetPositionManager().GetPEInfo();
        infoText.SetCharaInfo(charaStatus.charaName, info, this);
        FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons(charaStatus.abilitiesStatus,this);
        //charaObj.SetSelectedIcon(true);
        targetButton.SetSelectedIcon(true);
    }
    public void ResetCharaSprite()
    {
        charaObj.SetCharaSprite(charaStatus.variableSprites[0]);
    }
    public void SetCharaSprite(GameObject sprite)
    {
        charaObj.SetCharaSprite(sprite);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionStatus"></param>
    /// <param name="setTargets">actionTargetに第3引数を代入するか</param>
    /// <param name="actionTargets">setTargetsがtrueの時、これを対象群として改めてactionstatusに代入する</param>
    public void Enqueue(Action.ActionStatus actionStatus,bool setTargets,List<Character> actionTargets)
    {
        actionStatus.actionOwner = this;
        if (setTargets) { actionStatus.actionTargets = actionTargets; }    
        actionQueue.Enqueue(actionStatus);
    }

    public void SetTurnIcon() { charaObj.SetTurnIcons(charaStatus.turnPerRound); }
    public void SetActionInvolvedIcon(bool owner) { targetButton.SetActionInvolvedIcon(owner); }

    //===================================================<<ターン処理>>========================================================
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
                }
                else { StartCoroutine(Test()); }
            }  
        }
        else
        {
            infoText.AddDebugText("死亡につきターンスキップ");
            battleManager.TurnEnd();
        }
         
    }
    IEnumerator Stun()
    {
        soundManager.PlaySE(Definer.soundRef.stun);
        charaObj.SetDamageText("行動不能!!", Definer.colorRef.failed_unavailable);
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
        BattleManager.selectedAbility.StartSelectTarget();
    }
    public void EndPhase()
    {
        if (CheckAlive())
        {

            OnTurnEnd();
            charaObj.SetTurnIcon_End();
            actionQueue.StartResolve(4);
        }
        else { battleManager.TurnEnd(); }
    }
    //public void EndMyTurn()
    //{
    //    battleManager.TurnEnd();
    //}


    //======================================================<<アクションによって呼ばれる関数>>=================================================
   public void Kill(Character attacker)
    {
        Die(0,attacker);
    }
    public void DecreaseHP(int value)
    {
        charaStatus.HP -= value;
        charaObj.SetHPandShieldBar();
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.decreaseHP);
        infoText.AddLogText(string.Format("{0}はHPを{1}失った", charaStatus.charaName, util.GetColoredText(Definer.colorRef.decreaseHP, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.damage);
        if (charaStatus.HP <= 0)
        {
            if (charaStatus.surviveFatalWounds)//瀕死で耐えるキャラは、HP減少によって死なない
            {
                charaStatus.HP = 0;
                charaObj.SetDamageText("瀕死!", Definer.colorRef.damage);
                infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, "瀕死")));
                soundManager.PlaySE(Definer.soundRef.dying);
                charaObj.SetHPandShieldBar();
            }
            else
            {
                Die(0,null);
            }
        }
    }
    public void Damage(int DMG,bool CRIT,int shieldDMG,bool canCounter,Character attacker)
    {
        charaStatus.shield -=shieldDMG;//シールド減少 
       

        if (CRIT)//テキストの表示
        {
            charaObj.SetDamageText("Critical!!", Definer.colorRef.CRIT);
            charaObj.SetDamageText(DMG.ToString(), Definer.colorRef.CRIT);
            infoText.AddLogText(string.Format("{0}\n{1}は{2}ダメージを受けた", util.GetColoredText(Definer.colorRef.CRIT, "Critical!!"), charaStatus.charaName, util.GetColoredText(Definer.colorRef.CRIT, DMG.ToString())));
            soundManager.PlaySE(Definer.soundRef.CRIT);
        }
        else
        {
            charaObj.SetDamageText(DMG.ToString(), Definer.colorRef.damage);
            infoText.AddLogText(string.Format("{0}は{1}ダメージを受けた", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, DMG.ToString())));
            soundManager.PlaySE(Definer.soundRef.damage);
        }

        if (charaStatus.HP == 0)//瀕死の状態で1以上のダメージを受けたら死亡する
        {
            if (DMG > 0)
            {
                if (charaStatus.surviveFatalWounds)
                {
                    Die(0,attacker);
                }
                else { print("瀕死で耐えるキャラ出ないのにHP0で生き続けています"); }
            }
            else//0ダメージの時
            {
                charaStatus.HP = 0;
                charaObj.SetDamageText("瀕死!", Definer.colorRef.damage);
                infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, "瀕死")));
                soundManager.PlaySE(Definer.soundRef.dying);

            }
        }
        else//瀕死でないなら
        {
            charaStatus.HP -= DMG;
            if (charaStatus.HP <= 0)
            {
                if (charaStatus.surviveFatalWounds)//瀕死で耐えるキャラは、瀕死でない状態で致命傷を受けても死なな
                {
                    charaStatus.HP = 0;
                    charaObj.SetDamageText("瀕死!", Definer.colorRef.damage);
                    infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, "瀕死")));
                    soundManager.PlaySE(Definer.soundRef.dying);
                }
                else
                {
                    Die(0, attacker);
                }
            }
        }

        charaObj.SetHPandShieldBar();//HPバーに反映
        if (CheckAlive())
        {
            OnDamaged(DMG, attacker);
            //カウンター
        }
    }
    public void Heal(int value,Character healer)
    {
        charaStatus.HP = Mathf.Min(charaStatus.HP + value, charaStatus.maxHP);
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.heal);
        infoText.AddLogText(string.Format("{0}はHPを{1}回復した", charaStatus.charaName, util.GetColoredText(Definer.colorRef.heal, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.heal);
        charaObj.SetHPandShieldBar();
    }
    public void SANHeal(int value)
    {
        charaStatus.SAN = Mathf.Min(charaStatus.SAN + value, charaStatus.maxSAN);
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.SANHeal);
        infoText.AddLogText(string.Format("{0}は正気度を{1}回復した", charaStatus.charaName, util.GetColoredText(Definer.colorRef.SANHeal, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.SANHeal);
        charaObj.SetSANBar();
    }
    public void SANDamage(int value)
    {
        if (charaStatus.player)
        {
            charaStatus.SAN -= value;
            charaObj.SetDamageText(value.ToString(), Definer.colorRef.SANDecrease);
            infoText.AddLogText(string.Format("{0}は正気度を{1}失った", charaStatus.charaName, util.GetColoredText(Definer.colorRef.SANDecrease, value.ToString())));
            soundManager.PlaySE(Definer.soundRef.SANDecrease);
            charaObj.SetSANBar();
            if (charaStatus.SAN <= 0) { Die(1,null); }
        }
    }

    //==================================================<<ステータス変更系>>===========================================================
    /// <summary>HP、SANの差分回復はしないことに注意 </summary>
    public void ModifyStatus(CharaStatusMod mod,bool set)
    {
        int n = 1;
        if (!set) { n = -1; }
        if (mod.maxHP_mul != 0) { AddMaxHP(0, mod.maxHP_mul * n, false); }
        if (mod.maxSAN_mul != 0) { AddMaxSAN(0, mod.maxSAN_mul * n, false); }
        if (mod.ATK_mul != 0) { AddATK(0, mod.ATK_mul * n); }
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
    public void AddATK(int value_base, float value_mul)
    {
        charaStatus.ATK_base += value_base;
        charaStatus.ATK_mul += value_mul;
        charaStatus.ATK = Mathf.Max(0, Mathf.RoundToInt(charaStatus.ATK_base * charaStatus.ATK_mul / 100f));
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
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.shield);
        infoText.AddLogText(string.Format("{0}はシールドを{1}得た", charaStatus.charaName, util.GetColoredText(Definer.colorRef.shield, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.shield);
        charaObj.SetHPandShieldBar();
    }
    public void RemoveShield(bool removeAll,int value)
    {
        if(charaStatus.shield > 0)
        {
            if (removeAll)
            {
                charaObj.SetDamageText("シールドブレイク!", Definer.colorRef.shieldDecrease);
                charaStatus.shield = 0;
                infoText.AddLogText(string.Format("{0}はシールドを全て失った", charaStatus.charaName).ColorStr(Definer.colorRef.shieldDecrease));
            }
            else
            {
                int remove = Mathf.Min(value, charaStatus.shield);
                charaObj.SetDamageText(remove.ToString(), Definer.colorRef.shieldDecrease);
                charaStatus.shield -= remove;
                infoText.AddLogText(string.Format("{0}はシールドを{1}失った", charaStatus.charaName, util.GetColoredText(Definer.colorRef.shieldDecrease, remove.ToString())));

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
        foreach (StEApplyBonus applyBonus in charaStatus.StEApplyBonus)
        {
            if (applyBonus.applyStE == bonus.applyStE)
            {
                int n = 1;
                if (!set) { n = -1; }
                applyBonus.exChance += bonus.exChance * n;
                applyBonus.exStack += bonus.exStack * n;
                applyBonus.exValue += bonus.exValue * n;
                return;
            }
        }
        if (set) { charaStatus.StEApplyBonus.Add(bonus); }
        else { infoText.AddErrorText("error"); }
    }

    public void AddActionMod(GameObject mod ,bool set)
    {
        if (set) { charaStatus.actionMods.Add(mod); }
        else { charaStatus.actionMods.Remove(mod); }
       
    }
    public void Ability_AddRemain(int value, int index) { charaStatus.abilitiesStatus[index].AddRemain(value); }
    public void Ability_SetRemain(int value, int index) { charaStatus.abilitiesStatus[index].SetRemain(value); }
    public void Ability_StartCoolDown(int index) { charaStatus.abilitiesStatus[index].StartCoolDown(); }
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
        foreach(Ability.AbilityStatus ability in charaStatus.abilitiesStatus)
        {
            if (ability.abilityData == remainControll.abilityData)
            {
                if (remainControll.set) { ability.remain = remainControll.value; }
                else
                {
                    int maxRemain = ability.maxRemain;
                    if (ability.maxRemain==0) { maxRemain = 999; }
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
    
    //ここまでアクションによって呼ばれる関数


    public bool CheckAlive() { return !charaStatus.dead; }
    /// <summary>0:HP0 1:SAN0</summary>
    void Die(int cause,Character killer)
    {
        charaStatus.dead = true;
        Instantiate(Definer.VERef.die, charactersManager.GetCharacterWorldPos(charaStatus.position), Quaternion.identity);
        soundManager.PlaySE(Definer.soundRef.die1);
        soundManager.PlaySE(Definer.soundRef.die2);
        if (cause == 0)
        {
            charaObj.SetDamageText("死亡", Definer.colorRef.damage);
            infoText.AddLogText(util.GetColoredText(Definer.colorRef.damage, string.Format("{0}は死亡した", charaStatus.charaName)));
        }
        else if (cause == 1)
        {
            charaObj.SetDamageText("発狂", Definer.colorRef.damage);
            infoText.AddLogText(util.GetColoredText(Definer.colorRef.damage, string.Format("{0}は発狂して死亡した", charaStatus.charaName)));
        }

        charactersManager.RemoveExistingCharacter(this);
        battleManager.RemoveTurn(this);

        foreach (LootPanel.DropItem dropItem in charaStatus.dropItems)
        {
            //float[] dropRate = FindObjectOfType<PartyManager>().GetPartyStatus().dropMaterialChance;
            //float[] dropRate = new float[] {60, 30, 10, 5, 1 };
            //int dropQuantity = 0;
            //for (int i = 0; i < dropItem.quantity; i++)
            //{
            //    if (dropRate[(int)dropItem.GetItemData().rarity].Probability())
            //    {
            //        dropQuantity++;
            //    }
            //}

            //if (dropQuantity > 0)
            //{
            //    Definer.Item item = new Definer.Item();
            //    item.Init(dropItem.GetItemData());
            //    loot.AddItem(item, dropQuantity);
            //}
            loot.DropItem_Enemy(dropItem);
        }

        OnDie(killer);

        targetButton.ResetCharacter();
        charaObj.HideCharacterObj();

        if (charaStatus.corpse != null)
        {
            if (charaStatus.position < 9) { charactersManager.SpawnPlayer(charaStatus.corpse, charaStatus.position); }
            else { charactersManager.SpawnEnemy(charaStatus.corpse, charaStatus.position, false); }
        }
    }
    public void Retreat()
    {
        charactersManager.RemoveExistingCharacter(this);
        battleManager.RemoveTurn(this);

        targetButton.ResetCharacter();
        charaObj.HideCharacterObj();
    }

    
    /// <summary>操作不可キャラがアビリティの選択をする際に呼ばれる
    /// ランダムプールから除外するアビリティ以外から重みを考慮して選ぶ</summary>
    public virtual Ability.AbilityStatus SelectAbility_Random()
    {
        List<Ability.AbilityStatus> list = new List<Ability.AbilityStatus>();
        foreach (Ability.AbilityStatus ability in charaStatus.abilitiesStatus)
        {
            if (!ability.excludeRandomPool&&ability.CheckAvailable(this,charactersManager)) { list.Add(ability); }
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
            if (!(ability.excludeRandomPool && onlyIncludeRandamPool) && ability.CheckAvailable(this, charactersManager)) { availables.Add(ability); }
        }
        return availables;
    }
    public void OnBattleStart()
    {
        for(int i = 0; i < charaStatus.abilitiesStatus.Length; i++)
        {
            //Ability_AddRemain(charaStatus.abilitiesStatus[i].remainOnBattleStart, i);
            charaStatus.abilitiesStatus[i].AddRemain(charaStatus.abilitiesStatus[i].remainOnBattleStart);
        }
        foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnBattleStart(); }
        RemovePA_Execute();
    }
    public void OnRoundStart()
    {
        if (!charaStatus.playable)
        { 
           //予兆設定
        }
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
    public void OnBattleEnd() { }


    public void OnActivateAbility()
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnActivateAbility(); }
            RemovePA_Execute();
        }
    }
    /// <summary>攻撃時、命中したかに関わらず誘発</summary>
    public void OnAttack(bool evaded,bool missed)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnAttack(evaded, missed); }
            RemovePA_Execute();
        }
    }
    /// <summary>攻撃命中時</summary>
    public void OnDamage(int DMG, Character target,Action.ActionStatus actionStatus)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDamage(DMG, target,actionStatus); }
            RemovePA_Execute();
        }
    }
    public void OnCRIT(int ID) { }
    public void OnKill(int ID) { }
    public void OnMiss(int ID) { }
    public void OnHeal(int healValue, int ID) { }
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
    public void OnDamaged(int DMG, Character attacker)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDamaged(DMG, attacker); }
            RemovePA_Execute();
        }
    }
    public void OnCRITed(int ID) { }
    public void OnDie(Character killer)
    {
        if (BattleManager.inBattle)
        {
            foreach (PassiveAbility passiveAbility in GetPassiveAbilities()) { passiveAbility.OnDie(killer); }
            RemovePA_Execute();
        }
    }
    public void OnHealed(int healedValue, int ID) { }
    //public virtual void OnApplyedStE() { }
    //public virtual void OnRemoveedStE() { }

    public PositionManager GetPositionManager() { return GetCharacter_TargetButton().GetPositionManager(); }
}

