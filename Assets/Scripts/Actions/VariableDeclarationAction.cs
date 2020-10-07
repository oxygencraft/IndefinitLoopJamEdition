using System;

public class VariableDeclarationAction : Action
{
    public string variableName;
    public string variableType;

    public override void Execute()
    {
        if (LevelManager.instance.variablePlayerHolding != null && !LevelManager.instance.variablePlayerHolding.IsNull())
        {
            statusDisplay.SetText("Already holding variable");
        }
        var variable = new Variable(variableName, Type.GetType(variableType));
        LevelManager.instance.variablePlayerHolding = variable;
        Complete();
    }

    public override string MouseOverText()
    {
        string type = StatusDisplay.GetShortPrimitiveTypeName(Type.GetType(variableType));
        return type + " " + variableName + ";";
    }
}
