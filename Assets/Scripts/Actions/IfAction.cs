using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfAction : Action
{
    private List<Operand> operands = new List<Operand>();
    private List<bool> evaluatedOperands = new List<bool>();
    private List<LogicalOperator> logicalOperators = new List<LogicalOperator>();
    private object firstObject; // Check if it is a bool to allow bool if statement
    private bool secondObject; // If the above is not a bool, check if we provided a second object
    private bool isNextOperandValueNot = false; // Set to true if player inputted a NOT operator and apply the NOT to the next operand value
    private Operand currentOperand = null;

    public override void Execute()
    {
        if (LevelManager.instance.variablePlayerHolding != null)
        {
            Variable variable = LevelManager.instance.variablePlayerHolding;
            if (isNextOperandValueNot)
            {
                if (variable.GetValue() is bool b)
                {
                    if (currentOperand.value1 == null)
                    {
                        currentOperand.value1 = b;
                        currentOperand.notValue1 = true;
                    }
                    else
                    {
                        currentOperand.value2 = b;
                        currentOperand.notValue2 = true;
                        secondObject = true;
                    }
                }
                else
                {
                    statusDisplay.SetText("NOTed values must be booleans", true);
                    return;
                }

                isNextOperandValueNot = false;
            }

            if (variable.GetValue() is LogicalOperator logicalOperator)
            {
                if (logicalOperator == LogicalOperator.Not)
                {
                    isNextOperandValueNot = true;
                }

                if (operands.Count == 0 || currentOperand != null)
                {
                    statusDisplay.SetText("Operand incomplete", true);
                    return;
                }

                logicalOperators.Add(logicalOperator);
            }

            if (currentOperand != null && variable.GetValue() is ComparisonOperator compOperator)
            {
                if (!IsValidCompOperator(compOperator))
                {
                    return;
                }
                currentOperand.compOperator = compOperator;
            }

            if (currentOperand != null)
            {
                object value = variable.GetValue();
                if (!(value is ComparisonOperator || value is LogicalOperator))
                {
                    if (!IsValidForCompOperator(value))
                    {
                        return;
                    }
                    currentOperand.value2 = value;
                    secondObject = true;
                }
            }

            if (currentOperand == null)
            {
                object value = variable.GetValue();
                if (!(value is ComparisonOperator || value is LogicalOperator))
                {
                    currentOperand = new Operand(value);
                    firstObject = value;
                }
            }

            if (currentOperand != null && currentOperand.IsValid())
            {
                operands.Add(currentOperand);
                currentOperand = null;
            }

            LevelManager.instance.variablePlayerHolding = null;
            return;
        }

        // Executed if action without carrying any variables
        if (!(firstObject is bool) && !secondObject || operands.Count == 0 || !LogicalOperatorCheck() || currentOperand != null)
        {
            // TODO: Warn player that the if action is complete
            statusDisplay.SetText("If action incomplete", true);
            return;
        }
        Evaluate();
        Complete();
    }

    public override string MouseOverText()
    {
        string condition = "";
        var operators = logicalOperators;
        foreach (var operand in operands)
        {
            condition += operand;
            if (operators.Count != 0)
            {
                condition += " " + LogicalOperatorAction.GetTypeName(operators[0]) + " ";
                operators.RemoveAt(0);
            }
        }

        return "if (" + condition + ")";
    }

    private bool IsValidForCompOperator(object obj)
    {
        if (currentOperand.compOperator == ComparisonOperator.GreaterThan
            || currentOperand.compOperator == ComparisonOperator.GreaterThanEqual
            || currentOperand.compOperator == ComparisonOperator.LessThan
            || currentOperand.compOperator == ComparisonOperator.LessThanEqual)
        {
            if (!(obj is int))
            {
                statusDisplay.SetText("Size comparison operators require integers", true);
                return false;
            }
        }
        return true;
    }

    private bool IsValidCompOperator(ComparisonOperator compOperator)
    {
        if (compOperator == ComparisonOperator.GreaterThan
            || compOperator == ComparisonOperator.GreaterThanEqual
            || compOperator == ComparisonOperator.LessThan
            || compOperator == ComparisonOperator.LessThanEqual)
        {
            if (!(currentOperand.value1 is int))
            {
                statusDisplay.SetText("Size comparison operators require integers", true);
                return false;
            }
        }
        return true;
    }

    private void Evaluate()
    {
        // Evaluate each operand individually without considering logical operators (NOT operator already processed)
        foreach (var operand in operands)
        {
            evaluatedOperands.Add(operand.Evaluate());
        }
        EvaluateOperators(LogicalOperator.And);
        EvaluateOperators(LogicalOperator.Or);
        // This should leave only one evaluated operand left which is the result
    }

    private void EvaluateOperators(LogicalOperator operatorToEvaluate)
    {
        if (logicalOperators.Count == 0)
            return;
        // Loop through all the logical operators and evaluate it and update the list
        int logicalOperatorI = 0;
        for (int i = 0; i < evaluatedOperands.Count; i++)
        {
            // Notes to continue on tomorrow:
            // When you hit an AND operator, get the current operand and the next operand
            // Evaluate the AND operator and remove those two operands and replace the current
            // operand with the evaluation of the AND operator and remove the AND operator
            // The same should apply for the OR operator
            if (logicalOperators[logicalOperatorI] == operatorToEvaluate)
            {
                var operand1 = evaluatedOperands[i];
                var operand2 = evaluatedOperands[i + 1];
                var result = EvaluateOperator(operand1, operand2, operatorToEvaluate);

                evaluatedOperands.RemoveAt(i + 1);
                evaluatedOperands[i] = result;

                logicalOperators.RemoveAt(logicalOperatorI);
            }
            logicalOperatorI++;
        }
    }

    private bool EvaluateOperator(bool operand1, bool operand2, LogicalOperator logicalOperator)
    {
        switch (logicalOperator)
        {
            case LogicalOperator.And:
                return operand1 && operand2;
            case LogicalOperator.Or:
                return operand1 || operand2;
            case LogicalOperator.Not:
                return !operand1;
            default:
                throw new ArgumentException("Invalid logical operator");
        }
    }

    private bool LogicalOperatorCheck()
    {
        int count = logicalOperators.Count;
        foreach (var logicalOperator in logicalOperators)
        {
            if (logicalOperator == LogicalOperator.Not)
                count--;
        }

        return operands.Count - 1 == count;
    }

    public class Operand
    {
        public object value1 = null;
        public object value2 = null;
        public bool notValue1 = false;
        public bool notValue2 = false;
        public ComparisonOperator compOperator = ComparisonOperator.None;

        public Operand() {}

        public Operand(object value)
        {
            value1 = value;
        }

        public bool IsValid()
        {
            return value1 != null && value2 != null && compOperator != ComparisonOperator.None;
        }

        public bool Evaluate()
        {
            if (notValue1 || notValue2 || value2 == null)
            {
                // We're assuming we're working with booleans
                bool bool1 = (bool)value1;
                bool bool2 = false;
                bool bool2Exists = false;
                if (value2 != null)
                {
                    bool2 = (bool)value2;
                    bool2Exists = true;
                }
                if (notValue1) bool1 = !bool1;
                if (notValue2 && bool2Exists) bool2 = !bool2;

                if (!bool2Exists)
                    return bool1;

                if (compOperator == ComparisonOperator.Equal)
                {
                    return bool1 == bool2;
                }
                return bool1 != bool2;
            }
            switch (compOperator)
            {
                case ComparisonOperator.Equal:
                    return value1.Equals(value2);
                case ComparisonOperator.NotEqual:
                    return !value1.Equals(value2);
                case ComparisonOperator.LessThan:
                    return (int)value1 < (int)value2;
                case ComparisonOperator.LessThanEqual:
                    return (int)value1 <= (int)value2;
                case ComparisonOperator.GreaterThan:
                    return (int) value1 > (int)value2;
                case ComparisonOperator.GreaterThanEqual:
                    return (int) value1 >= (int)value2;
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            string not1 = notValue1 ? "!" : "";
            string not2 = notValue2 ? "!" : "";

            if (IsValid())
            {
                return not1 + value1 + " " + ComparisonOperatorAction.GetTypeName(compOperator) + " " + not2 + value2;
            }

            if (value1 == null && value2 == null)
            {
                return not1;
            }

            if (value2 == null && compOperator != ComparisonOperator.None)
            {
                return not1 + value1 + " " + ComparisonOperatorAction.GetTypeName(compOperator) + " " + not2;
            }

            return not1 + value1;
        }
    }
}

public enum LogicalOperator
{
    And,
    Or,
    Not
}

public enum ComparisonOperator
{
    Equal,
    NotEqual,
    LessThan,
    LessThanEqual,
    GreaterThan,
    GreaterThanEqual,
    None
}