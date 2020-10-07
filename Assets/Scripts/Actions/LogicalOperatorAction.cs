using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalOperatorAction : Action
{
    public LogicalOperator type;

    public override void Execute()
    {
        if (LevelManager.instance.variablePlayerHolding != null)
        {
            statusDisplay.SetText("Already holding variable", true);
            return;
        }
        var variable = new Variable(GetTypeName(type), typeof(LogicalOperator));
        variable.SetValue(type);
        LevelManager.instance.variablePlayerHolding = variable;
        Complete();
    }

    public override string MouseOverText()
    {
        return GetTypeName(type);
    }

    public static string GetTypeName(LogicalOperator type)
    {
        switch (type)
        {
            case LogicalOperator.Not:
                return "!";
            case LogicalOperator.And:
                return "&&";
            case LogicalOperator.Or:
                return "||";
            default:
                return "??";
        }
    }
}
