using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArithmeticAction : Action
{
    public ArithmeticOperator type;

    private int operand1;
    private int operand2;
    private bool operand1Exists = false;
    private bool operand2Exists = false;

    public override void Execute()
    {
        Variable variable = LevelManager.instance.variablePlayerHolding;
        if (variable == null)
        {
            statusDisplay.SetText("Not holding variable", true);
            return;
        }

        if (variable.type.Name != "Int32")
        {
            statusDisplay.SetText("Variable must be an integer", true);
            return;
        }

        if (!operand1Exists)
        {
            operand1 = (int)variable.GetValue();
            operand1Exists = true;
            return;
        }

        if (!operand2Exists)
        {
            operand2 = (int)variable.GetValue();
            operand2Exists = true;
            return;
        }

        LevelManager.instance.variablePlayerHolding.SetValue(Evaluate());
    }

    private int Evaluate()
    {
        switch (type)
        {
            case ArithmeticOperator.Plus:
                return operand1 + operand2;
            case ArithmeticOperator.Minus:
                return operand1 - operand2;
            case ArithmeticOperator.Multiply:
                return operand1 * operand2;
            case ArithmeticOperator.Divide:
                return operand1 / operand2;
            default:
                return int.MinValue;
        }
    }

    public override string MouseOverText()
    {
        if (!operand1Exists)
        {
            return GetTypeName(type);
        }

        if (!operand2Exists)
        {
            return operand1 + " " + GetTypeName(type);
        }

        return operand1 + " " + GetTypeName(type) + " " + operand2;
    }

    public static string GetTypeName(ArithmeticOperator type)
    {
        switch (type)
        {
            case ArithmeticOperator.Plus:
                return "+";
            case ArithmeticOperator.Minus:
                return "-";
            case ArithmeticOperator.Multiply:
                return "*";
            case ArithmeticOperator.Divide:
                return "/";
            default:
                return "?";
        }
    }
}

public enum ArithmeticOperator
{
    Plus,
    Minus,
    Multiply,
    Divide
}