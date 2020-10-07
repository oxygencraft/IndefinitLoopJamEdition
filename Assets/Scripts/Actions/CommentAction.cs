using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentAction : Action
{
    public string[] comments;
    private int currentComment = 0;

    public override void Execute()
    {
        if (currentComment + 1 == comments.Length)
            currentComment = -1;

        statusDisplay.SetText(comments[++currentComment]);
    }

    public override string MouseOverText()
    {
        return comments[currentComment];
    }
}
