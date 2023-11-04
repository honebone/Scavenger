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

        public bool player;
        public bool playable;
        /// <summary>0:idle 1:damaged </summary>
        public GameObject[] variableSprites; 
        public Sprite spriteForUI;
        public Ability.AbilityStatus[] abilitiesStatus;

        public List<GameObject> passiveAbilities;
        public List<GameObject> actionMods;

        public DropItem[] dropItems;

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

        //public DropItem[] dropItems;
        public string leftBehind;//死亡時に変身するキャラクター名

        public float stunRes;
        public float bleedRes;
        public float poisonRes;
        public float burnRes;

        public float moveRes;
        public float debuffRes;


        public int instanceID;
        public int position;

        /// <summary>自身をかばっているキャラのinstanceID</summary>
        public int protectedBy;

        public bool omenSet;
        public Ability.AbilityStatus omen;

        public int HP;
        public int shield;

        public int SAN;

        public int exATK;

        public bool doesDropItem;

        //以下バフ
        public int hide;

        //以下デバフ
        public int marked;
        public int focused;
        public int stun;
        public int bleed;//被ダメージ時この値分HP減少
        public int poison;//行動時この値分HP減少
        public int burn;//ターン終了時にこの値分HPが減少

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
            if (immovable) { s += "移動不可"; }
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

            if (omenSet) {s += string.Format("<{0}>を準備中\n", omen.abilityName.ColorStr(omen.abilityType.ToColor())); }
            return s;
        }

        public void Init(CharacterData data,int ID)
        {
            fileName = data.fileName;
            characterTags = new List<CharacterData.CharacterTag>(data.characterTags);
            charaName = data.charaName;
            //size = data.size;
            immovable = data.immovable;

            player = data.player;
            playable = data.playable;
            variableSprites = data.variableSprites;
            spriteForUI = data.spriteForUI;

            abilitiesStatus = new Ability.AbilityStatus[data.abilities.Length];
            for (int i = 0; i < abilitiesStatus.Length; i++) { abilitiesStatus[i].Init(data.abilities[i],i); }
            passiveAbilities = new List<GameObject>(data.passiveAbilities);

            actionMods = data.actionMods;

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

            leftBehind = data.leftBehind;

            debuffRes = data.debuffRes;

            stunRes = data.stunRes;
            bleedRes = data.bleedRes;
            poisonRes = data.poisonRes;
            burnRes = data.burnRes;

            moveRes = data.moveRes;
           

            instanceID = ID;
        }
        public Vector2Int posIntToVector() { return new Vector2Int(position % 3, Mathf.FloorToInt(position / 3)); }
    }
    CharacterStatus charaStatus;

    [SerializeField]
    protected Action.ActionStatus[] actionsStatusTest;
    public CharacterStatus GetCharacterStatus() { return charaStatus; }

    Character_Object charaObj;
    Character_TargetButton targetButton;
    public Character_Object GetCharacter_Object() { return charaObj; }
    public Character_TargetButton GetCharacter_TargetButton() { return targetButton; }

    protected List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    List<PassiveAbility> deletePAs = new List<PassiveAbility>();

    ActionQueueManager actionQueue;
    BattleManager battleManager;
    Utility util;
    InfoText infoText;
    protected CharactersManager charactersManager;
    SoundManager soundManager;
    LootPanel loot;

    public void Init(CharacterStatus status,Character_Object obj,Character_TargetButton tb,bool dropItem)
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
        foreach(GameObject pa in charaStatus.passiveAbilities) { AddPA_Personality(pa); }

        targetButton.SetCharacter(this);

        actionQueue = FindObjectOfType<ActionQueueManager>();
        battleManager = FindObjectOfType<BattleManager>();
        util = FindObjectOfType<Utility>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager=FindObjectOfType<CharactersManager>();
         soundManager=FindObjectOfType<SoundManager>();
        loot = FindObjectOfType<LootPanel>();

        charaObj.SetDamageText("出現", Definer.colorRef.abilityColors[5]);
        infoText.AddLogText(string.Format("{0}が現れた", charaStatus.charaName));
        //TurnIconはラウンド開始時にセット
    }
    public void AddPA_Personality(GameObject paObj)
    {
        var p = Instantiate(paObj, transform);
        passiveAbilities.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this,1);
    }
    public void RemovePA(PassiveAbility passiveAbility)
    {
        deletePAs.Add(passiveAbility);
    }
    void RemovePA_Execute()
    {
        foreach (PassiveAbility deletePA in deletePAs) { passiveAbilities.Remove(deletePA); }
        deletePAs.Clear();
    }
    public void ApplyStE(PA_StatusEffect.StatusEffectParams StEParams)
    {
        bool f = false;
        if (StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().merge)
        {
            PA_StatusEffect StE = StEParams.applyStE.GetComponent<PA_StatusEffect>();
            foreach (PassiveAbility pa in passiveAbilities)
            {
                if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.GetStatusEffectStatus().StEName)//同種のStEがすでにあるなら
                {
                    pa.GetComponent<PA_StatusEffect>().AddStack(StEParams.stack);
                    charaObj.SetDamageText(string.Format("付与：{0}", StE.GetPAName()), Color.white);
                    infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, StE.GetPAName()));
                    f = true;
                }
            }
        }
        if (!f)
        {
            var s = Instantiate(StEParams.applyStE, transform);
            passiveAbilities.Add(s.GetComponent<PassiveAbility>());
            //sort
            s.GetComponent<PA_StatusEffect>().Init(StEParams, charaObj.SetStEIcon().GetComponent<StEIcon>());
            s.GetComponent<PassiveAbility>().Init(this, 0);
            charaObj.SetDamageText(string.Format("付与：{0}", s.GetComponent<PA_StatusEffect>().GetPAName()), Color.white);
            infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, s.GetComponent<PA_StatusEffect>().GetPAName()));
        }
    }

    public bool CheckHasStE(GameObject StEObj)
    {
        PA_StatusEffect.StatusEffectStatus StE = StEObj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        foreach (PassiveAbility pa in passiveAbilities)
        {
            if (pa.GetPAType() == 0 && pa.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName == StE.StEName) { return true; }
        }
        return false;
    }

    public void DisplayInfo()
    {
        string info = charaStatus.GetInfo();
        //info += "◇◇特性◇◇\n";
        info += "\n";
        foreach(PassiveAbility pa in passiveAbilities)
        {
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(),pa.GetPAInfo());
            
        }
        infoText.SetCharaInfo(charaStatus.charaName, info, this);
        FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons(charaStatus.abilitiesStatus,this);
        charaObj.SetSelectedIcon(true);
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
        OnTurnStart();
        actionQueue.StartResolve(2);
    }
    public virtual void MainPhase()
    {
        if (CheckAlive())
        {
            //行動可能か～
            if (charaStatus.playable)
            {
                DisplayInfo();
                battleManager.SetSelectingAbility(true);
            }
            else { StartCoroutine(Test()); }
        }
        else
        {
            infoText.AddDebugText("死亡につきターンスキップ");
            EndMyTurn();
        }
         
    }
    IEnumerator Test()
    {
        yield return new WaitForSeconds(0.5f);

        battleManager.SetSelectedAbility(charaStatus.omen, this);//test　本来はラウンド開始時に決定する
        charaStatus.omenSet = false;
        charaStatus.omen = new Ability.AbilityStatus();
        BattleManager.selectedAbility.StartSelectTarget();
    }
    public void EndPhase()
    {
        if (CheckAlive())
        {

            OnTurnEnd();
            charaObj.SetTurnIcon_End();
            //Resolve開始
        }

        EndMyTurn();
    }
    public void EndMyTurn()
    {
        battleManager.TurnEnd();
    }


    //ここからアクションによって呼ばれる関数
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
                Die(0);
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
                    Die(0);
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
                    Die(0);
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
        charaStatus.SAN -= value;
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.SANDecrease);
        infoText.AddLogText(string.Format("{0}は正気度を{1}失った", charaStatus.charaName, util.GetColoredText(Definer.colorRef.SANDecrease, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.SANDecrease);
        charaObj.SetSANBar();
        if (charaStatus.SAN <= 0) { Die(1); }
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
    public void AddATK(int value_base, float value_mul)
    {
        charaStatus.ATK_base += value_base;
        charaStatus.ATK_mul += value_mul;
        charaStatus.ATK = Mathf.Max(0, Mathf.RoundToInt(charaStatus.ATK_base * charaStatus.ATK_mul / 100f));
    }
    public void AddEVD(float value)
    {
        charaStatus.EVD += value;
    }
    public void AddACT(int value)
    {
        charaStatus.ACT += value;
    }
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
    public void AddFocused(bool apply)
    {
        if (apply) { charaStatus.focused++; }
        else { charaStatus.focused--; }
    }

    public void Ability_AddRemain(int value, int index) { charaStatus.abilitiesStatus[index].AddRemain(value); }
    public void Ability_SetRemain(int value, int index) { charaStatus.abilitiesStatus[index].SetRemain(value); }
    public void Ability_StartCoolDown(int index) { charaStatus.abilitiesStatus[index].StartCoolDown(); }
    public void Ability_AddCoolDown(int value, int index) { charaStatus.abilitiesStatus[index].AddCoolDown(value); }

    public void ChangePos(int moveTo)
    {
        charaObj.StopMove(charaStatus.position);
        charaStatus.position = moveTo;
        targetButton = charactersManager.GetTargetButton(charaStatus.position);
        targetButton.SetCharacter(this);
        charactersManager.SortExistingCharacters();

        charaObj.MoveStart(charaStatus.position);
    }
    
    
    //ここまでアクションによって呼ばれる関数


    public bool CheckAlive() { return !charaStatus.dead; }
    /// <summary>0:HP0 1:SAN0</summary>
    void Die(int cause)
    {
        charaStatus.dead = true;
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

        foreach (DropItem dropItem in charaStatus.dropItems)
        {
            //float[] dropRate = FindObjectOfType<PartyManager>().GetPartyStatus().dropMaterialChance;
            float[] dropRate = new float[] {60, 30, 10, 5, 1 };
            int dropQuantity = 0;
            for (int i = 0; i < dropItem.quantity; i++)
            {
                if (dropRate[(int)dropItem.GetItemData().rarity].Probability())
                {
                    dropQuantity++;
                }
            }

            if (dropQuantity > 0)
            {
                Definer.Item item = new Definer.Item();
                item.Init(dropItem.GetItemData());
                loot.AddItem(item, dropQuantity);
            }
        }

        targetButton.ResetCharacter();
        charaObj.HideCharacterObj();
    }
    public void Retreat()
    {

    }

    public void SetOmen()
    {
        if (!charaStatus.playable && CheckAlive() && battleManager.CheckIfTurnRemain(this) && !charaStatus.omenSet)
        {
            charaStatus.omen = SelectAbility();
            charaStatus.omenSet = true;
            battleManager.SetOmenIcon(this, charaStatus.omen);
        }
    }
    /// <summary>操作不可キャラがアビリティの選択をする際に呼ばれる</summary>
    public virtual Ability.AbilityStatus SelectAbility()
    {
        return charaStatus.abilitiesStatus[Random.Range(0, charaStatus.abilitiesStatus.Length)];
    }
    public void OnBattleStart()
    {
        for(int i = 0; i < charaStatus.abilitiesStatus.Length; i++)
        {
            //Ability_AddRemain(charaStatus.abilitiesStatus[i].remainOnBattleStart, i);
            charaStatus.abilitiesStatus[i].AddRemain(charaStatus.abilitiesStatus[i].remainOnBattleStart);
        }
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnBattleStart(); }
        RemovePA_Execute();
    }
    public void OnRoundStart()
    {
        if (!charaStatus.playable)
        { 
           //予兆設定
        }
    }
    public void OnTurnStart()
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnTurnStart(); }
        RemovePA_Execute();
    }
    public void OnTurnEnd() { }
    public void OnRoundEnd() { }
    public void OnBattleEnd() { }


    public void OnActivateAbility()
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnActivateAbility(); }
        RemovePA_Execute();
    }
    /// <summary>攻撃時、命中したかに関わらず誘発</summary>
    public void OnAttack(bool evadeed,bool missed)
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnAttack(evadeed,missed); }
        RemovePA_Execute();
    }
    /// <summary>攻撃命中時</summary>
    public void OnDamage(int DMG, Character target)
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnDamage(DMG, target); }
        RemovePA_Execute();
    }
    public void OnCRIT(int ID) { }
    public void OnKill(int ID) { }
    public void OnMiss(int ID) { }
    public void OnHeal(int healValue, int ID) { }
    //public virtual void OnApplyStE() { }
    //public virtual void OnRemoveStE() { }

    public void BecomeAbilityTarget(Character actor)
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.BecomeAbilityTarget(actor); }
        RemovePA_Execute();
    }
    public void OnDamaged(int DMG, Character attacker)
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnDamaged(DMG, attacker); }
        RemovePA_Execute();
    }
    public void OnCRITed(int ID) { }
    public void OnEvade( int ID) { }
    public void OnHealed(int healedValue, int ID) { }
    //public virtual void OnApplyedStE() { }
    //public virtual void OnRemoveedStE() { }
}

