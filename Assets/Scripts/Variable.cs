using System;

[Serializable]
public class Variable
{
    public string name;
    private object value = null;
    public Type type;

    public Variable(string name, Type type)
    {
        this.name = name;
        this.type = type;
    }

    public Variable(string name, object value, Type type)
    {
        this.name = name;
        this.value = value;
        this.type = type;
    }

    public object GetValue()
    {
        return value;
    }

    public void SetValue(object value)
    {
        string valueName = value.GetType().Name;
        string typeName = type.Name;
        if (valueName != typeName)
            throw new InvalidCastException("Cannot convert a " + valueName + " to a " + typeName + ".");
        this.value = value;
    }

    public bool IsNull()
    {
        if (name == null && value == null && type == null)
            return true;
        return false;
    }
}