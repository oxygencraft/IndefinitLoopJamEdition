using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMonitor : MonoBehaviour
{
    public bool resetsToOriginalPosition;
    public GameObject monitoringGameObject;

    private GameObject originalGameObject;

    public void ResetAction(bool resetPosition)
    {
        if (!resetPosition)
        {
            originalGameObject.transform.SetParent(transform, false);
            originalGameObject.transform.localPosition = Vector2.zero;
            originalGameObject.transform.parent = null;
        }

        originalGameObject.SetActive(true);
        //originalGameObject.GetComponent<Action>().InitalizeActionMonitor();
    }

    public void ResetAction()
    {
        ResetAction(resetsToOriginalPosition);
    }

    void Start()
    {
        originalGameObject = Instantiate(monitoringGameObject, monitoringGameObject.transform);
        originalGameObject.GetComponent<Action>().isDuplicate = true;
        originalGameObject.SetActive(false);
        originalGameObject.transform.parent = null;
    }
}
