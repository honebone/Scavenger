using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using System;


public class UtilityWindow : EditorWindow
{
    public CommonParams cp;

    private string color_input = "";
    private Color color_col;

    string linkText;
    string linkKey;
    float evaluationValue;
    bool invertEvaluation;

    GameObject obj;
    CharacterData charaData;

    bool overrideSTName;
    bool STOutline;
    string spriteTextName;
    SpriteTextMode spriteTextMode;

    Vector2 scroll;
    List<GameObject> prefabList = new List<GameObject>();

    Dictionary<string, string> commonText = new Dictionary<string, string>()
    {
        {"物理","<sprite name=ATK><color=#C30000>物理</color>" },
        {"魔法","<sprite name=INT><color=#256CC8>魔法</color>" },
        {"回復","<sprite name=HP><color=#87FF79>回復</color>" },
        {"誘発能力","<link=U_誘発能力><u>誘発能力</u></link>" },
        {"クイックアビリティ","<color=#FFBF69><link=U_クイックアビリティ><u>クイックアビリティ</u></link></color>" },
        {"{X}","<color=#FFBF69><i>{X}</i></color>" },
        {"{効果量}","<color=#FFBF69><i>{効果量}</i></color>" },
        {"ATK補正","<sprite name=ATK><link=U_ATK(INT)補正><u><color=#C30000>ATK</color>補正</u></link>" },
        {"INT補正","<sprite name=INT><link=U_ATK(INT)補正><u><color=#256CC8>INT</color>補正</u></link>" },
        {"フォーカス","<color=#DD6300><sprite name=focus><link=S_フォーカス><u>フォーカス</u></link></color>" },
        {"[魔術]","<link=U_魔術><u>[魔術]</u></link>" },
        {"[ルーン]","<link=U_ルーン><u>[ルーン]</u></link>" },
        {"マーク","<color=#C900FF><sprite name=debuff><link=S_マーク><u>マーク</u></link></color>" },
        {"潜伏","<color=#FAED8A><sprite name=buff><link=S_潜伏><u>潜伏</u></link></color>" },
        {"星屑","<color=#3473CA><link=S_星屑><u>星屑</u></link></color>" },
        {"分配","<link=U_分配ダメージ><u>分配</u></link>" },
        {"分配ダメージ","<link=U_分配ダメージ><u>分配ダメージ</u></link>" },
        {"反響","<color=#96FFD6><link=U_反響><u>反響</u></link></color>" },
        {"回避","<sprite name=EVD><color=#55B075>回避</color>" },
        {"悪い特性","<color=#C900FF>悪い特性</color>" },
        {"良い特性","<color=#FAED8A>良い特性</color>" },
        {"レア特性","<color=#00BBFF>レア特性</color>" },
        {"ランダム特性","<link=U_ランダム特性><u>ランダム特性</u></link>" },

    };
    Dictionary<string, string> commonPhrases = new Dictionary<string, string>()
    {
        {"(マークを無視)","(<color=#C900FF><sprite name=debuff><link=S_マーク><u>マーク</u></link></color>を無視)" },
        {"(潜伏を無視)","(<color=#FAED8A><sprite name=buff><link=S_潜伏><u>潜伏</u></link></color>を無視)" },
        {"(潜伏・マークを無視)","(<color=#FAED8A><sprite name=buff><link=S_潜伏><u>潜伏</u></link></color>・<color=#C900FF><sprite name=debuff><link=S_マーク><u>マーク</u></link></color>を無視)" },
    };


    [MenuItem("Tools/Utility Window")]
    public static void ShowWindow()
    {
        GetWindow<UtilityWindow>("Utility Window");
    }

