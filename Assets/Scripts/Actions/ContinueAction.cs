using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueAction : Action
{
    public bool deletesObjects = false;

    public override void Execute()
    {
        LevelManager.instance.FailLevel();
        Complete();
    }

    public override string MouseOverText()
    {
        return "continue;";
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "Player")
        {
            if (deletesObjects)
                Destroy(collider.gameObject);
            return;
        }
        Execute();
    }
}
