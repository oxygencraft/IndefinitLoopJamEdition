using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAction : Action
{
    public override void Execute()
    {
        LevelManager.instance.WinLevel();
        Complete();
    }

    public override string MouseOverText()
    {
        return "break;";
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "Player")
            return;
        Execute();
    }
}
