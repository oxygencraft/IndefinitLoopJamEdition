using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueAction : Action
{
    public string value;
    public string valueType;
    public Type type;

    void Awake()
    {
        base.Awake();
        type = Type.GetType(valueType);
    }

    public override void Execute()
    {
        Variable variable = LevelManager.instance.variablePlayerHolding;
        if (variable == null)
        {
            statusDisplay.SetText("Not holding variable", true);
            return;
        }

        if (variable.type.Name != type.Name)
        {
            statusDisplay.SetText("Object types do not match", true);
            return;
        }

        LevelManager.instance.variablePlayerHolding.SetValue(Convert.ChangeType(value, type));
        Complete();
    }

    public override string MouseOverText()
    {
        return value;
    }
}
