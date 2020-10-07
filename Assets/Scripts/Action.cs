using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public bool destroyOnComplete = true;
    public float distanceToInteract = 2f;
    public bool allowDistantInfoView = true;
    public bool resetAtOriginalPosition = false;

    [HideInInspector] 
    public bool isDuplicate = true;

    protected StatusDisplay statusDisplay;
    private bool didMouseActivateStatus = true;
    //private ActionMonitor actionMonitor;

    public abstract void Execute();

    public abstract string MouseOverText();

    public void Complete()
    {
        LevelManager.instance.CompleteAction(this);
        if (destroyOnComplete)
        {
            Destroy(gameObject);
            return;
        }
        //actionMonitor.ResetAction(false);
        Destroy(gameObject);
    }

    protected virtual void OnMouseDown()
    {
        if (!DistanceCheck())
        {
            if (!allowDistantInfoView)
                return;
            statusDisplay.SetText("[too far to interact]", true);
            return;
        }
        Execute();
        statusDisplay.SetText(MouseOverText());
    }

    protected virtual void OnMouseEnter()
    {
        if (!allowDistantInfoView && !DistanceCheck())
        {
            statusDisplay.SetText("[too far to view]", true);
            return;
        }
        statusDisplay.SetText(MouseOverText());
        didMouseActivateStatus = true;
    }

    protected virtual void OnMouseExit()
    {
        if (!didMouseActivateStatus)
            return;
        statusDisplay.ResetText();
        didMouseActivateStatus = false;
    }

    protected virtual bool DistanceCheck()
    {
        Vector2 playerPosition = LevelManager.instance.playerTransform.position;
        if (Vector2.Distance(playerPosition, gameObject.transform.position) > distanceToInteract)
            return false;
        return true;
    }

    /*public void InitalizeActionMonitor()
    {
        isDuplicate = false;
        GameObject monitorGameObject = new GameObject(name + " Monitor", typeof(ActionMonitor));
        monitorGameObject.transform.SetParent(transform);
        actionMonitor = monitorGameObject.GetComponent<ActionMonitor>();
        monitorGameObject.transform.localPosition = Vector2.zero;
    }*/

    protected virtual void OnDestroy()
    {
        if (didMouseActivateStatus)
            statusDisplay.ResetText();
    }

    protected virtual void Start()
    {
        statusDisplay = GameManager.instance.statusDisplay;
    }

    protected virtual void Awake()
    {
        if (isDuplicate)
            return;
        //InitalizeActionMonitor();
    }
}
