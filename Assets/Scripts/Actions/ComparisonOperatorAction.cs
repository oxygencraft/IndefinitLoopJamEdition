using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComparisonOperatorAction : Action
{
    public ComparisonOperator type;

    public override void Execute()
    {
        if (LevelManager.instance.variablePlayerHolding != null)
        {
            statusDisplay.SetText("Already holding variable", true);
            return;
        }
        var variable = new Variable(GetTypeName(type), typeof(ComparisonOperator));
        variable.SetValue(type);
        LevelManager.instance.variablePlayerHolding = variable;
        Complete();
    }

    public override string MouseOverText()
    {
        return GetTypeName(type);
    }

    public static string GetTypeName(ComparisonOperator type)
    {
        switch (type)
        {
            case ComparisonOperator.Equal:
                return "==";
            case ComparisonOperator.NotEqual:
                return "!=";
            case ComparisonOperator.LessThan:
                return "<";
            case ComparisonOperator.LessThanEqual:
                return "<=";
            case ComparisonOperator.GreaterThan:
                return ">";
            case ComparisonOperator.GreaterThanEqual:
                return ">=";
            default:
                return "??";
        }
    }
}
