using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoAction : Action
{
    public string locationName;
    public Vector2 teleportTo;

    public override void Execute()
    {
        LevelManager.instance.playerTransform.position = teleportTo;
    }

    public override string MouseOverText()
    {
        return "goto " + locationName + ";";
    }
}