    private void OnGUI()
    {
        cp = (CommonParams)EditorGUILayout.ObjectField("Common Params", cp, typeof(CommonParams), false);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        GUI.enabled = cp != null;

        GUILayout.Label("Color Text", EditorStyles.boldLabel);
        color_input = EditorGUILayout.TextField("Text", color_input);
        color_col = EditorGUILayout.ColorField("Color", color_col);

        if (GUILayout.Button("Color Text"))
        {
            ColorText();
        }
        if (GUILayout.Button("Emphasize Text"))
        {
            EmphasizeText();
        }
        if (GUILayout.Button("Formula Text"))
        {
            FormulaText();
        }
        if (GUILayout.Button("Price Text"))
        {
            PriceText();
        }

        GUILayout.Space(10);
        GUILayout.Label("Create Link Text", EditorStyles.boldLabel);

        linkText = EditorGUILayout.TextField("Link Text", linkText);
        linkKey = EditorGUILayout.TextField("Link Key", linkKey);

        if (GUILayout.Button("Create Link Text"))
        {
            LinkText();
        }

        GUILayout.Space(10);
        GUILayout.Label("Evaluate Value", EditorStyles.boldLabel);

        evaluationValue = EditorGUILayout.FloatField("Evaluation Value", evaluationValue);
        invertEvaluation = EditorGUILayout.Toggle("Invert Evaluation", invertEvaluation);

        if (GUILayout.Button("Evaluate Value"))
        {
            EvaluateValue();
        }

        GUILayout.Space(10);
        GUILayout.Label("text sprite", EditorStyles.boldLabel);
        STOutline = EditorGUILayout.Toggle("outline", STOutline);
        overrideSTName = EditorGUILayout.Toggle("override name", overrideSTName);
        spriteTextName = EditorGUILayout.TextField("上書きする名称", spriteTextName);
        spriteTextMode = (SpriteTextMode)EditorGUILayout.EnumPopup("mode", spriteTextMode);

        int columns = 7;
        if (cp != null)
        {
            for (int i = 0; i < cp.textSpriteParamsList.Count; i++)
            {
                if (i % columns == 0)
                {
                    GUILayout.BeginHorizontal();
                }

                if (GUILayout.Button(cp.textSpriteParamsList[i].key, GUILayout.Height(30), GUILayout.Width(150)))
                {
                    CopyTextSprite(cp.textSpriteParamsList[i].key);
                }

                if (i % columns == columns - 1 || i == cp.textSpriteParamsList.Count - 1)
                {
                    GUILayout.EndHorizontal();
                }
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("common texts", EditorStyles.boldLabel);

        int columns_ct = 7;
        if (cp != null)
        {
            int i = 0;
            foreach(var key in commonText.Keys)
            {
                if (i % columns_ct == 0)
                {
                    GUILayout.BeginHorizontal();
                }

                if (GUILayout.Button(key, GUILayout.Height(30), GUILayout.Width(150)))
                {
                    CopyCommonText(key);
                }

                if (i % columns_ct == columns_ct - 1 || i == commonText.Count - 1)
                {
                    GUILayout.EndHorizontal();
                }
                i++;
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("rarity texts", EditorStyles.boldLabel);

        int columns_rarity = 6;
        if (cp != null)
        {
            int i = 0;
            foreach (ItemData.Rarity key in Enum.GetValues(typeof(ItemData.Rarity)))
            {
                if (i % columns_rarity == 0)
                {
                    GUILayout.BeginHorizontal();
                }

                if (GUILayout.Button(key.ToString(), GUILayout.Height(30), GUILayout.Width(150)))
                {
                    CopyRarityText(key);
                }

                if (i % columns_rarity == columns_rarity - 1 || i == Enum.GetNames(typeof(ItemData.Rarity)).Count() - 1)
                {
                    GUILayout.EndHorizontal();
                }
                i++;
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("common phrases", EditorStyles.boldLabel);
        int columns_cp = 3;
        if (cp != null)
        {
            int i = 0;
            foreach (var key in commonPhrases.Keys)
            {
                if (i % columns_cp == 0)
                {
                    GUILayout.BeginHorizontal();
                }

                if (GUILayout.Button(key, GUILayout.Height(30), GUILayout.Width(300)))
                {
                    CopyCommonPhrase(key);
                }

                if (i % columns_cp == columns_cp - 1 || i == commonPhrases.Count - 1)
                {
                    GUILayout.EndHorizontal();
                }
                i++;
            }
        }

        GUILayout.Space(10);
        obj = (GameObject)EditorGUILayout.ObjectField("ref obj", obj, typeof(GameObject), false);
        if (GUILayout.Button("StE Link"))
        {
            StELink();
        }
        if (GUILayout.Button("PE Link"))
        {
            PELink();
        }

        GUILayout.Space(10);
        charaData = (CharacterData)EditorGUILayout.ObjectField("chara data", charaData, typeof(CharacterData), false);
        if (GUILayout.Button("Chara Link"))
        {
            CharaLink();
        }

        EditorGUILayout.LabelField("編集するプレハブ一覧", EditorStyles.boldLabel);

        if (GUILayout.Button("プレハブを追加"))
            prefabList.Add(null);


        for (int i = 0; i < prefabList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            prefabList[i] = (GameObject)EditorGUILayout.ObjectField(prefabList[i], typeof(GameObject), false);
            if (GUILayout.Button("×", GUILayout.Width(20)))
                prefabList.RemoveAt(i);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("変数を一括変更"))
        {
            foreach (var prefab in prefabList)
            {
                if (prefab == null) continue;

                string path = AssetDatabase.GetAssetPath(prefab);
                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);

                // 任意のスクリプトと変数を変更
                var eq = prefabRoot.GetComponent<PA_Equipment>();
                if (eq == null)
                {
                    Debug.Log(path);
                }
                else
                {
                    //eq.statMod = eq.GetEquipmentStatus().statusMod;
                    //eq.AMods = new List<GameObject>(eq.GetEquipmentStatus().actionMods);
                }
                
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        EditorGUILayout.EndScrollView();

        GUI.enabled = true;
    }

    private void ColorText()
    {
        if (!string.IsNullOrEmpty(color_input))
        {
            string output = color_input.ColorStr(color_col);
            EditorGUIUtility.systemCopyBuffer = output;
            Debug.Log($"Copied: {output}");
            ShowNotification(new GUIContent($"Copied: {output}"));
        }
    }
    private void EmphasizeText()
    {
        if (!string.IsNullOrEmpty(color_input))
        {
            string output = color_input.ColorStr(cp.colorRef.emphasize);
            EditorGUIUtility.systemCopyBuffer = output;
            Debug.Log($"Copied: {output}");
            ShowNotification(new GUIContent($"Copied: {output}"));
        }
    }

    private void FormulaText()
    {
        if (!string.IsNullOrEmpty(color_input))
        {
            string output = $"<i>{{{color_input}}}</i>".ColorStr(cp.colorRef.emphasize);
            EditorGUIUtility.systemCopyBuffer = output;
            Debug.Log($"Copied: {output}");
            ShowNotification(new GUIContent($"Copied: {output}"));
        }
    }
    private void PriceText()
    {
        if (!string.IsNullOrEmpty(color_input))
        {
            string output = $"<sprite name=coin><color=#D83ECE>{color_input}</color>";
            EditorGUIUtility.systemCopyBuffer = output;
            Debug.Log($"Copied: {output}");
            ShowNotification(new GUIContent($"Copied: {output}"));
        }
    }

    void LinkText()
    {
        string output = linkText.ToLinkKey(linkKey);
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }

    void EvaluateValue()
    {
        string output = evaluationValue.Evaluate(invertEvaluation);
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }

    private void CopyTextSprite(string key)
    {
        string output = "error";

        foreach (var s in cp.textSpriteParamsList)
        {
            if (s.key == key)
            {
                if (overrideSTName) output = s.GetTextSprite(spriteTextMode, STOutline, spriteTextName);
                else output = s.GetTextSprite(spriteTextMode, STOutline, null);
            }
        }
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }

    void CopyCommonText(string key)
    {
        string output = commonText[key];
       
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }
    void CopyRarityText(ItemData.Rarity rarity)
    {
        List<string> list= new List<string>() { "コモン","アンコモン","レア", "エピック", "レジェンダリー", "狂気" };
        string output = list[(int)rarity].ColorStr(cp.colorRef.rarityColors[(int)rarity]);

        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }
    void CopyCommonPhrase(string key)
    {
        string output = commonPhrases[key];

        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }

    void StELink()
    {
        string output = "error";
        PA_StatusEffect.StatusEffectStatus status = obj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        Color c = cp.StEColor(status);
        string sprite = "";
        if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff) sprite = "buff".ToSpr();
        else if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff) sprite = "debuff".ToSpr();
        else if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus) sprite = "focus".ToSpr();

        output = $"{sprite}<link=S_{status.StEName}><u>{status.StEName}</u></link>".ColorStr(c);
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }
    void PELink()
    {
        string output = "error";
        PositionEffect.PositionEffectStatus status = obj.GetComponent<PositionEffect>().GetPositionEffectStatus();
        Color c = cp.colorRef.positionEffectColors[(int)status.PEType];
        string sprite = "";
        if (status.PEType == PositionEffect.PositionEffectStatus.PositionEffectType.buff) sprite = "buff".ToSpr();
        else if (status.PEType == PositionEffect.PositionEffectStatus.PositionEffectType.debuff) sprite = "debuff".ToSpr();

        output = $"{sprite}<link=P_{status.PEName}><u>{status.PEName}</u></link>".ColorStr(c);
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }

    void CharaLink()
    {
        string output = "error";

        //output = $"<link=C_{charaData.fileName}><u>{charaData.charaName}</u></link>";
        output = charaData.ToLinkKey();
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }
}
