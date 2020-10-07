using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public List<Action> actions;
    public List<ActionsText> actionText;
    public Action currentAction = null;
    public Variable variablePlayerHolding = null;
    public Transform playerTransform;
    public TaskList taskList;
    public GameObject continueActionPrefab;

    private int currentActionI = 0;

    public void CompleteAction(Action action)
    {
        if (action != currentAction)
        {
            if (action is ContinueAction || action is BreakAction)
                return;
            // Worst piece of code lol ever must clean up
            Vector2 playerPosition = playerTransform.position;
            playerPosition.x += 1.5f;
            Instantiate(continueActionPrefab, playerPosition, Quaternion.identity);
            playerPosition = playerTransform.position;
            playerPosition.x += -1.5f;
            Instantiate(continueActionPrefab, playerPosition, Quaternion.identity);
            playerPosition = playerTransform.position;
            playerPosition.y += 1.5f;
            Instantiate(continueActionPrefab, playerPosition, Quaternion.identity);
            playerPosition = playerTransform.position;
            playerPosition.y += -1.5f;
            Instantiate(continueActionPrefab, playerPosition, Quaternion.identity);
        }

        if (currentActionI + 1 == actions.Count)
        {
            taskList.HighlightEndOfLoop();
            return;
        }

        currentAction = actions[++currentActionI];
        taskList.SetNewText(actionText[currentActionI]);
    }

    public void FailLevel()
    {
        GameManager.instance.FailLevel();
    }

    public void WinLevel()
    {
        GameManager.instance.WinLevel();
    }

    void Awake()
    {
        variablePlayerHolding = null;
        instance = this;
        currentAction = actions[0];
        taskList.SetNewText(actionText[0]);
    }

    void Start()
    {
        variablePlayerHolding = null;
    }

    void OnDestroy()
    {
        instance = null;
    }

    [System.Serializable]
    public class ActionsText
    {
        public string text;
        public int line;
    }
}