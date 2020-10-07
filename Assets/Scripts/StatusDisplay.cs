using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float inactiveAlpha = 0.25f;
    public Color normalColor;
    public Color warningColor;
    public bool activeCurrentText = false;
    public bool isActive = false;
    public TMP_Text statusText;
    public CanvasGroup uiCanvasGroup;

    private bool isPointerActivating = false;

    // Update is called once per frame
    void Update()
    {
        if (isActive || isPointerActivating)
        {
            uiCanvasGroup.alpha = 1f;
        }
        else
        {
            uiCanvasGroup.alpha = inactiveAlpha;
        }

        if (!activeCurrentText)
        {
            SetInactiveText();
        }
    }

    private void SetInactiveText()
    {
        statusText.color = normalColor;
        Variable variable = LevelManager.instance.variablePlayerHolding;
        if (variable == null || variable.IsNull())
        {
            statusText.text = "null";
            return;
        }
        object value = variable.GetValue();

        if (value is LogicalOperator || value is ComparisonOperator)
        {
            statusText.text = variable.name;
            return;
        }

        string typeName = GetShortPrimitiveTypeName(variable.type);
        bool displayable = value != null && typeName != value.GetType().Name;
        string text;

        if (displayable)
        {
            text = typeName + " " + variable.name + " = " + value + ";";
        }
        else if (value == null)
        {
            text = typeName + " " + variable.name + ";";
        }
        else
        {
            text = typeName + " " + variable.name + " = " + "[undisplayable]" + ";";
        }
        statusText.text = text;
    }

    public void SetText(string text, bool warning = false)
    {
        isActive = true;
        activeCurrentText = true;
        statusText.text = text;
        statusText.color = warning ? warningColor : normalColor;
    }

    public void ResetText()
    {
        isActive = false;
        activeCurrentText = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerActivating = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerActivating = false;
    }

    public static string GetShortPrimitiveTypeName(Type type)
    {
        switch (type.Name)
        {
            // Basic types that we'll be using for now
            // We probably won't need the rest for now
            // NOTE: If you are using this as a reference
            // for type strings, add the prefix "System."
            case "Boolean":
                return "bool";
            case "Char":
                return "char";
            case "Single":
                return "float";
            case "Int32":
                return "int";
            case "String":
                return "string";
        }

        return type.Name;
    }
}
