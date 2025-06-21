using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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

    bool overrideSTName;
    bool STOutline;
    string spriteTextName;
    SpriteTextMode spriteTextMode;


    [MenuItem("Tools/Utility Window")]
    public static void ShowWindow()
    {
        GetWindow<UtilityWindow>("Utility Window");
    }

    private void OnGUI()
    {
        cp = (CommonParams)EditorGUILayout.ObjectField("Common Params", cp, typeof(CommonParams), false);

        GUI.enabled = cp != null;

        GUILayout.Label("Color Text", EditorStyles.boldLabel);
        color_input = EditorGUILayout.TextField("Text", color_input);
        color_col = EditorGUILayout.ColorField("Color", color_col);

        if (GUILayout.Button("Color Text"))
        {
            ColorText();
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
        spriteTextName = EditorGUILayout.TextField("è„èëÇ´Ç∑ÇÈñºèÃ", spriteTextName);
        spriteTextMode = (SpriteTextMode)EditorGUILayout.EnumPopup("mode", spriteTextMode);

        int columns = 4;
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
        obj = (GameObject)EditorGUILayout.ObjectField("ref obj", obj, typeof(GameObject), false);
        if (GUILayout.Button("StE Link"))
        {
            StELink();
        }



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

    void StELink()
    {
        string output = "error";
        PA_StatusEffect.StatusEffectStatus status = obj.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        Color c = cp.colorRef.statusEffectColors[(int)status.StEType];
        string sprite = "";
        if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff) sprite = "buff".ToSpr();
        else if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff) sprite = "debuff".ToSpr();
        else if (status.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus) sprite = "focus".ToSpr();

        output = $"{sprite}<link=S_{status.StEName}><u>{status.StEName}</u></link>".ColorStr(c);
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log($"Copied: {output}");
        ShowNotification(new GUIContent($"Copied: {output}"));
    }
}
