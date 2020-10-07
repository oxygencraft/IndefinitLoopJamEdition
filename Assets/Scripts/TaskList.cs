using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskList : MonoBehaviour
{
    public Color currentActionColor;
    public float inactiveMiniCanvasGroupAlpha = 0.25f;
    public CanvasGroup miniCanvasGroup;
    public GameObject miniGameObject;
    public GameObject fullGameObject;
    public TMP_Text miniText;
    public TMP_Text fullText;

    private string originalFullText;
    private bool originalTextNotSet = true;

    public void SwitchToMini()
    {
        fullGameObject.SetActive(false);
        miniGameObject.SetActive(true);
    }

    public void SwitchToFull()
    {
        miniGameObject.SetActive(false);
        miniCanvasGroup.alpha = inactiveMiniCanvasGroupAlpha;
        fullGameObject.SetActive(true);
    }

    public void SetNewText(LevelManager.ActionsText text)
    {
        miniText.text = text.text;
        UpdateFullDisplayText(text);
    }

    public void HighlightEndOfLoop()
    {
        string[] lines = originalFullText.Split(new string[] { "\n" }, StringSplitOptions.None);
        LevelManager.ActionsText text = new LevelManager.ActionsText();
        text.line = lines.Length - 1;
        text.text = "}";
        UpdateFullDisplayText(text);
    }

    private void UpdateFullDisplayText(LevelManager.ActionsText actionsText)
    {
        // Unity refuses to call the Start function for
        // some reason so I have to do a workaround
        if (originalTextNotSet)
        {
            originalFullText = fullText.text;
            originalTextNotSet = false;
        }
        string textOriginal = originalFullText;
        string text = actionsText.text;
        int lineNum = actionsText.line;
        string[] lines = textOriginal.Split(new string[] { "\n" }, StringSplitOptions.None);
        string colorHex = ColorUtility.ToHtmlStringRGB(currentActionColor);

        string lineText = lines[lineNum];
        lines[lineNum] = lineText.Replace(text, "<color=#" + colorHex + ">" + text + "</color>");
        fullText.text = ReassembleString(lines);
    }

    private string ReassembleString(string[] lines)
    {
        string text = "";

        foreach (var line in lines)
        {
            text += line + "\n";
        }

        text.Remove(text.Length - 1);
        text.Remove(text.Length - 1);

        return text;
    }

    public void ActivateMini()
    {
        miniCanvasGroup.alpha = 1f;
    }

    public void DeactivateMini()
    {
        miniCanvasGroup.alpha = inactiveMiniCanvasGroupAlpha;
    }

    void Start()
    {
        originalFullText = fullText.text;
    }
}