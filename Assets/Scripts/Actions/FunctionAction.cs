using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FunctionAction : Action
{
    public string functionName;
    public Parameter[] parameters;
    public int completeParameters = 0;

    [SerializeField]
    private FunctionCall called = new FunctionCall();

    void Awake()
    {
        foreach (var parameter in parameters)
        {
            parameter.Initalize();
        }
    }

    public override void Execute()
    {
        if (completeParameters == parameters.Length)
        {
            called.Invoke(parameters);
            Complete();
            return;
        }

        Variable variable = LevelManager.instance.variablePlayerHolding;
        if (variable == null || variable.IsNull() && completeParameters != parameters.Length)
        {
            statusDisplay.SetText("Not all parameters are complete", true);
            return;
        }

        Parameter parameterToComplete = parameters[completeParameters];
        if (variable.type.Name != parameterToComplete.type.Name)
        {
            statusDisplay.SetText("Object types do not match", true);
            return;
        }

        if (variable.GetValue() == null)
        {
            statusDisplay.SetText("Variable is null", true);
            return;
        }

        parameterToComplete.value = variable.GetValue();
        parameters[completeParameters++] = parameterToComplete;

        LevelManager.instance.variablePlayerHolding = null;
    }

    public override string MouseOverText()
    {
        string parametersText = "";
        bool firstParameter = true;

        foreach (var parameter in parameters)
        {
            if (firstParameter)
            {
                parametersText = parameter.GetParameterText();
                firstParameter = false;
                continue;
            }

            parametersText += ", " + parameter.GetParameterText();
        }

        return functionName + "(" + parametersText + ");";
    }

    [Serializable]
    public class FunctionCall : UnityEvent<Parameter[]> { }

    [Serializable]
    public class Parameter
    {
        public string name;
        public string typeName;
        public object value = null;
        public Type type;

        public Parameter(string name, string typeName)
        {
            this.name = name;
            this.typeName = typeName;
            type = Type.GetType(typeName);
        }

        public void Initalize()
        {
            type = Type.GetType(typeName);
        }

        public string GetParameterText()
        {
            string shortTypeName = StatusDisplay.GetShortPrimitiveTypeName(type);
            bool displayable = shortTypeName != type.Name;
            if (value == null)
            {
                return shortTypeName + " " + name;
            }
            if (!displayable)
            {
                return shortTypeName + " " + name + " [undisplayable]";
            }
            return shortTypeName + " " + name + " [" + value + "]";
        }
    }
}
